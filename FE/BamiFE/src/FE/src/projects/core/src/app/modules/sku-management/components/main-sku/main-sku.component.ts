import { SkuService } from './../../services/sku.service';
import { MaterialService } from './../../services/material.service';
import { ProductMethod } from './../../model/product-method.model';
import { Component, Injector } from '@angular/core';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { ProductMethodService } from '../../services/product-method.service';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { Sku } from '../../../order-item/models/sku.model';
import { Material } from '../../model/material.model';
import { SkuBase } from '../../model/skubase.models';
import { finalize } from 'rxjs';
import { SkuBaseService } from '../../services/sku-base.service';
import { Page } from '@mylib-shared/models/page';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { CreateOrUpdateSkuComponent } from './create-or-update-sku/create-or-update-sku.component';
import { Router } from '@angular/router';

@Component({
	selector: 'app-main-sku',
	templateUrl: './main-sku.component.html',
	styleUrls: ['./main-sku.component.scss']
})
export class MainSkuComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private skuService: SkuService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý SKU" },
			{ label: "SKU" }
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: Sku[] = [];
	productMethod: ProductMethod[] = [];
	material: Material[] = [];
	skuBase: SkuBase[] = [];
	dataFilter: any;

	ngOnInit() {
		this.setColumn()
		this.genListAction(this.rows);
		this.genItemFilters();
		this.setPage();
	}

	public setPage(event?: Page) {
		this.dataFilter = { ...this.dataFilter, ...event }
		this.isLoading = true;
		this.skuService.getAll(this.page, this.dataFilter)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.rows = this.page.getRowLoadMore(this.rows, res?.data?.items);
				this.page.totalItems = res?.data?.totalItems;
				if (this.rows?.length) {
					this.genListAction(this.rows);
				}
			});
	}

	setColumn() {
		this.columns = [
			{
				field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT,
			},

			{ field: 'code', header: 'Code', width: 15, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },

			{
				field: 'description', header: "Description", width: 15, type: ETableColumnType.TEXT,
			},
			{
				field: 'isBigSize', header: 'Kích thước lớn', width: 10, displaySettingColumn: false, alignFrozen: ETableFrozen.RIGHT,
				type: ETableColumnType.CHECKBOX_CHANGE, isHideOptionCheckbox: true
			},
			{
				field: 'isActive', header: 'Hoạt động', width: 10, displaySettingColumn: false, alignFrozen: ETableFrozen.RIGHT,
				type: ETableColumnType.CHECKBOX_CHANGE, isHideOptionCheckbox: true
			},
			{
				field: 'needToReview', header: 'Cần phải xem lại', width: 10, displaySettingColumn: false, alignFrozen: ETableFrozen.RIGHT,
				type: ETableColumnType.CHECKBOX_CHANGE, isHideOptionCheckbox: true
			},
			{
				field: 'needManageMaterials', header: 'Cần quản lý chất liệu', width: 10, displaySettingColumn: false, alignFrozen: ETableFrozen.RIGHT,
				type: ETableColumnType.CHECKBOX_CHANGE, isHideOptionCheckbox: true
			},
			{
				field: 'allowQcMultipleItems', header: 'Cho phép QC nhiều item', width: 10, displaySettingColumn: false, alignFrozen: ETableFrozen.RIGHT,
				type: ETableColumnType.CHECKBOX_CHANGE, isHideOptionCheckbox: true
			},
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}
		]
	}

	genListAction(data: any) {
		this.listAction = data.map(item => {
			const actions = [];

			actions.push({
				label: 'Cập nhật Product Method',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createOrUpdateSku(item?.id)
				}
			});

			actions.push({
				label: 'Xóa',
				icon: 'pi pi-trash',
				command: () => {
					this.onDelete(item)
				}
			});

			return actions;
		});
	}

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'keyword',
				placeholder: 'Tìm kiếm',
			},

		]
	}


	createOrUpdateSku(id?: number) {
		this.router.navigate([`/sku-management/sku/detail/${this.cryptEncode(id)}`])
	}


	createSku() {
		this.router.navigate(['/sku-management/sku/create'])
	}

	async onDelete(sku?: Sku) {
		const accept = await this.confirmAction('Bạn có chắc chắn muốn xóa?')
		if (accept) {

			this.isLoading = true;
			this.skuService.delete(sku.id)
				.pipe((finalize(() => this.isLoading = false)))
				.subscribe((res) => {
					if (this.checkStatusResponse(res, "Xóa thành công")) {
						this.setPage();
					}
				})
		}
	}

}
