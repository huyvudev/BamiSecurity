import { Component, Injector } from '@angular/core';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { Page } from '@mylib-shared/models/page';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { LabelConstant } from '../const/shipping-carrier.const';
import { EFormatDateDisplay } from '@mylib-shared/consts/base.consts';

@Component({
	selector: 'app-main-labels',
	templateUrl: './main-labels.component.html',
	styleUrls: ['./main-labels.component.scss']
})

export class MainLabelsComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Labels" },
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	shippingCarrierStatus = LabelConstant.ShippingCarrierStatus;
	labelStauts = LabelConstant.statusLabels;
	rows: any[] = [];
	dataFilter: any
	ngOnInit() {
		this.setColumn()
		this.genListAction(this.rows);
		this.genItemFilters();
		this.setPage();
	}

	public setPage(event?: Page) {
		this.rows = [{
			orderId: '436325',
			label: 'https://i0.wp.com/news.qoo-app.com/en/wp-content/uploads/sites/3/2022/05/rent-a-girlfriend-season-2-chizuru.jpg?resize=707%2C1000&ssl=1',
			trackingCarrier: 2,
			trackingNumber: 1,
			createdDate: "2024-12-02T10:48:46.120Z",
			note: 'Hiệp đẹp trai',
			labelType: 'International',
		}]
		this.genListAction(this.rows)
	}

	setColumn() {
		this.columns = [
			{ field: 'orderId', header: 'Mã đơn', width: 15, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },

			{
				field: 'label', header: "Nhãn", width: 15, type: ETableColumnType.LABEL,
			},
			{
				field: 'trackingCarrier', header: "Tracking carrier", width: 15, customValue: (value) => {
					return LabelConstant.getNameByCode(value)
				}

			},
			{
				field: 'trackingNumber', header: 'Tracking number', width: 10, type: ETableColumnType.TRACKING_NUMBER
			},
			{
				field: 'createdDate', header: "Ngày tạo", width: 15, type: EFormatDateDisplay.DATE_DMY,
				customValue: (value) => this.formatTimeAgo(value)

			},
			{
				field: 'note', header: "Note", width: 15, type: ETableColumnType.NOTE_BATCH,

			},
			{
				field: 'labelType', header: "Loại", width: 15,

			},
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}
		]
	}

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.SELECT,
				label: 'Shipping carrier',
				variableReference: 'shippingCarrier',
				optionConfig: {
					data: this.shippingCarrierStatus,
					label: 'name',
					value: 'code'
				}
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Trạng thái',
				variableReference: 'status',
				optionConfig: {
					data: this.labelStauts,
					label: 'name',
					value: 'code'
				}

			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'keyword',
				placeholder: 'Tìm kiếm theo barcode',
				label: 'Barcode'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'keyword',
				placeholder: 'Tìm kiếm theo tracking number',
				label: 'Tracking number'
			},
		]
	}

	genListAction(data: any) {
		this.listAction = data.map(item => {
			const actions = [];

			actions.push({
				label: 'Xóa nhãn',
				icon: 'pi pi-trash',
				command: () => {

				}
			});

			return actions;
		});
	}



	listJob() {

	}

	importPDF() {

	}
}
