import { SkuService } from './../../../services/sku.service';
import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ComponentBase } from '@shared/component-base';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { MaterialService } from '../../../services/material.service';
import { ProductMethodService } from '../../../services/product-method.service';
import { SkuBaseService } from '../../../services/sku-base.service';
import { Material } from '../../../model/material.model';
import { ProductMethod } from '../../../model/product-method.model';
import { SkuBase } from '../../../model/skubase.models';
import { Sku } from '../../../model/sku.model';
import { required } from '@shared/validators/validator-common';
import { finalize } from 'rxjs';
import { Page } from '@mylib-shared/models/page';
import { ActivatedRoute, Router } from '@angular/router';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { SkuSize } from '../../../model/sku-size.model';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { CreateOrUpdateSkuSizeComponent } from '../create-or-update-sku-size/create-or-update-sku-size.component';
import { SkuSizeService } from '../../../services/sku-size.service';

@Component({
	selector: 'app-create-or-update-sku',
	templateUrl: './create-or-update-sku.component.html',
	styleUrls: ['./create-or-update-sku.component.scss']
})

export class CreateOrUpdateSkuComponent extends ComponentBase {
	constructor(
		injector: Injector,
		private fb: FormBuilder,
		private router: Router,
		private productMethodService: ProductMethodService,
		private materialService: MaterialService,
		private skuBaseService: SkuBaseService,
		private skuService: SkuService,
		private skuSizeService: SkuSizeService,
		private dialogService: DialogService,
		private routeActive: ActivatedRoute,
	) {
		super(injector)
	}

	postForm: FormGroup;
	sku: Sku

	columns: IColumn[] = [];
	skuSizes: SkuSize[] = [];
	listAction: IAction[][] = [];

	idSku: any
	ListMaterails: Material[] = [];
	ListProductMethod: ProductMethod[] = [];
	ListSkuBase: SkuBase[] = [];
	isEdit: boolean = false;

	pageCustom: Page = new Page()

	ngOnInit() {
		this.pageCustom.pageSize = -1
		this.pageCustom.pageNumber = 1

		if (this.routeActive.snapshot.paramMap.get('id')) {
			this.idSku = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'))
			if (this.idSku) {
				this.getSku(this.idSku);
			}

		}

		this.setForm();

		this.getMaterial();

		this.getProductMethod();

		this.getSkuBase();

		this.setColumn();
	}

