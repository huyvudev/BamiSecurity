import { SkuService } from './../../sku-management/services/sku.service';
import { OrderItemService } from './../services/order-item.service';
import { OrderItem } from './../models/order-item.model';
import { Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentBase } from '@shared/component-base';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { StautsFilePrint } from '../const/status-File.const';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { EFormatDateDisplay } from '@mylib-shared/consts/base.consts';
import { StatusOrderItem } from '../const/status-item.const';
import { finalize } from 'rxjs';
import { Page } from '@mylib-shared/models/page';
import { Sku } from '../../sku-management/model/sku.model';
import { Brand } from '../../name-space/models/brand.model';
import { BrandService } from '../../name-space/services/brand.service';
import { Dialog } from 'primeng/dialog';
import { DialogService } from 'primeng/dynamicdialog';
import { CreateBatchComponent } from '../../batch/components/create-batch/create-batch.component';
import { BatchService } from '../../batch/services/batch.service';
@Component({
	selector: 'app-main-order-item',
	templateUrl: './main-order-item.component.html',
	styleUrls: ['./main-order-item.component.scss']
})
export class MainOrderItemComponent extends ComponentBase {

	constructor(
		inj: Injector,
		private orderItemService: OrderItemService,
		private skuService: SkuService,
		private dialogService: DialogService,
		private brandService: BrandService,
		private batchService: BatchService,
		private router: Router,

	) {
		super(inj)
	}

	ListOrDerItems: OrderItem[] = []

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: OrderItem[] = [];

	currentSku: number = -1
	curentStatusOrderItem: number = 0;
	dataFilter: any

	numberItems = StautsFilePrint.numberItems;
	autoApproved = StautsFilePrint.autoApproved;
	statusItems = StatusOrderItem.statusItems;
	pageCustom: Page = new Page()
	data: any
	ListSku: Sku[] = [];

	listBrand: Brand[] = [];

	ngOnInit() {
		this.pageCustom.pageSize = -1
		this.pageCustom.pageNumber = 1
		this.getSku();
		this.getBrand();
		this.setPage();
		this.genItemFilters()
		this.genListAction(this.ListOrDerItems);
	}

