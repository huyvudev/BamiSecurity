import { Component } from '@angular/core';
import { PageBase } from '@shared/core/component-bases.ts/page-base';
import { AccountService } from '../../services/account.service';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { IAccountItemList } from '../../models/account.model';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { Page } from '@mylib-shared/models/page';
import { ActiveDeactiveConst, EActiveDeactive, SexConst } from '@mylib-shared/consts/base.consts';
import { finalize } from 'rxjs/operators';
import { AccountFormComponent } from './account-form/account-form.component';
import { SetPasswordComponent } from './set-password/set-password.component';
import { ButtonActiveDeactiveAccount, ButtonAddAccount, ButtonDeleteAccount, ButtonSetPasswordAccount, ButtonUpdateAccount } from '@shared/consts/permissionWeb/permission-key.const';

@Component({
	selector: 'app-account-management',
	templateUrl: './account-management.component.html',
	styleUrls: ['./account-management.component.scss']
})
export class AccountManagementComponent extends PageBase {

	constructor(
		private _selfService: AccountService,
		private breadcrumbService: BreadcrumbService,
	) {
		super()
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý người dùng"},
			{ label: "Tài khoản" }
		]);
	}

	rows: IAccountItemList[] = [];
	columns: IColumn[] = [];
	itemFilters: IItemFilter[] = [];

	listAction: IAction[][] = [];

	page = new Page();

	ButtonAddAccount = ButtonAddAccount;

	ngOnInit() {
		//
		this.genItemFilters();
		this.setPage();
		this.setColumns();
	}

	setColumns() {
		this.columns = [
			this.getColumnId(),
			{ field: 'fullName', header: 'Họ và tên', minWidth: 14, isPin: true },
			{ field: 'email', header: 'Email', minWidth: 12, isPin: true },
			{ field: 'phone', header: 'Số điện thoại', width: 12, isPin: true },
			{ 
				field: 'gender', header: 'Giới tính', minWidth: 12, isPin: true,
				customValue: (status) => SexConst.getValue(status) 
			},
			this.getColumnStatus({
				getTagInfo: (status) => ActiveDeactiveConst.getInfo(status)
			}),
			this.getColumnAction()
		];
	}

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'keyword',
				placeholder: 'Tìm kiếm',
			},
			{
				type: ETableTopBar.SELECT,
				label: 'Trạng thái',
				variableReference: 'status',
				optionConfig: {
					data: ActiveDeactiveConst.list,
					label: 'name',
					value: 'code'
				}
			}
		]
	}

	dataFilter: any = {

	};
	setPage(event?: any) {
		if(event) this.dataFilter = event;
		//
		this.isLoading = true;
		this._selfService.getAll(this.page, this.dataFilter, this.destroyRef)
		.pipe(finalize(() => this.isLoading = false))
		.subscribe((res) => {
			if (this.checkStatusResponse(res)) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res?.data?.items;
				if (this.rows?.length) {
					this.genListAction(this.rows);
				}
			}
		});
	}

	/* ACTION */
	genListAction(data: IAccountItemList[] = []) {
		this.listAction = data.map(item => {
			const actions = [];

			if (this.isGranted(ButtonUpdateAccount)) {
				actions.push({
					label: 'Cập nhật tài khoản',
					icon: 'pi pi-pencil',
					command: () => {
						this.createOrUpdate(item.id)
					}
				});
			}
			//
			if (this.isGranted(ButtonSetPasswordAccount)) {
				actions.push({
					label: 'Cập nhật mật khẩu',
					icon: 'pi pi-pencil',
					command: () => {
						this.setPassword(item.id)
					}
				});
			}
			//
			if (this.isGranted(ButtonActiveDeactiveAccount)) {
				actions.push({
					label: item.status === EActiveDeactive.ACTIVE ? 'Khóa tài khoản' : 'Kích hoạt',
					icon: item.status === EActiveDeactive.ACTIVE ? 'pi pi-lock' : 'pi pi-check-circle',
					command: () => {
						this.changeStatus(item)
					}
				});
			}
			//
			if (this.isGranted(ButtonDeleteAccount) && item.status === EActiveDeactive.DEACTIVE) {
				actions.push({
					label: 'Xóa tài khoản',
					icon: 'pi pi-trash',
					command: () => {
						this.delete(item.id)
					}
				});
			}
			//
			return actions;
		});
	}

	// Thêm phân phối đầu tư
	createOrUpdate(id?: number) {
		const ref = this.dialogService.open(
			AccountFormComponent,
			{
				header: id ? 'Cập nhật tài khoản' : 'Thêm tài khoản',
				width: '700px',
				data: {
					id: id,
				}
			}
		);
		//
		ref.onClose.subscribe((res: boolean) => {
			if(res) {
				this.setPage()
			}
		});
	}

	async changeStatus(item: IAccountItemList) {
		const messageConfirm = item.status === EActiveDeactive.ACTIVE ? 'Bạn chắc chắn Khóa tài khoản?' : 'Bạn chắc chắn Kích hoạt tài khoản?';
		const acceptAction = await this.confirmAction(messageConfirm);
		if(acceptAction) {
			this._selfService.changeStatus(item.id).subscribe((res) => {
				if(this.checkStatusResponse(res, 'Cập nhật thành công!')) {
					this.setPage();
				} 
			})
		} 
	}

	async delete(id: number) {
		const acceptAction = await this.confirmAction('Bạn chắc chắn xóa tài khoản?');
		if(acceptAction) {
			this._selfService.delete(id).subscribe((res) => {
				if(this.checkStatusResponse(res, 'Xóa thành công!')) {
					this.setPage();
				} 
			})
		} 
	}

	setPassword(id: number) {
		this.dialogService.open(SetPasswordComponent, {
			header: 'Cập nhật mật khẩu',
			width: '400px',
			data: {
				id: id
			}
		})
	}
}
