import { Component, Injector } from '@angular/core';
import { EFormatDateDisplay } from '@mylib-shared/consts/base.consts';
import { IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { Page } from '@mylib-shared/models/page';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { BasketsConst } from '../const/baskest-status.const';

@Component({
	selector: 'app-main-baskets',
	templateUrl: './main-baskets.component.html',
	styleUrls: ['./main-baskets.component.scss']
})
export class MainBasketsComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Baskets" },
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: any[] = [];
	dataFilter: any

	currentStautsAddress: number = 0;
	currentStautsBaskets: number = 0;

	statusAddress = BasketsConst.Address;
	statusBaskets = BasketsConst.BasketsStatus;
	typeOrder = BasketsConst.typeOrder;

	ngOnInit() {
		this.setColumn()
		this.setPage();
	}

	setPage(event?: Page) {
		this.rows = [{
			numberBasket: 19,
			codeOrder: '#100904',
			typeOrderItem: 'NAME_NIGHT_P1_0115MT',// như SKU	
			typeOrder: '1',
			completed: 1,
			address: 1,
			status: 1,
			createdDate: '2024-11-03T04:26:39.985Z',
			completedDate: '2024-12-03T04:26:39.985Z',
		}]
	}

	setColumn() {
		this.columns = [
			{ field: 'numberBasket', header: 'Số giỏ', width: 10, isFrozen: true, alignFrozen: ETableFrozen.LEFT },

			{
				field: 'codeOrder', header: "Mã đơn", width: 10,
			},
			{
				field: 'typeOrderItem', header: "Loại sản phẩm", width: 10

			},
			{
				field: 'typeOrder', header: 'Loại đơn', width: 10
			},
			{
				field: 'completed', header: "Đã hoàn thành", width: 10

			},
			{
				field: 'address', header: "Địa chỉ", width: 10

			},
			{
				field: 'status', header: "Trạng thái", width: 10, 
				type: ETableColumnType.STATUS, 
				getTagInfo: (status) => BasketsConst.getStatusInfo(status)

			},
			{
				field: 'createdDate', header: "Ngày tạo", width: 10, type: EFormatDateDisplay.DATE_DMY,
				customValue :(value) =>  this.formatTimeAgo(value)

			},
			{
				field: 'completedDate', header: 'Ngày hoàn thành', width: 10, type: EFormatDateDisplay.DATE_DMY,
				customValue :(value) =>  this.formatTimeAgo(value)

			}
		]
	}

	changeStatusAddress(index: number) {
		this.currentStautsAddress = index
	}

	changeStatusBasket(index: number) {
		this.currentStautsBaskets = index
	}

	addBasket() {

	}
	deleteBasket() {

	}
	unCompletedBasket() {

	}

	
	
	
}
