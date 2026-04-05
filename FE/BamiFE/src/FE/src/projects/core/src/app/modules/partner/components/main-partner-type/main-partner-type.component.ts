import { Component, Injector } from '@angular/core';
import { ComponentBase } from '@shared/component-base';
import { PartnerTypeService } from '../../services/partner-type.service';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { PartnerType } from '../../model/partner.model';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { Page } from '@mylib-shared/models/page';
import { finalize } from 'rxjs';
import { CreatePartnerTypeComponent } from './create-partner-type/create-partner-type.component';

@Component({
	selector: 'app-main-partner-type',
	templateUrl: './main-partner-type.component.html',
	styleUrls: ['./main-partner-type.component.scss']
})
export class MainPartnerTypeComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private partnerTypeService: PartnerTypeService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{label:"Quản lý đối tác"},
			{ label: "Loại đối tác" },

		]);

	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: PartnerType[] = [];
	dataFilter: any;

	ngOnInit() {
		this.setColumn()
		this.genListAction(this.rows);
		this.genItemFilters();
		this.setPage();
	}
	setColumn() {
		this.columns = [
			{ field: 'id', header: '#ID', width: 15, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },

			{
				field: 'name', header: "Tên loại đối tác", width: 15, type: ETableColumnType.TEXT,
			},
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}
		]
	}

	public setPage(event?: Page) {
		this.dataFilter = { ...this.dataFilter, ...event }
		this.isLoading = true;
		this.partnerTypeService.getAll(this.page, this.dataFilter)
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

	genListAction(data: any) {
		this.listAction = data.map(item => {
			const actions = [];

			actions.push({
				label: 'Cập nhật loại đối tác',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createOrUpdatePartnerType(item)
				}
			});


			actions.push({
				label: 'Xóa',
				icon: 'pi pi-trash',
				command: () => {
					this.deletePartnerType(item?.id)
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
				label: 'Tến loại đối tác'
			},

		]
	}

	createOrUpdatePartnerType(partnerType?: PartnerType){
		this.dialogService.open(CreatePartnerTypeComponent, {
			header: "Tạo loại đối tác",
			styleClass: `p-dialog-custom customModal`,
			width: '586px',
			data: partnerType
		}).onClose.subscribe(result => {
			this.setPage();
		})
	}

	async deletePartnerType(id: number) {
		const acceptComfirm = await this.confirmAction("Bạn có chắc muốn xóa loại đối tác ?")
		if (acceptComfirm) {
			this.isLoading = true;
			this.partnerTypeService.delete(id)
				.pipe((finalize(() => this.isLoading = false)))
				.subscribe((res) => {
					if (this.checkStatusResponse(res, "Xóa thành công")) {
						this.setPage();
					}
				})
		}
	}
}
