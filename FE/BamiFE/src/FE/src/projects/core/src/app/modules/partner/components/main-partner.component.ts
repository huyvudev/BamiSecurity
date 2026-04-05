import { PartnerService } from './../services/partner.service';
import { Component, Injector } from '@angular/core';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ComponentBase } from '@shared/component-base';
import { Partner, PartnerType } from '../model/partner.model';
import { PartnerTypeService } from '../services/partner-type.service';
import { finalize } from 'rxjs';
import { Page } from '@mylib-shared/models/page';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { DialogService } from 'primeng/dynamicdialog';
import { CreatePartnerComponent } from './create-partner/create-partner.component';

@Component({
	selector: 'app-main-partner',
	templateUrl: './main-partner.component.html',
	styleUrls: ['./main-partner.component.scss']
})
export class MainPartnerComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private partnerService: PartnerService,
		private dialogService: DialogService,
		private partnerTypeService: PartnerTypeService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý đối tác" },
			{ label: "Đối tác" }
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: Partner[] = [];
	partnerTypes: PartnerType[] = [];
	dataFilter: any

	pagePartnerType: Page = new Page()
	ngOnInit() {
		this.setColumn()
		this.genListAction(this.rows);
		this.genItemFilters();
		this.getPartnerType();
		this.setPage();
	}

	setColumn() {
		this.columns = [
			{ field: 'name', header: 'Tên đối tác', width: 15, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },

			{
				field: 'Loại đối tác', header: "Loại đối tác", width: 15, type: ETableColumnType.TEXT,
			},
			{
				field: 'createDate', header: "Ngày tạo", width: 15,

			},
			{
				field: 'numberBatch', header: "Số lô", width: 15,

			},
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}
		]
	}

	public setPage(event?: Page) {
		this.dataFilter = { ...this.dataFilter, ...event }
		this.isLoading = true;
		this.partnerService.getAll(this.page, this.dataFilter)
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

	createPartnerOrUpdate(partner?: Partner) {
		this.dialogService.open(CreatePartnerComponent, {
			header: "Tạo đối tác",
			styleClass: `p-dialog-custom customModal`,
			width: '586px',
			data: partner
		}).onClose.subscribe(result => {
			if (result) {

				this.setPage();
			}
		})
	}


	getPartnerType() {
		this.isLoading = true;
		this.pagePartnerType.pageSize = -1
		this.pagePartnerType.pageNumber = 1
		this.partnerTypeService.getAll(this.pagePartnerType)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.partnerTypes = res?.data.items
				this.genItemFilters();
			})
	}

	genListAction(data: any) {
		this.listAction = data.map(item => {
			const actions = [];

			actions.push({
				label: 'Cập nhật đối tác',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.createPartnerOrUpdate(item)
				}
			});

			actions.push({
				label: 'Xem',
				icon: 'pi pi-eye',
				command: () => {

				}
			});

			actions.push({
				label: 'Copy',
				icon: 'pi pi-copy',
				command: () => {

				}
			});

			actions.push({
				label: 'Xóa',
				icon: 'pi pi-trash',
				command: () => {
					this.deletePartner(item?.id)
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
				label: 'Tến đối tác'
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Trạng thái',
				variableReference: 'status',
				optionConfig: {
					data: this.partnerTypes,
					label: 'name',
					value: 'id'
				}
			}
		]
	}

	async deletePartner(id: number) {
		const acceptComfirm = await this.confirmAction("Bạn có chắc muốn xóa đối tác ?")
		if (acceptComfirm) {
			this.partnerService.delete(id)
				.pipe((finalize(() => this.isLoading = false)))
				.subscribe((res) => {
					if (this.checkStatusResponse(res, "Xóa thành công")) {
						this.setPage();
					}
				})
		}
	}
}
