import { OrderDetailService } from './../../services/order-detail.service';
import { Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { Order, OrderDetail } from '../../models/order.models';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { Page } from '@mylib-shared/models/page';
import { UpdateOrderDetailComponent } from './update-order-detail/update-order-detail.component';
import { finalize } from 'rxjs';
import { StatusOrderDetail } from '../../const/orderDetailStatus.const';

@Component({
	selector: 'app-main-order-detail',
	templateUrl: './main-order-detail.component.html',
	styleUrls: ['./main-order-detail.component.scss']
})
export class MainOrderDetailComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private dialogService: DialogService,
		private orderDetailService: OrderDetailService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý đơn hàng" },
			{ label: "Chi tiết đơn hàng" }
		]);
	}
	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: OrderDetail[] = []
	dataFilter: any
	currentStatus: number = 3;
	statusOrderDetail = StatusOrderDetail.status

	ngOnInit() {
		this.dataFilter = { ...this.dataFilter, status: this.currentStatus }
		this.setColumn()
		this.genListAction(this.rows);
		this.genItemFilters();
		this.setPage();
	}

	setPage(event?: Page) {
		this.dataFilter = { ...this.dataFilter, ...event }
		this.isLoading = true;
		this.orderDetailService.getAllOrderDetailDone(this.page, this.dataFilter)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((response) => {
				if (this.checkStatusResponse(response)) {
					this.rows = this.page.getRowLoadMore(this.rows, response?.data?.items);
					this.page.totalItems = response?.data?.totalItems;
					if (this.rows?.length) {
						this.genListAction(this.rows);
					}
				}
			})

	}

	setColumn() {
		this.columns = [
			{
				field: 'orderId', header: '#IDOrder', width: 5, isFrozen: true,
				alignFrozen: ETableFrozen.LEFT,
				otherType: ETableColumnType.LINK,
				action: (value) => (this.detail(value?.orderId)),
				customValue: (value) => this.padId(value)
			},
			{
				field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT, otherType: ETableColumnType.LINK,
				customValue: (value) => this.padId(value),
				action: (value) => (this.updateOrderDetail(value))
			},
			{ field: 'type', header: 'Type', width: 10, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{
				field: 'title', header: "Title", width: 10, type: ETableColumnType.TEXT,
			},

			{
				field: 'size', header: "Size", width: 5,

			},
			{
				field: 'sellerSku', header: "SellerSku", width: 10,

			},
			{
				field: 'quantity', header: "Số lượng", width: 6,

			},
			{
				field: 'status', header: "Trạng thái", width: 10, type: ETableColumnType.STATUS,
				getTagInfo: (status) => StatusOrderDetail.getStatusInfo(status)
			},
			{
				field: 'width', header: "Width", width: 5,

			},
			{
				field: 'Length', header: "Length", width: 5,

			},
			{
				field: 'mockUpFront', header: "Mock Up", width: 10, type: ETableColumnType.IMAGE_SET, otherType: ETableColumnType.IS_MOCKUP_FRONT,
				isFrozen: true, alignFrozen: ETableFrozen.LEFT,
			},
			{
				field: 'mockUpBack', header: "", width: 10, type: ETableColumnType.IMAGE_SET, otherType: ETableColumnType.IS_MOCKUP_BACK,
				isFrozen: true, alignFrozen: ETableFrozen.LEFT,

			},

			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,
			}
		]
	}

	changeStatus(index?: number) {
		if (index == -1) {
			this.currentStatus = -1
			this.dataFilter = { ...this.dataFilter, status: null }
			this.setPage()
		}
		else {
			this.currentStatus = index
			this.dataFilter = { ...this.dataFilter, status: this.currentStatus }
			this.setPage()
		}

	}

	genListAction(data?: any) {
		this.listAction = data.map(item => {
			const actions = [];
			if (item?.status != 5)
				actions.push({
					label: 'Chỉnh sửa thông tin',
					icon: 'pi pi-exclamation-circle',
					command: () => {
						this.updateOrderDetail(item)
					}
				});

			if (item?.status == 3) {
				actions.push({
					label: 'Đẩy sinh ra item',
					icon: 'pi pi-exclamation-circle',
					command: () => {
						this.approvedDetail(item?.id)
					}
				});
			}



			return actions;
		});
	}

	detail(id: any) {
		this.router.navigate([`/order-management/order/detail/${this.cryptEncode(id)}`])
	}

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'keyword',
				placeholder: 'Tìm kiếm',
				label: 'Type chi tiết đơn hàng'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'orderId',
				placeholder: 'Tìm kiếm',
				label: 'OrderId'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'title',
				placeholder: 'Tìm kiếm',
				label: 'Title'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'size',
				placeholder: 'Tìm kiếm',
				label: 'Size'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'sellerSku',
				placeholder: 'Tìm kiếm',
				label: 'SellerSku'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'color',
				placeholder: 'Tìm kiếm',
				label: 'Color'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'quantity',
				placeholder: 'Tìm kiếm',
				label: 'Quantity'
			},

		]
	}

	updateOrderDetail(item?: OrderDetail) {
		this.router.navigate([`/order-management/detail-order/detail/${this.cryptEncode(item?.id)}`])
	}

	padId(id: number): string {
		return `#${id.toString().padStart(6, '0')}`;
	}

	async approvedDetail(id: number) {
		const acceptComfirm = await this.confirmAction("Bạn có chắc muốn duyệt Order?")
		if (acceptComfirm) {
			this.isLoading = true;
			this.orderDetailService.approved(id)
				.pipe(finalize(() => this.isLoading = false))
				.subscribe((response) => {
					if (this.checkStatusResponse(response, "Duyệt detail Thành công")) {
						this.setPage();
					}
				})
		}
	}
}
