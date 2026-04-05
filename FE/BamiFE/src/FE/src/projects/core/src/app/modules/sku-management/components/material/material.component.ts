import { Component, Injector } from '@angular/core';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { Material } from '../../model/material.model';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { MaterialService } from '../../services/material.service';
import { Page } from '@mylib-shared/models/page';
import { finalize } from 'rxjs';
import { CreateOrUpdateMaterialComponent } from './create-or-update-material/create-or-update-material.component';

@Component({
	selector: 'app-material',
	templateUrl: './material.component.html',
	styleUrls: ['./material.component.scss']
})
export class MaterialComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private materialService: MaterialService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý SKU" },
			{ label: "Chất liệu" }
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: Material[] = [];
	dataFilter: any

	ngOnInit() {
		this.setColumn()
		this.genListAction(this.rows);
		this.genItemFilters();
		this.setPage();
	}

	public setPage(event?: Page) {
		this.dataFilter = { ...this.dataFilter, ...event }
		this.isLoading = true;
		this.materialService.getAll(this.page, this.dataFilter)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.rows = this.page.getRowLoadMore(this.rows, res?.data?.items);
				this.page.totalItems = res?.data?.totalItems;
				if (this.rows?.length) {
					this.genListAction(this.rows);
				}
			}
			);
	}

	setColumn() {
		this.columns = [
			{
				field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT,
			},
			{ field: 'name', header: 'Tên Chất liệu', width: 15, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },

			{
				field: 'code', header: "Code", width: 15, type: ETableColumnType.TEXT,
			},
			{
				field: 'description', header: "Mô tả", width: 15,

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
				label: 'Cập nhật Chất liệu',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createOrUpdateMaterial(item)
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

	createOrUpdateMaterial(material?: Material) {
		this.dialogService.open(CreateOrUpdateMaterialComponent, {
			header: material ? "Cập nhật Chất liệu" : "Tạo Chất liệu",
			styleClass: `p-dialog-custom customModal`,
			width: '586px',
			data: material
		}).onClose.subscribe(result => {
			if (result) {

				this.setPage();
			}
		})
	}

	async onDelete(material?: Material) {
		const accept = await this.confirmAction('Bạn có chắc chắn muốn xóa?')
		if (accept) {

			this.isLoading = true;
			this.materialService.delete(material.id)
				.pipe((finalize(() => this.isLoading = false)))
				.subscribe((res) => {
					if (this.checkStatusResponse(res, "Xóa thành công")) {
						this.setPage();
					}
				})
		}
	}
}

