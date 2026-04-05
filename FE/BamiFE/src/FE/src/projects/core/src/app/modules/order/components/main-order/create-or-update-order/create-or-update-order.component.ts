import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { Order, OrderDetail } from '../../../models/order.models';
import { required } from '@shared/validators/validator-common';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { EImageTable, ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { CreateOrUpdateOrderDetailComponent } from '../create-or-update-order-detail/create-or-update-order-detail.component';
import { OrderService } from '../../../services/order.service';
import { finalize } from 'rxjs';
import { OrderDetailService } from '../../../services/order-detail.service';
import { Brand } from '../../../../name-space/models/brand.model';
import { Page } from '@mylib-shared/models/page';
import { BrandService } from '../../../../name-space/services/brand.service';
import { Store } from '../../../../name-space/models/store.model';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { EStatusOder, StatusOrder } from '../../../const/orderStatus.models';

@Component({
	selector: 'app-create-or-update-order',
	templateUrl: './create-or-update-order.component.html',
	styleUrls: ['./create-or-update-order.component.scss']
})

export class CreateOrUpdateOrderComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private fb: FormBuilder,
		private dialogService: DialogService,
		private routeActive: ActivatedRoute,
		private breadcrumbService: BreadcrumbService,
		private orderService: OrderService,
		private brandService: BrandService,
		private orderDetailService: OrderDetailService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý đơn", routerLink: ['/order-management/order'] },
			{ label: "Tạo hoặc cập nhật đơn" }
		]);
	}
	order: Order
	postForm: FormGroup;

	isEdit: boolean = false;
	idOrder: any;
	hasData :boolean = true
	isCreate : boolean = false;

	columns: IColumn[] = [];
	details: OrderDetail[] = [];
	listAction: IAction[][] = [];

	itemFilters: IItemFilter[] = [];

	listBrand: Brand[] = [];
	listStores: Store[] = [];
	pageCustom: Page = new Page()

	EImageTable = EImageTable

	statusOrder = EStatusOder

	ngOnInit() {
		this.pageCustom.pageSize = -1
		this.pageCustom.pageNumber = 1

		if (this.routeActive.snapshot.paramMap.get('id')) {
			this.idOrder = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'))
			if (this.idOrder) {
				this.getDetail(this.idOrder);
			}

		}

		this.getBrand();

		this.setColumn();

		this.genItemFilters();

		this.setForm();

	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.order?.id],
			name: [this.order?.name, required()],
			address: [this.order?.address, required()],
			city: [this.order?.city, required()],
			state: [this.order?.state, required()],
			postalCode: [this.order?.postalCode, required()],
			country: [this.order?.country, required()],
			orderNumber: [this.order?.orderNumber, required()],
			email: [this.order?.email],
			address2: [this.order?.address2],
			phone: [this.order?.phone],
			tax: [this.order?.tax,],
			details: [this.details],
			brandId: [this.order?.brandId],
			namespace: [this.order?.namespace],
			storeId: [0]
		});

		this.postForm.get('brandId').valueChanges.subscribe(value => {
			if (value) {
				this.getStore(value);
			}
		});

		this.postForm.get('storeId').valueChanges.subscribe(value => {
			if (value) {
				const nameSpace = this.listStores.find(i => i.id === value);
				console.log(nameSpace)
				this.postForm.get('namespace').setValue(nameSpace?.name);
			}
		});

		if (!this.order?.id) {
			this.isCreate = true
			this.postForm.removeControl('id');
		}
		else {
			this.isCreate = false
			this.postForm.disable();
		}
	}

	getBrand() {
		this.isLoading = true;
		this.brandService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.listBrand = res?.data.items
				console.log(this.listBrand)
			})
	}

	getDetail(id?: any) {
		let idOrder = this.unpadId(id)
		this.isEdit = false;
		this.hasData = true;
		this.isLoading = true;
		this.orderService.getById(idOrder)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((response) => {
				if (this.checkStatusResponse(response)) {
					this.order = response.data
					console.log(this.order)
					this.details = this.order.details.map(detail => {
						return {
							...detail,
							designSale: {
								saleDesignBack: detail.saleDesignBack,
								saleDesignFront: detail.saleDesignFront,
								saleDesignHood: detail.saleDesignHood,
								saleDesignSleeves: detail.saleDesignSleeves
							}
						}
					});
					this.genListAction(this.details)
					this.setForm();
				}
				else{
					this.hasData = false
				}
			})
	}

	onSubmit() {
		if (!this.inValidForm(this.postForm)) {
			const body: Order = this.postForm.value;
			let successMessage = body?.id ? "Cập nhật chi tiết đơn hàng thành công !" : "Thêm chi tiết đơn hàng thành công"

			if (body.id) {
				this.orderService.update(body)
					.pipe(finalize(() => this.isLoading = false))
					.subscribe((response) => {
						if (this.checkStatusResponse(response, successMessage)) {
							this.getDetail(this.idOrder);
						}
					})
			}
			else {
				this.orderService.create(body)
					.pipe(finalize(() => this.isLoading = false))
					.subscribe((response) => {
						if (this.checkStatusResponse(response, successMessage)) {
							this.router.navigate([`/order-management/order/detail/${this.cryptEncode(response?.data.id)}`])
						}
					})
			}
		}
	}

	activeEdit() {
		this.isEdit = true;
		this.postForm.enable();
	}

	setColumn() {
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{ field: 'type', header: 'Type', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{
				field: 'size', header: "Size", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'sellerSku', header: "Mã Sku", width: 10,

			},
			{
				field: 'color', header: "Color", width: 5,

			},
			{
				field: 'quantity', header: "Số lượng", width: 6,

			},
			{
				field: 'mockUpFront', header: "Mock up", width: 10, type: ETableColumnType.IMAGE_SET, otherType: ETableColumnType.IS_MOCKUP_FRONT,
				isFrozen: true, alignFrozen: ETableFrozen.LEFT,

			},
			{
				field: 'mockUpBack', header: "", width: 10, type: ETableColumnType.IMAGE_SET, otherType: ETableColumnType.IS_MOCKUP_BACK,
				isFrozen: true, alignFrozen: ETableFrozen.LEFT,

			},
			{
				field: 'designSale', header: "Design", width: 15, type: ETableColumnType.DESIGN,
			},
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}

		]
	}

	genListAction(data?: any) {
		this.listAction = data.map((item, index) => {
			const actions = [];

			actions.push({
				label: 'Thông tin chi tiết',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createOrUpdateOrderDetail(item, index)
				}
			});

			actions.push({
				label: 'Xóa chi tiết đơn hàng',
				icon: 'pi pi-trash',
				command: () => {
					this.onDelete(item, index)
				}
			});
			return actions;
		});
	}

	async approveOrder() {
		const acceptComfirm = await this.confirmAction("Bạn có chắc muốn duyệt đơn hàng?")
		if (acceptComfirm) {
			this.isLoading = true;
			this.orderService.approve(this.idOrder)
				.pipe(finalize(() => this.isLoading = false))
				.subscribe((response) => {
					if (this.checkStatusResponse(response, "Duyệt đơn hàng thành công")) {
						this.getDetail(this.idOrder);
					}
				})
		}
	}


	back() {
		this.router.navigate([`order-management/order`]);
	}

	getStore(idBrand: number) {
		this.brandService.findById(idBrand)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((res) => {
				this.listStores = res.data.stores
				console.log(this.listStores)
			})
	}

	createOrUpdateOrderDetail(item?: OrderDetail, index?: number, isAdd?: boolean) {
		const ref = this.dialogService.open(CreateOrUpdateOrderDetailComponent, {
			header: isAdd ? 'Thêm chi tiết đơn hàng' : 'Cập nhật chi tiết đơn hàng',
			width: '1000px',
			data: {
				item,
				orderId: this.idOrder,
				index: index
			}
		})

		ref.onClose.subscribe((response) => {
			if (response) {
				if (response.isCreate) {
					if (response.index !== undefined && response.index != null) {
						this.details[response.index] = response.body
						this.handleOrderDetail(false)
					} else {
						this.details.push(response.body)
						this.handleOrderDetail(true)
					}
				}
				else {
					this.getDetail(this.idOrder);
				}
			}
		});

	}

	async onDelete(item?: OrderDetail, index?: any) {
		let accept = await this.confirmAction('Xác nhận xóa đơn hàng chi tiết?')
		if (accept) {
			if (item.id) {
				this.isLoading = true;
				this.orderDetailService.delete(item?.id)
					.pipe(finalize(() => this.isLoading = false))
					.subscribe((response) => {
						if (this.checkStatusResponse(response, 'Xóa đơn hàng chi tiết thành công!')) {
							this.getDetail(this.idOrder);
						}
					})
			}
			else {
				this.details.splice(index, 1);
				console.log(this.details)
				this.handleOrderDetail(true)
			}
		}
	}

	cancel() {
		this.postForm.disable();
		this.isEdit = false;
	}

	handleOrderDetail(isCreateOrRemove: boolean) {
		this.details = [...this.details];
		this.postForm.get('details').setValue(this.details);
		this.genListAction(this.details);

		if (isCreateOrRemove) {
			this.page.totalItems = this.details.length;
		}
	}

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'id',
				placeholder: 'Tìm kiếm',
			},
		]
	}

	
	unpadId(paddedId: string): number {
		if (paddedId != null) {
			const cleanedId = paddedId.startsWith('#') ? paddedId.slice(1) : paddedId;
			return parseInt(cleanedId, 10);
		}
		else {
			return 0
		}
	}
}
