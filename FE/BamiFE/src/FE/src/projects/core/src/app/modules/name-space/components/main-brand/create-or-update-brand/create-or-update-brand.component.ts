import { BrandService } from './../../../services/brand.service';
import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ComponentBase } from '@shared/component-base';
import { Brand } from '../../../models/brand.model';
import { required } from '@shared/validators/validator-common';
import { finalize } from 'rxjs';
import { Store } from '../../../models/store.model';
import { ActivatedRoute, Router } from '@angular/router';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { DialogService } from 'primeng/dynamicdialog';
import { MainStoreComponent } from '../../main-store/main-store.component';
import { StoreService } from '../../../services/store.service';

@Component({
	selector: 'app-create-or-update-brand',
	templateUrl: './create-or-update-brand.component.html',
	styleUrls: ['./create-or-update-brand.component.scss']
})
export class CreateOrUpdateBrandComponent extends ComponentBase {
	constructor(
		injector: Injector,
		private fb: FormBuilder,
		private router: Router,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
		private routeActive: ActivatedRoute,
		private brandService: BrandService,
		private storeService :StoreService
	) {
		super(injector)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản Namespace", routerLink: ['/name-space/brand'] },
			{ label: "Brand" }
		]);
	}

	postForm: FormGroup;
	idBrand: any;
	brand: Brand;
	stores: Store[] =[];

	isEdit: boolean = false;
	columns: IColumn[] = [];
	listAction: IAction[][] = [];

	ngOnInit() {

		if (this.routeActive.snapshot.paramMap.get('id')) {
			this.idBrand = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'))
			if (this.idBrand) {
				this.getDetail(this.idBrand);
			}

		}
		this.setColumn();
		this.setForm();
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.brand?.id],
			name: [this.brand?.name, required()],
			stores : [this.stores]

		});

		if (!this.brand?.id) {
			this.postForm.removeControl('id');
		}
		
		else {
			this.postForm.disable();
		}
	}

	onSumbit() {
		if (!this.inValidForm(this.postForm)) {
			const body: Brand = this.postForm.value;
			this.isLoading = true;
			console.log(body)
			let successMessage = body.id ? "Cập nhật brand thành công !" : 'Thêm brand thành công !'
			if (body.id) {
				this.brandService.update(body)
					.pipe((finalize(() => this.isLoading = false)))
					.subscribe((res) => {
						if (this.checkStatusResponse(res, successMessage)) {
							this.cancel()
							this.router.navigate([`/name-space/brand/detail/${this.cryptEncode(res?.data.id)}`])
						}
					})
			}

			else {
				this.brandService.create(body)
					.pipe((finalize(() => this.isLoading = false)))
					.subscribe((res) => {
						if (this.checkStatusResponse(res, successMessage)) {
							this.cancel()
							this.router.navigate([`/name-space/brand/detail/${this.cryptEncode(res?.data.id)}`])
						}
					})
			}
		}
	}

	getDetail(id?: any) {
		this.isEdit = false;
		this.isLoading = true;
		this.brandService.findById(id)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((response) => {
				if (this.checkStatusResponse(response)) {
					this.brand = response.data
					this.stores = this.brand.stores
					this.genListAction(this.stores)
					this.setForm();
				}
			})
	}

	setColumn() {
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{ field: 'name', header: 'Name', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}

		]
	}

	activeEdit() {
		this.isEdit = true;
		this.postForm.enable();
	}

	back() {
		this.router.navigate([`name-space/brand`]);
	}

	cancel() {
		this.postForm.disable();
		this.isEdit = false;
	}

	genListAction(data?: any) {
		this.listAction = data.map((item, index) => {
			const actions = [];
			actions.push({
				label: 'Thông tin chi tiết',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createOrUpdateStore(item, index)
				}
			});

			actions.push({
				label: 'Xóa Order Detail',
				icon: 'pi pi-trash',
				command: () => {
					this.onDelete(item, index)
				}
			});
			return actions;
		});
	}

	createOrUpdateStore(item?: Store, index?: number, isAdd?: boolean){
		const ref = this.dialogService.open(MainStoreComponent, {
			header: isAdd ? 'Thêm OrderDetail' : 'Cập nhật OrderDetail',
			width: '1000px',
			data: {
				item,
				brandId: this.idBrand,
				index: index
			}
		})

		ref.onClose.subscribe((response) => {
			if (response) {
				if (response.isCreate) {
					if (response.index !== undefined && response.index != null) {
						this.stores[response.index] = response.body
						this.handleStore(false)
					} else {
						this.stores.push(response.body)
						this.handleStore(true)
					}
				}
				else {
					this.getDetail(this.idBrand);
				}
			}
		});
	}


	handleStore(isCreateOrRemove: boolean) {
		this.stores = [...this.stores];
		this.postForm.get('stores').setValue(this.stores);
		this.genListAction(this.stores);

		if (isCreateOrRemove) {
			this.page.totalItems = this.stores.length;
		}
	}

	async onDelete(item?: Store, index?: any) {
		let accept = await this.confirmAction('Xác nhận xóa orderId?')
		if (accept) {
			if (item.id) {
				this.isLoading = true;
				this.storeService.delete(item?.id)
					.pipe(finalize(() => this.isLoading = false))
					.subscribe((response) => {
						if (this.checkStatusResponse(response, 'Xóa Order Detail thành công!')) {
							this.getDetail(this.idBrand);
						}
					})
			}
			else {
				this.stores.splice(index, 1);
				console.log(this.stores)
				this.handleStore(true)
			}

		}
	}

}
