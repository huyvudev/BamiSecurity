import {Component, Injector} from '@angular/core';
import { AppMainComponent } from '../main/app.main.component';
import { environment } from 'projects/shared/environments/environment';
import { IUserLogin } from 'projects/my-lib/src/lib/shared/interfaces/user.interface';
import { DefaultImage } from 'projects/my-lib/src/lib/shared/consts/base.consts';
import { TokenService } from '@mylib-shared/services/auth/token.service';
import { AppAuthService } from '@mylib-shared/services/auth/app-auth.service';
import { catchError, of } from 'rxjs';
import { LibHelperService } from '@mylib-shared/services/lib-helper.service';

@Component({
    selector: 'app-topbar',
    template: `
        <div class="layout-topbar">
			<div class="layout-topbar-wrapper">
                <div class="layout-topbar-left">
                    <div class="wrapper-logo">
                        <a href="" class="link">
                            <img 
                                src="assets/layout/images/logo/logo-default.png" class="logo-left"
                                style="width: 100%; height: 30px"
                            />
                        </a>
                    </div>
                </div>
                <div class="layout-topbar-right fadeInDown">
                    <ul class="layout-topbar-actions">
                        <li #profile class="topbar-item profile-item" [ngClass]="{'active-topmenuitem': appMain.activeTopbarItem === profile}">
                            <a href="#" (click)="appMain.onTopbarItemClick($event,profile)">
                                <p-avatar image="{{ userLogin?.avatar }}" size="large" shape="circle"/>
                                <span class="profile-info-wrapper">
                                    <span>{{ userLogin?.fullName || userLogin?.username }}</span>
                                </span>
                                <i class="pi pi-angle-down ml-3"></i>
                            </a>
                            <ul class="profile-item-submenu fadeInDown">
								<li style="cursor: pointer; border-bottom: 1px solid #0000001a;" class="layout-submenu-item p-3 pl-5" >
									<i class="pi pi-user icon"></i>
									<div class="menu-text ml-2">
										<p>Tài khoản</p>
									</div>
								</li>
								<li style="cursor: pointer;" (click)="logout()" class="layout-submenu-item p-3 pl-5">
									<i class="pi pi-sign-out icon"></i>
									<div class="menu-text ml-2">
										<p>Đăng xuất</p>
									</div>
								</li>
							</ul>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    `,
})
export class AppTopBarComponent {

    activeItem: number;

    constructor(
		public appMain: AppMainComponent,
		private _authService: AppAuthService,
		private _tokenService: TokenService,
		private _libHelperService: LibHelperService,
	) {
    }

    userLogin: IUserLogin;
    product: boolean = false;
    env = environment;
    // UserManagerConst = UserManagerConst
    permissionWebs: string[];

    ngOnInit() {
        this.userLogin = this._authService.userSubject.getValue();
		this._authService.getUserInfo().pipe(
            catchError(() => of())
        )
        .subscribe((res) => {
            if(this._libHelperService.checkStatusResponse(res)) {
                this.userLogin = {
                    ...this.userLogin,
                    ...res.data,
                    avatar: res.data.avatarImageUri ? `${environment.api}/media/${res.data.avatarImageUri}` : DefaultImage.AVATAR
                }
            }
        })
        
	}

    logout() {
        this._authService.logout().subscribe((res) => {
            this._tokenService.clearToken();
            window.location.href = `auth/login`;
        });
    }

    toggleProductPopup(product: boolean) {
        this.product = !product;
    }
}
