import { Component, Injector, OnInit } from '@angular/core';
import { AppMainComponent } from '../main/app.main.component';
import { Utils } from 'projects/my-lib/src/lib/shared/utils';
import { IMenu } from 'projects/my-lib/src/lib/shared/interfaces/menu.interface';
import { PermissionsService } from 'projects/my-lib/src/lib/shared/services/auth/permission.service';
import { MenuIcons } from '@shared/consts/icon-menu.const';
import { MenuAccount, MenuRole, MenuUser } from '@shared/consts/permissionWeb/permission-key.const';

@Component({
	selector: 'app-menu',
	templateUrl: './app.menu.component.html',
})
export class AppMenuComponent {
	model: IMenu[];

	constructor(
		public appMain: AppMainComponent,
		private _permissionService: PermissionsService
	) {
		this._permissionService.getPermissions();
	}

	// CONSTS
	MenuIcons = MenuIcons;

	menuCache: any;
	userInfo: {
		name: string,
		username: string,
		userId: number,
		userType: string
	}

	isGranted(permissionKey: any): boolean {
		return this._permissionService.isGrantedRoot(permissionKey);
	}

	ngOnInit() {
		this.userInfo = Utils.getLocalStorage('userInfo');
		this._permissionService.getPermissions$.subscribe(() => {
			this.setMenu();
		})
	}

	setMenu() {
		const menuModel: IMenu[] = [
			{ label: 'Tổng quan', icon: 'pi pi-home', routerLink: ['home'] },
			{ label: 'Quản lý file', icon: 'pi pi-dollar', routerLink: ['manager-file'] },
			// {
			// 	label: 'Quản lý đơn hàng', icon: 'pi pi-credit-card', routerLink: ['order-management'],
			// 	items: [
			// 		{ label: 'Đơn hàng', icon: '', routerLink: ['order'], isShow: true },
			// 		{ label: 'Chi tiết đơn hàng', icon: '', routerLink: ['detail-order'], isShow: true },
			// 	]
			// },
			{
				label: 'Quản lý người dùng', icon: 'pi pi-users', routerLink: ['user-management'],
				isShow: this.isGranted(MenuUser),
				items: [
					{ label: 'Tài khoản', icon: '', routerLink: ['accounts'], isShow: this.isGranted(MenuAccount) },
					{ label: 'Vai trò', icon: '', routerLink: ['roles'], isShow: this.isGranted(MenuRole) },
				]
			},
			// {
			// 	label: 'Quản lý đối tác', icon: 'pi pi-star', routerLink: ['partner-management'],
			// 	items: [
			// 		{ label: 'Đối tác', icon: '', routerLink: ['partner'], isShow: true },
			// 		{ label: 'Loại đối tác', icon: '', routerLink: ['type-partner'], isShow: true },
			// 	]
			// },
			// { label: 'Labels', icon: 'pi pi-bookmark', routerLink: ['labels'] },
			// { label: 'Baskets', icon: 'pi pi-cart-plus', routerLink: ['baskets'] },
			// {
			// 	label: 'Quản lý lô', icon: 'pi pi-table', routerLink: ['batch-management'],
			// 	items: [
			// 		{ label: 'Lô', icon: '', routerLink: ['batch'], isShow: true },
			// 		{ label: 'Tạo QR code cho lô', icon: '', routerLink: ['qrCreate'], isShow: true },
			// 	]
			// },
			// {
			// 	label: 'Quản lý Sku', icon: 'pi pi-paperclip', routerLink: ['sku-management'],
			// 	isShow: true,
			// 	items: [
			// 		{ label: 'Sku', icon: '', routerLink: ['sku'], isShow: true },
			// 		{ label: 'SkuBase', icon: '', routerLink: ['base-sku'], isShow: true },
			// 		{ label: 'Chất liệu', icon: '', routerLink: ['material'], isShow: true },
			// 		{ label: 'Phương thức sản xuất', icon: '', routerLink: ['product-method'], isShow: true },
			// 	]
			// },
			// {
			// 	label: 'Quản lý Namespace', icon: 'pi pi-tag', routerLink: ['name-space'],
			// 	isShow: true,
			// 	items: [
			// 		{ label: 'Brand', icon: '', routerLink: ['brand'], isShow: true },

			// 	]
			// },
			// {
			// 	label: 'Quản lý Trạng thái', icon: 'pi pi-compass', routerLink: ['status-management'],
			// 	isShow: true,
			// 	items: [
			// 		{ label: 'Đã đẩy', icon: '', routerLink: ['pushed'], isShow: true },
			// 		{ label: 'Đã in', icon: '', routerLink: ['printed'], isShow: true },
			// 		{ label: 'Đã cắt', icon: '', routerLink: ['cuted'], isShow: true },
			// 		{ label: 'Đã khắc', icon: '', routerLink: ['engraved'], isShow: true },
			// 		{ label: 'Đã hoàn thiện', icon: '', routerLink: ['improved'], isShow: true },
			// 		{ label: 'Hoàn thành', icon: '', routerLink: ['complete'], isShow: true },
			// 		{ label: 'Đã ship', icon: '', routerLink: ['shipped'], isShow: true },
			// 		{ label: 'Hủy ship', icon: '', routerLink: ['cancel-ship'], isShow: true },
			// 	]
			// },

		];

		if (!Utils.compareData(menuModel, this.menuCache)) {
			this.menuCache = JSON.parse(JSON.stringify(menuModel));
			// Hanlde add endPoint Path Url
			this.model = menuModel.map((menu) => {
				// if (menu?.routerLink?.length) menu.routerLink = ['/' + menu.routerLink];
				if (menu?.items?.length) {
					menu.items.forEach((subMenu) => {
						subMenu.routerLink = [
							'/' + menu.routerLink + '/' + subMenu.routerLink,
						];
					});
				}
				return menu;
			});
		}
	}

	onMenuClick() {
		this.appMain.menuClick = true;
	}

	activeSubMenu: boolean = false;
}
