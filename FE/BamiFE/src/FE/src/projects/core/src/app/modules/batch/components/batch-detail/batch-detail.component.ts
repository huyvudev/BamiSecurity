import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { Page } from '@mylib-shared/models/page';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { BatchService } from '../../services/batch.service';
import { finalize } from 'rxjs';
import { BatchDetail } from '../../models/batch.model';
import { OrderItem } from '../../../order-item/models/order-item.model';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { StatusOrderItem } from '../../../order-item/const/status-item.const';
import { EFormatDateDisplay } from '@mylib-shared/consts/base.consts';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { StatusBatch } from '../../const/batch.const';

@Component({
	selector: 'app-batch-detail',
	templateUrl: './batch-detail.component.html',
	styleUrls: ['./batch-detail.component.scss']
})
export class BatchDetailComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private fb: FormBuilder,
		private dialogService: DialogService,
		private routeActive: ActivatedRoute,
		private breadcrumbService: BreadcrumbService,
		private batchService: BatchService,
	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Lô", routerLink: ['/batch'] },
			{ label: "Thông tin chi tiết lô" }
		]);
	}

	postForm: FormGroup;
	columns: IColumn[] = [];
	listAction: IAction[][] = [];
	pageCustom: Page = new Page()
	idBatch: any;
	batch: BatchDetail;
	orderItem: OrderItem[];
	itemFilters: IItemFilter[] = [];

	hasData: boolean = true
	priorityList = StatusBatch.Prioritize

	ngOnInit() {
		this.pageCustom.pageSize = -1
		this.pageCustom.pageNumber = 1

		if (this.routeActive.snapshot.paramMap.get('id')) {
			this.idBatch = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'))
			if (this.idBatch) {
				this.getDetail(this.idBatch);
			}
		}
		this.setForm();
		this.setColumn();
		this.genItemFilters();
	}

	getDetail(id?: any) {
		this.isLoading = true;
		this.hasData = true;
		this.batchService.getById(id)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((response) => {
				if (this.checkStatusResponse(response)) {
					this.batch = response.data
					this.orderItem = this.batch.orderItems.map(row => {
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
					console.log(this.batch)
					console.log(this.orderItem)
					this.setColumn();
					this.setForm();
				}
				else {
					this.hasData = false;
				}
			})
	}

	setForm() {
		this.postForm = this.fb.group({
			creatorName: [this.batch?.creatorName || ''],
			sku: [this.batch?.sku || ''],
			priority: [this.batch?.priority || '',],
			partnerName: [this.batch?.partnerName || '',],
			printDate: [this.batch?.printDate || '',],
			createdDate
				: [this.batch?.createdDate || ''],
			cutDate: [this.batch?.printDate || '',],
			engravedDate: [this.batch?.printDate || '',],
			finishDate: [this.batch?.printDate || '',],
		});
		this.postForm.disable();
	}


	padId(id: number, numberIndex: number): string {
		return `#${id.toString().padStart(6, '0')}_${numberIndex}`;
	}

	setColumn() {
		this.columns = [
			// { field: '', header: '', width: 3, type: ETableColumnType.CHECKBOX_ACTION, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{
				field: 'template', header: 'Template', width: 11, isFrozen: true, alignFrozen: ETableFrozen.LEFT, type: ETableColumnType.TEMPLATE,
				otherType: ETableColumnType.LINK,
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
		]
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

	detail(id: any) {
		this.router.navigate([`/order-management/item/detail/${this.cryptEncode(id)}`])
	}
}

