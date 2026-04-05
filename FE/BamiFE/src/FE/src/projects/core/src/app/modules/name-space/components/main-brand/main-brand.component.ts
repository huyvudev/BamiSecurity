import { Component, Injector } from '@angular/core';
import { ComponentBase } from '@shared/component-base';
import { BrandService } from '../../services/brand.service';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { DialogService } from 'primeng/dynamicdialog';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { Brand } from '../../models/brand.model';
import { Page } from '@mylib-shared/models/page';
import { finalize } from 'rxjs';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { CreateOrUpdateBrandComponent } from './create-or-update-brand/create-or-update-brand.component';
import { Router } from '@angular/router';

@Component({
	selector: 'app-main-brand',
	templateUrl: './main-brand.component.html',
	styleUrls: ['./main-brand.component.scss']
})
export class MainBrandComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private brandService: BrandService,
		private breadcrumbService: BreadcrumbService,
		private router: Router,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý name space" },
			{ label: "Brand" }
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: Brand[] = [];
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
		this.brandService.getAll(this.page, this.dataFilter)
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
			{
				field: 'name', header: "Name", width: 15, type: ETableColumnType.TEXT,
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
				label: 'Cập nhật brand',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createOrUpdateBrand(item)
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

	createOrUpdateBrand(brand?: Brand) {
		if(brand){
			this.router.navigate([`/name-space/brand/detail/${this.cryptEncode(brand?.id)}`])
		}
		else{
			this.router.navigate([`/name-space/brand/create`])
		}
	}

	async onDelete(brand?: Brand) {
		const accept = await this.confirmAction('Bạn có chắc chắn muốn xóa?')
		if (accept) {
			this.isLoading = true;
			this.brandService.delete(brand.id)
				.pipe((finalize(() => this.isLoading = false)))
				.subscribe((res) => {
					if (this.checkStatusResponse(res, "Xóa thành công")) {
						this.setPage();
					}
				})
		}
	}
}
