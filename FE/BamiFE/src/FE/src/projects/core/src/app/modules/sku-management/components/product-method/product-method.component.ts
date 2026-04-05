import { Component, Injector } from '@angular/core';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { ProductMethodService } from '../../services/product-method.service';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { ProductMethod } from '../../model/product-method.model';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { finalize } from 'rxjs';
import { Page } from '@mylib-shared/models/page';
import { CreateOrUpdateProductMethodComponent } from './create-or-update-product-method/create-or-update-product-method.component';

@Component({
	selector: 'app-product-method',
	templateUrl: './product-method.component.html',
	styleUrls: ['./product-method.component.scss']
})
export class ProductMethodComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private productMethodService: ProductMethodService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý SKU" },
			{ label: "Phương thức sản xuất" }
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: ProductMethod[] = [];
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
		this.productMethodService.getAll(this.page, this.dataFilter)
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
			{ field: 'name', header: 'Tên phương thức sản xuất', width: 15, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },

			{
				field: 'code', header: "Code", width: 15, type: ETableColumnType.TEXT,
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
				label: 'Cập nhật phương thức sản xuất',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createOrUpdateProductMethod(item)
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

	createOrUpdateProductMethod(productMethod?: ProductMethod) {
		this.dialogService.open(CreateOrUpdateProductMethodComponent, {
			header: productMethod ? "Cập nhật phương thức sản xuất" : "Tạo phương thức sản xuất",
			styleClass: `p-dialog-custom customModal`,
			width: '586px',
			data: productMethod
		}).onClose.subscribe(result => {
			if (result) {

				this.setPage();
			}
		})
	}

	async onDelete(material?: ProductMethod) {
		const accept = await this.confirmAction('Bạn có chắc chắn muốn xóa?')
		if (accept) {

			this.isLoading = true;
			this.productMethodService.delete(material.id)
				.pipe((finalize(() => this.isLoading = false)))
				.subscribe((res) => {
					if (this.checkStatusResponse(res, "Xóa thành công")) {
						this.setPage();
					}
				})
		}
	}
}


