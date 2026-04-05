import { MaterialService } from './../../sku-management/services/material.service';
import { BrandService } from './../../name-space/services/brand.service';
import { Component, Injector } from '@angular/core';
import { EFormatDateDisplay } from '@mylib-shared/consts/base.consts';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { StatusBatch } from '../const/batch.const';
import { SkuService } from '../../sku-management/services/sku.service';
import { Page } from '@mylib-shared/models/page';
import { finalize } from 'rxjs';
import { Sku } from '../../sku-management/model/sku.model';
import { Batch } from '../models/batch.model';
import { BatchService } from '../services/batch.service';
import { Partner } from '../../partner/model/partner.model';
import { PartnerService } from '../../partner/services/partner.service';
import { Brand } from '../../name-space/models/brand.model';
import { Material } from '../../sku-management/model/material.model';
import { Router } from '@angular/router';

interface AdditionalData {
	createDateStart?: string;
	createDateEnd?: string;
	printDateStart?: string;
	printDateEnd?: string;
	cutDateStart?: string;
	cutDateEnd?: string;
	engravedDateStart?: string;
	engravedDateEnd?: string;
	finishDateStart?: string;
	finishDateEnd?: string;
}

@Component({
	selector: 'app-main-batch',
	templateUrl: './main-batch.component.html',
	styleUrls: ['./main-batch.component.scss']
})
export class MainBatchComponent extends ComponentBase {

	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private skuService: SkuService,
		private batchService: BatchService,
		private parterService: PartnerService,
		private router: Router,
		private brandService: BrandService,
		private materialService: MaterialService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Lô" },
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: Batch[] = [];
	dataFilter: any
	statusBatch = StatusBatch.status
	prioritize = StatusBatch.Prioritize
	timePrint = StatusBatch.TimePrint
	timeCut = StatusBatch.TimeCut
	statusMerge = StatusBatch.StatusMerge
	shipStatus = StatusBatch.ShipStatus

	skuList: Sku[] = [];
	parterList: Partner[] = [];
	brandList: Brand[] = [];
	materialList: Material[] = []

	pageCustom: Page = new Page()

	ngOnInit() {
		this.pageCustom.pageSize = -1
		this.pageCustom.pageNumber = 1

		this.setPage();
		this.genItemFilters();
		this.getPartner();
		this.getSku();
		this.getBrand();
		this.getMaterial();

	}

