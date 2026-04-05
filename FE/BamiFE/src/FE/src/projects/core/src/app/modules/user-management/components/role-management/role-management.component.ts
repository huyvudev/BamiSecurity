import { Component, OnInit } from '@angular/core';
import { PageBase } from '@shared/core/component-bases.ts/page-base';
import { RoleService } from '../../services/role.service';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { finalize } from 'rxjs/operators';
import { RoleFormComponent } from './role-form/role-form.component';
import { Page } from '@mylib-shared/models/page';
import { IRoleItemList } from '../../models/role.model';
import { ActiveDeactiveConst, EActiveDeactive } from '@mylib-shared/consts/base.consts';
import { IPermissionConfig } from '@mylib-shared/models/base.model';
import { IPermissionTreeItem } from '../../models/base.models';
import { ButtonActiveDeactiveRole, ButtonAddRole, ButtonDeleteRole, ButtonUpdateRole } from '@shared/consts/permissionWeb/permission-key.const';

@Component({
	selector: 'app-role-management',
	templateUrl: './role-management.component.html',
	styleUrls: ['./role-management.component.scss']
})
//
export class RoleManagementComponent extends PageBase implements OnInit {

	constructor(
		private _selfService: RoleService,
		private breadcrumbService: BreadcrumbService,
	) {
		super()
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý người dùng"},
			{ label: "Vai trò" }
		]);
	}

	rows: IRoleItemList[] = [];
	columns: IColumn[] = [];
	itemFilters: IItemFilter[] = [];

	listAction: IAction[][] = [];

	page = new Page();
	ButtonAddRole = ButtonAddRole;

	ngOnInit() {
		//
		this.genItemFilters();
		this.setPage();
		this.getPermisionKeys();
		//
		this.columns = [
			this.getColumnId(),
			{ field: 'name', header: 'Tên vai trò', minWidth: 12, isPin: true, isResize: true },
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

	dataFilter: any;
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
	genListAction(data: IRoleItemList[] = []) {
		this.listAction = data.map(item => {
			const actions = [];

			if (this.isGranted(ButtonUpdateRole)) {
				actions.push({
					label: 'Cập nhật vai trò',
					icon: 'pi pi-pencil',
					command: () => {
						this.createOrUpdate(item.id)
					}
				});
			}
			//
			if (this.isGranted(ButtonActiveDeactiveRole)) {
				actions.push({
					label: item.status === EActiveDeactive.ACTIVE ? 'Khóa vai trò' : 'Kích hoạt',
					icon: item.status === EActiveDeactive.ACTIVE ? 'pi pi-lock' : 'pi pi-check-circle',
					command: () => {
						this.changeStatus(item)
					}
				});
			}
			if (this.isGranted(ButtonDeleteRole) && item.status === EActiveDeactive.DEACTIVE) {
				actions.push({
					label: 'Xóa vài trò',
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
			RoleFormComponent,
			{
				header: id ? 'Cập nhật vai trò' : 'Thêm vai trò',
				width: '500px',
				styleClass: 'dialog-full-height',
				position: 'right',
				data: {
					id: id,
					permisisonKeys: this.permisisonKeys,
					permissionsTree: this.permissionsTree
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

	async changeStatus(item: IRoleItemList) {
		const messageConfirm = item.status === EActiveDeactive.ACTIVE ? 'Bạn chắc chắn Khóa vai trò?' : 'Bạn chắc chắn Kích hoạt vai trò?';
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
		const acceptAction = await this.confirmAction('Bạn chắc chắn xóa vai trò?');
		if(acceptAction) {
			this._selfService.delete(id).subscribe((res) => {
				if(this.checkStatusResponse(res, 'Xóa thành công!')) {
					this.setPage();
				} 
			})
		} 
	}

	// Chạy ngầm xử lý cây quyền trước để tối ưu trải nghiệm khi mở cập nhật vai trò
	permisisonKeys: IPermissionConfig[] = [];
	permissionsTree: IPermissionTreeItem[] = [];
	getPermisionKeys() {
		this._permissionService.getAllPermisionKey().subscribe((res) => {
			if(this.checkStatusResponse(res)) {
				this.permisisonKeys = res?.data;
				this.handleDataPermision();
			}
		})
	}

	handleDataPermision() {
		this.permissionsTree = this.permisisonKeys.map((item) => {
			return {
				...item,
				children: [],
				styleClass: ''
			}
		});
		//
		for( let i=1; i <= 8; i++) {
		  this.permissionsTree = this.permissionsTree.map((item) => {
			item.children = this.permissionsTree.filter(c => c.parentKey == item.key);
			item.styleClass = ((item.children?.length || !item.parentKey)  ? 'permission-parents ' : '') + (item.parentKey == null ? 'node-0 ' : '');
			return item;
		  });
		}
		//
		this.permissionsTree = this.permissionsTree.filter(item => item.parentKey == null);
	}
}