	setPage(event?: any) {
		console.log(event)
		const { dateCreate, ...data } = event || {};
		if (Array.isArray(dateCreate) && dateCreate.length > 0) {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				startDate: dateCreate[0],
				endDate: dateCreate[1],
			};
		} else {
			this.dataFilter = {
				...this.dataFilter,
				...data,
			};
		}
		this.isLoading = true;
		this.orderItemService.getAll(this.page, this.dataFilter)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.rows = this.page.getRowLoadMore(this.rows, res?.data?.items);
				this.page.totalItems = res?.data?.totalItems;
				this.rows = this.rows.map(row => {
					return {
						...row,
						design: {
							saleDesignBack: row?.designBack,
							saleDesignFront: row?.designFront,
							saleDesignHood: row?.designHood,
							saleDesignSleeves: row?.designSleeves,
						},
						template: {
							code: this.padId(row?.orderId, row.itemIndex),
							namespace: row.namespace,
							orderNumber: row.orderNumber,
							listStatus: [row.status]
						},
						sizeOrderItem: {
							length: row?.length,
							width: row?.width,
							size: row?.size
						}

					}
				})
				if (this.rows?.length) {
					this.setColumn()
					this.genListAction(this.rows);
				}
			});
	}

	getSku() {
		this.isLoading = true;
		this.skuService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.ListSku = res?.data.items.sort((a, b) => {
					return (a.isBigSize === b.isBigSize) ? 0 : a.isBigSize ? 1 : -1
				})
			})
	}

	getBrand() {
		this.isLoading = true;
		this.brandService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.listBrand = res?.data.items
				this.genItemFilters();
			})
	}

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.INPUT_TEXT,
				label: 'Số đơn',
				styleClass: 'w-25rem',
				placeholder: 'Tìm theo số đơn',
				variableReference: 'numberOrderItem',
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				label: 'Order number',
				styleClass: 'w-25rem',
				placeholder: 'Tìm theo order name',
				variableReference: 'orderNumber'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				label: 'Namespace',
				styleClass: 'w-25rem',
				placeholder: 'Namespace',
				variableReference: 'nameSpace'
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Auto approved',
				styleClass: 'w-25rem',
				variableReference: 'autoApproved',
				optionConfig: {
					data: StatusOrderItem.autoApproved,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.MULTIPLE_SELECT,
				label: 'Request update',
				styleClass: 'w-25rem',
				variableReference: 'autoApproved',
				placeholder: 'Chọn status request update',
				optionConfig: {
					data: StatusOrderItem.requestUpdate,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Tags',
				placeholder: 'Chọn tags',
				variableReference: 'tags',
				optionConfig: {
					data: StatusOrderItem.autoApproved,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Cảnh báo ship',
				variableReference: 'shipWarn',
				optionConfig: {
					data: StatusOrderItem.warningShip,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Brand',
				styleClass: 'w-25rem',
				variableReference: 'idBrand',
				placeholder: 'Chọn brand name',
				optionConfig: {
					data: this.listBrand,
					label: 'name',
					value: 'id'
				}
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày tạo',
				styleClass: 'w-25rem',
				variableReference: 'dateCreate',
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày duyệt',
				styleClass: 'w-25rem',
				variableReference: 'dateApprove',
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày upload',
				styleClass: 'w-25rem',
				variableReference: 'dateUpload',
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày đánh dấu đã in',
				styleClass: 'w-25rem',
				variableReference: 'datePrinted',
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày đẩy vào lô',
				styleClass: 'w-25rem',
				variableReference: 'datePullBatch',
			},
		]
	}

	setColumn() {
		this.columns = [
			// { field: '', header: '', width: 3, type: ETableColumnType.CHECKBOX_ACTION, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{
				field: 'template', header: 'Template', width: 11, isFrozen: true, alignFrozen: ETableFrozen.LEFT, type: ETableColumnType.TEMPLATE, otherType: ETableColumnType.LINK,
				getTagInfo: (template) => StatusOrderItem.getListStatusInfo(template?.listStatus),
				action: (value) => (this.detail(value?.id)),
			},
			{
				field: 'mockUpFront', header: "Mock up", width: 10, type: ETableColumnType.IMAGE_SET, otherType: ETableColumnType.IS_MOCKUP_FRONT,
				isFrozen: true, alignFrozen: ETableFrozen.LEFT,

			},
			{
				field: 'mockUpBack', header: "", width: 10, type: ETableColumnType.IMAGE_SET, otherType: ETableColumnType.IS_MOCKUP_BACK,
				isFrozen: true, alignFrozen: ETableFrozen.LEFT,
				customValue: (value) => {
					return value
				}
			},
			{
				field: 'design', header: "Design", width: 15, type: ETableColumnType.DESIGN,
			},
			{
				field: 'filePrint', header: "File In", width: 10, type: ETableColumnType.IMAGE_FILE_PRINT, isFrozen: true, alignFrozen: ETableFrozen.LEFT,

			},
			{
				field: 'code', header: 'Loại sản phẩm', width: 10
			},
			{
				field: 'sizeOrderItem', header: "Size", width: 10, type: ETableColumnType.SIZE_TEXT,
			},
			{
				field: 'status', header: "Trạng thái", width: 10, type: ETableColumnType.STATUS,
				getTagInfo: (status) => StatusOrderItem.getStatusInfo(status)
			},
			{
				field: 'errorMessage', header: "Lỗi", width: 3

			},
			{
				field: 'createdDate', header: "Ngày tạo", width: 15, type: EFormatDateDisplay.DATE_DMY,
				// customValue: (value) => this.formatTimeAgo(value)

			},
			{
				field: 'note', header: "Note", width: 10, type: ETableColumnType.NOTE_ITEM
			},
			// {
			// 	field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			// }
		]
	}

	genListAction(data: any) {
		this.listAction = data.map(item => {
			const actions = [];

			actions.push({
				label: 'Check',
				icon: 'pi pi-check',
				command: () => {

				}
			});

			actions.push({
				label: 'Chia sẻ',
				icon: 'pi pi-share-alt',
				command: () => {

				}
			});

			actions.push({
				label: 'Tạm dừng',
				icon: 'pi pi-pause',
				command: () => {

				}
			});
			return actions;
		});
	}

	changeSku(index: number) {
		if (index == -1) {
			this.currentSku = -1
			this.dataFilter = { ...this.dataFilter, idSku: null }
			this.setPage()
		}
		else {
			this.currentSku = index
			this.dataFilter = { ...this.dataFilter, idSku: this.currentSku }
			this.setPage()
		}
	}

	changeStatusOrderItem(index: number) {
		if (index == -1) {
			this.curentStatusOrderItem = -1
			this.dataFilter = { ...this.dataFilter, status: null }
			this.setPage()
		}
		else {
			this.curentStatusOrderItem = index
			this.dataFilter = { ...this.dataFilter, status: this.curentStatusOrderItem }
			this.setPage()
		}
	}

	padId(id: number, numberIndex: number): string {
		return `#${id.toString().padStart(6, '0')}_${numberIndex}`;
	}

	createBulkAction() {
		this.dialogService.open(CreateBatchComponent, {
			header: "Tạo lô",
			styleClass: `p-dialog-custom customModal`,
			width: '600px',
		}).onClose.subscribe(result => {
			if (result) {

				this.setPage();
			}
		})
	}

	detail(id: any) {
		this.router.navigate([`/order-management/item/detail/${this.cryptEncode(id)}`])
	}

}