	setPage(event?: any) {

		const { createDate, printDate, cutDate, engraveDate, finishDate, ...data } = event || {};

		console.log(event)
		if (Array.isArray(createDate) && createDate.length > 0) {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				createDateStart: createDate[0],
				createDateEnd: createDate[1],
			};
		} else {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				createDateStart: null,
				createDateEnd: null,
			};
		}

		if (Array.isArray(printDate) && printDate.length > 0) {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				printDateStart: printDate[0],
				printDateEnd: printDate[1],
			};
		} else {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				printDateStart: null,
				printDateEnd: null,
			};
		}

		if (Array.isArray(cutDate) && cutDate.length > 0) {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				cutDateStart: cutDate[0],
				cutDateEnd: cutDate[1],
			};
		} else {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				cutDateStart: null,
				cutDateEnd: null,
			};
		}

		if (Array.isArray(engraveDate) && engraveDate.length > 0) {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				engravedDateStart: engraveDate[0],
				engravedDateEnd: engraveDate[1],
			};
		} else {
			this.dataFilter = {
				...this.dataFilter,
				...data,
				engravedDateStart: null,
				engravedDateEnd: null,
			};
		}

		if (Array.isArray(finishDate) && finishDate.length > 0) {
			this.dataFilter = {
				...this.dataFilter,
				finishDateStart: finishDate[0],
				finishDateEnd: finishDate[1],
			};
		} else {
			this.dataFilter = {
				...this.dataFilter,
				...data,
			};
		}

		console.log(this.dataFilter)
		this.isLoading = true;
		this.batchService.getAll(this.page, this.dataFilter)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.rows = this.page.getRowLoadMore(this.rows, res?.data?.items);
				this.page.totalItems = res?.data?.totalItems;
				this.rows = this.rows.map(row => {
					return {
						...row, nameBatch: {
							code: this.padBatch(row?.id),
							listStatus: [row.priority]
						}
					}
				})
				console.log(this.rows)
				if (this.rows?.length) {
					this.setColumn()
				}
			})
	}

	getSku() {
		this.isLoading = true;
		this.skuService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.skuList = res?.data.items
				this.genItemFilters();
			})
	}

	getPartner() {
		this.isLoading = true;
		this.parterService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.parterList = res?.data.items
				this.genItemFilters();
			})
	}

	getBrand() {
		this.isLoading = true;
		this.brandService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.brandList = res?.data.items
				this.genItemFilters();
			})
	}

	getMaterial() {
		this.isLoading = true;
		this.materialService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.materialList = res?.data.items
				this.genItemFilters();
			})
	}

	setColumn() {
		this.columns = [
			{
				field: 'nameBatch', header: 'Tên lô', width: 6, isFrozen: true, alignFrozen: ETableFrozen.LEFT, type: ETableColumnType.TEMPLATE, otherType: ETableColumnType.LINK,
				getTagInfo: (nameBatch) => StatusBatch.getPrioritizeStatusInfo(nameBatch?.listStatus),
				action: (value) => (this.detail(value?.id)),
			},
			{
				field: 'createdDate', header: "Ngày tạo", width: 2, type: EFormatDateDisplay.DATE_DMY,


			},
			{
				field: 'printDate', header: "Ngày in", width: 3, type: EFormatDateDisplay.DATE_DMY,


			},
			{
				field: 'cutDate', header: "Ngày cắt", width: 3, type: EFormatDateDisplay.DATE_DMY,


			},
			{
				field: 'engravedDate', header: "Ngày khắc", width: 3, type: EFormatDateDisplay.DATE_DMY,


			},
			{
				field: 'finishDate', header: "Ngày hoàn thiện", width: 3, type: EFormatDateDisplay.DATE_DMY,


			},
			{
				field: 'sku', header: "Loại sản phẩm", width: 5,
			},
			{
				field: '', header: "Đang chờ / Đã move / Hoàn thành / Tổng", width: 15

			},
			{
				field: 'status', header: "Trạng thái", width: 5,
				type: ETableColumnType.STATUS,
				getTagInfo: (status) => StatusBatch.getStatusInfo(status)


			},
			{
				field: 'partnerName', header: 'Đối tác in ép', width: 5
			},
			{
				field: 'creatorName', header: "Người tạo", width: 5

			},
			// {
			// 	field: 'creatorName', header: "Xử lý", width: 5

			// },
			{
				field: 'note', header: 'Ghi chú', width: 5, type: ETableColumnType.NOTE_BATCH

			}
		]
	}

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.SELECT,
				variableReference: 'status',
				label: 'Trạng thái lô',
				styleClass: 'w-25rem',
				optionConfig: {
					data: this.statusBatch,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Độ ưu tiên',
				variableReference: 'priority',
				styleClass: 'w-25rem',
				optionConfig: {
					data: this.prioritize,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				label: 'Tên lô',
				styleClass: 'w-25rem',
				variableReference: 'name',
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				label: 'Người tạo lô',
				styleClass: 'w-25rem',
				variableReference: 'creatorName',
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				label: 'Note',
				styleClass: 'w-25rem',
				variableReference: 'note',
			},
			{
				type: ETableTopBar.MULTIPLE_SELECT,
				label: 'Loại sản phẩm',
				styleClass: 'w-25rem',
				variableReference: 'skuId',
				optionConfig: {
					data: this.skuList,
					label: 'code',
					value: 'id'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Đối tác in ép',
				styleClass: 'w-25rem',
				variableReference: 'partnerId',
				optionConfig: {
					data: this.parterList,
					label: 'name',
					value: 'id'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Thời gian quá hạn in',
				styleClass: 'w-25rem',
				variableReference: 'timePrint',
				optionConfig: {
					data: this.timePrint,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Thời gian quá hạn cắt',
				styleClass: 'w-25rem',
				variableReference: 'timeCut',
				optionConfig: {
					data: this.timeCut,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Thời gian quá hạn khắc',
				styleClass: 'w-25rem',
				variableReference: 'timeEngraving',
				optionConfig: {
					data: this.timeCut,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Thời gian quá hạn hoàn thiện',
				styleClass: 'w-25rem',
				variableReference: 'timeComplete',
				optionConfig: {
					data: this.timeCut,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Trạng thái gộp',
				styleClass: 'w-25rem',
				variableReference: 'statusMerger',
				optionConfig: {
					data: this.statusMerge,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Tag',
				styleClass: 'w-25rem',
				variableReference: 'tagId',
				optionConfig: {
					data: this.statusMerge,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Brand',
				styleClass: 'w-25rem',
				variableReference: 'brandId',
				optionConfig: {
					data: this.brandList,
					label: 'name',
					value: 'id'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Nguyên liệu',
				styleClass: 'w-25rem',
				variableReference: 'materialId',
				optionConfig: {
					data: this.materialList,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Đã giao',
				styleClass: 'w-25rem',
				variableReference: 'shipStatus',
				optionConfig: {
					data: this.shipStatus,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày tạo lô',
				styleClass: 'w-25rem',
				variableReference: 'createDate',
				placeholder: 'Start date - End date',
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày in',
				styleClass: 'w-25rem',
				variableReference: 'printDate',
				placeholder: 'Start date - End date',
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày cắt',
				styleClass: 'w-25rem',
				variableReference: 'cutDate',
				placeholder: 'Start date - End date',
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày khắc',
				styleClass: 'w-25rem',
				variableReference: 'engraveDate',
				placeholder: 'Start date - End date',
			},
			{
				type: ETableTopBar.DATE_TO_DATE,
				label: 'Ngày hoàn thiện',
				styleClass: 'w-25rem',
				variableReference: 'finishDate',
				placeholder: 'Start date - End date',
			},

		]
	}

	padBatch(id: number): string {
		return `B-${id.toString()}`;
	}

	QCError() {

	}
	
	detail(id: any) {
		this.router.navigate([`/batch-management/batch/detail/${this.cryptEncode(id)}`])
	}
}
