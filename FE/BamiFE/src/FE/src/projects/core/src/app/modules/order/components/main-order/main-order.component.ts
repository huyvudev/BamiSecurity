import { Component, Injector } from '@angular/core';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { Page } from '@mylib-shared/models/page';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { Router } from '@angular/router';
import { Order } from '../../models/order.models';
import { OrderService } from '../../services/order.service';
import { finalize } from 'rxjs';
import { StatusOrder } from '../../const/orderStatus.models';

@Component({
	selector: 'app-main-order',
	templateUrl: './main-order.component.html',
	styleUrls: ['./main-order.component.scss']
})
export class MainOrderComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private dialogService: DialogService,
		private orderService: OrderService,

		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý đơn" },
			{ label: "Đơn" }
		]);
	}

	columns: IColumn[] = []
	itemFilters: IItemFilter[] = [];
	listAction: IAction[][] = [];
	rows: Order[] = []
	dataFilter: any
	currentStatus: number = 0;

	statusOrder = StatusOrder.status

	ngOnInit() {
		this.dataFilter = { ...this.dataFilter, status: this.currentStatus }
		this.setColumn()
		this.genListAction(this.rows);
		this.genItemFilters();
		this.setPage();
	}

	public setPage(event?: any) {
		this.dataFilter = { ...this.dataFilter, ...event }
		this.isLoading = true;
		this.orderService.getAll(this.page, this.dataFilter)
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


		this.genListAction(this.rows)
	}

	padId(id: number): string {
		return `#${id.toString().padStart(6, '0')}`;
	}

	setColumn() {
		this.columns = [
			{
				field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT,
				otherType: ETableColumnType.LINK,
				action: (value) => (this.detail(value?.id)),
				customValue: (value) => this.padId(value),
			},
			{ field: 'name', header: 'Tên người nhận', width: 10, isFrozen: true, alignFrozen: ETableFrozen.LEFT, },
			{
				field: 'address', header: "Địa chỉ", width: 10, type: ETableColumnType.TEXT,
			},

			{
				field: 'orderNumber', header: "Order number", width: 10,

			},
			{
				field: 'city', header: "Thành phố", width: 10,

			},
			{
				field: 'state', header: "Bang", width: 10,

			},

			{
				field: 'postalCode', header: "Mã bưu chính", width: 10,

			},
			{
				field: 'status', header: "Trạng thái", width: 10, type: ETableColumnType.STATUS,
				getTagInfo: (status) => StatusOrder.getStatusInfo(status)
			},
			{
				field: ' country', header: "Quốc gia", width: 10,

			},
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}
		]
	}

	genListAction(data?: any) {
		this.listAction = data.map(item => {
			const actions = [];

			actions.push({
				label: 'Thông tin chi tiết',
				icon: 'pi pi-exclamation-circle',
				command: () => {
					this.detail(item?.id)
				}
			});
			if (item?.status == 0) {
				actions.push({
					label: 'Đẩy đơn hàng',
					icon: 'pi pi-copy',
					command: () => {
						this.approveOrder(item?.id)
					}
				});
			}

			actions.push({
				label: 'Xóa đơn hàng',
				icon: 'pi pi-trash',
				command: () => {
					this.deleteOrder(item?.id)
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
				label: 'Tên người nhận'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'orderNumber',
				placeholder: 'Tìm kiếm',
				label: 'Order Number'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'address',
				placeholder: 'Tìm kiếm',
				label: 'Địa chỉ'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'namespace',
				placeholder: 'Tìm kiếm',
				label: 'Namesapce'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'state',
				placeholder: 'Tìm kiếm',
				label: 'State'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'postalCode',
				placeholder: 'Tìm kiếm',
				label: 'Mã bưu điện'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'city',
				placeholder: 'Tìm kiếm',
				label: 'Thành phố'
			},
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'country',
				placeholder: 'Tìm kiếm',
				label: 'Quốc gia'
			},
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


	createOrder() {
		this.router.navigate(['/order-management/order/create'])
	}

	detail(id: any) {
		this.router.navigate([`/order-management/order/detail/${this.cryptEncode(id)}`])
	}

	async deleteOrder(idOrder: number) {
		const acceptComfirm = await this.confirmAction("Bạn có chắc muốn xóa Order?")
		if (acceptComfirm) {
			this.orderService.delete(idOrder)
				.pipe(finalize(() => this.isLoading = false))
				.subscribe((response) => {
					if (this.checkStatusResponse(response, "Xóa Order Thành công")) {
						this.setPage();
					}
				})
		}
	}

	async approveOrder(idOrder: number) {
		const acceptComfirm = await this.confirmAction("Bạn có chắc muốn duyệt Order?")
		if (acceptComfirm) {
			this.isLoading = true;
			this.orderService.approve(idOrder)
				.pipe(finalize(() => this.isLoading = false))
				.subscribe((response) => {
					if (this.checkStatusResponse(response, "Duyệt Order Thành công")) {
						this.setPage();
					}
				})
		}
	}

}