	getMaterial() {
		this.isLoading = true;
		this.materialService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.ListMaterails = res?.data.items
			})
	}

	getSku(idSku: number) {
		this.isLoading = true;
		this.skuService.findById(idSku)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.sku = res?.data
				this.skuSizes = this.sku.sizes;
				this.genListAction(this.skuSizes)
				this.setForm();
			})
	}

	getProductMethod() {
		this.isLoading = true;
		this.productMethodService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.ListProductMethod = res?.data.items
			})
	}

	getSkuBase() {
		this.isLoading = true;
		this.skuBaseService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.ListSkuBase = res?.data.items
			})
	}

	setForm() {
		console.log(this.sku)
		this.postForm = this.fb.group({
			id: [this.sku?.id],
			code: [this.sku?.code, required()],
			description: [this.sku?.description],
			isBigSize: [this.sku?.isBigSize || false],
			isActive: [this.sku?.isActive || false],
			needToReview: [this.sku?.needToReview || false],
			needManageMaterials: [this.sku?.needManageMaterials || false],
			allowQcMultipleItems: [this.sku?.allowQcMultipleItems || false],
			skuBaseId: [this.sku?.skuBaseId],
			materialId: [this.sku?.materialId],
			productMethodId: [this.sku?.productMethodId],
			skuSizeList: [this.skuSizes]
		});

		if (!this.sku?.id) {
			this.postForm.removeControl('id');
		}
		else {
			this.postForm.disable();
		}

		this.postForm.valueChanges.subscribe(value => {
			if (value?.code !== '') {
				const skuBase = this.ListSkuBase.find(i => i.id === value?.skuBaseId);
				const material = this.ListMaterails.find(i => i.id === value?.materialId);
				const productMethod = this.ListProductMethod.find(i => i.id === value?.productMethodId);

				if (skuBase && material && productMethod) {
					const newCode = `${skuBase.code}_${material.code}_${productMethod.code}`;
					if (this.postForm.get('code')?.value !== newCode) {
						this.postForm.get('code')?.setValue(newCode, { emitEvent: false });
					}
				}
			}
		});
	}

	onSumbit() {
		if (!this.inValidForm(this.postForm)) {
			const body: Sku = this.postForm.value;
			this.isLoading = true;
			let successMessage = body.id ? "Cập nhật Sku thành công !" : 'Thêm Sku thành công !'
			if (body.id) {
				this.skuService.update(body)
					.pipe((finalize(() => this.isLoading = false)))
					.subscribe((res) => {
						if (this.checkStatusResponse(res, successMessage)) {
							this.getSku(this.idSku);
						}
					})
			}
			else {
				this.skuService.create(body)
					.pipe((finalize(() => this.isLoading = false)))
					.subscribe((res) => {
						if (this.checkStatusResponse(res, successMessage)) {
							this.router.navigate([`/sku-management/sku/detail/${this.cryptEncode(res?.data.id)}`])
						}
					})
			}
		}
	}

	back() {
		this.router.navigate([`sku-management/sku`]);
	}

	activeEdit() {
		this.isEdit = true;
		this.postForm.enable();
	}

	cancel() {
		this.postForm.disable();
		this.isEdit = false;
	}

	setColumn() {
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{ field: 'name', header: 'Name', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{
				field: 'width', header: "Chiều rộng", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'height', header: "Độ dày", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'length', header: "Chiều dài", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'weight', header: "Nặng", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'additionalWeight', header: "Trọng lượng tiêu chuẩn", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'baseCost', header: "Giá tiêu chuẩn", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'costInMeters', header: "Giá theo mét", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'weightByVolume', header: "Trọng lượng theo thể tích", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: 'packageDescription', header: "Mô tả đóng gói", width: 5, type: ETableColumnType.TEXT,
			},
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}
		]
	}

	createOrUpdateMockupSku(item?: SkuSize, index?: number, isAdd?: boolean) {
		const ref = this.dialogService.open(CreateOrUpdateSkuSizeComponent, {
			header: isAdd ? 'Thêm sku size' : 'Cập nhật sku size',
			width: '1000px',
			data: {
				item,
				orderId: this.idSku,
				index: index
			}
		})

		ref.onClose.subscribe((response) => {
			if (response) {
				if (response.isCreate) {
					if (response.index !== undefined && response.index != null) {
						this.skuSizes[response.index] = response.body
						this.handleOrderDetail(false)
					} else {
						this.skuSizes.push(response.body)
						this.handleOrderDetail(true)
					}
				}
				else {
					this.getSku(this.idSku);
				}
			}
		});

	}

	handleOrderDetail(isCreateOrRemove: boolean) {
		this.skuSizes = [...this.skuSizes];
		this.postForm.get('skuSizes').setValue(this.skuSizes);
		this.genListAction(this.skuSizes);

		if (isCreateOrRemove) {
			this.page.totalItems = this.skuSizes.length;
		}
	}

	genListAction(data?: any) {
		this.listAction = data.map((item, index) => {
			const actions = [];

			actions.push({
				label: 'Thông tin chi tiết',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createOrUpdateMockupSku(item, index)
				}
			});

			actions.push({
				label: 'Xóa sku size',
				icon: 'pi pi-trash',
				command: () => {
					this.onDelete(item, index)
				}
			});
			return actions;
		});
	}

	async onDelete(item?: Sku, index?: any) {
		let accept = await this.confirmAction('Xác nhận xóa sku size?')
		if (accept) {
			if (item.id) {
				this.isLoading = true;
				this.skuSizeService.delete(item?.id)
					.pipe(finalize(() => this.isLoading = false))
					.subscribe((response) => {
						if (this.checkStatusResponse(response, 'Xóa sku size thành công!')) {
							this.getSku(this.idSku);
						}
					})
			}
			else {
				this.skuSizes.splice(index, 1);
				this.handleOrderDetail(true)
			}
		}
	}
}
