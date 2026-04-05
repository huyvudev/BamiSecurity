import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { TokenService } from "../../my-lib/src/lib/shared/services/auth/token.service";
import { Observable } from "rxjs";
import { AppAuthService } from "projects/my-lib/src/lib/shared/services/auth/app-auth.service";
import { MessageService } from "primeng/api";
import { PermissionsService } from "projects/my-lib/src/lib/shared/services/auth/permission.service";
import { BaseConsts } from "@mylib-shared/consts/base.consts";

@Injectable({
	providedIn: 'root'
})
export class AuthGuard {
	constructor(
		private _tokenService: TokenService,
		private _authService: AppAuthService,
		private router: Router,
		private _permissionService: PermissionsService,
		private messageService: MessageService,
	) { }

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		return new Promise<boolean>((resolve) => {
			const checkPermission = () => {
				try {
					const permissions = this._permissionService.permissionSubject.getValue();
					const nextStateUrl = state.url;
					const currentUrl = window.location.pathname;
					let activeRoute: boolean = false;
					//
					if(route.data?.['permissionKey']) {
						activeRoute = (permissions || []).includes(route.data['permissionKey']);
						if(!activeRoute) {
							this.messageService.add({ severity: 'error', detail: 'Tài khoản không có quyền truy cập' });
							if (nextStateUrl === currentUrl) {
								location.href = '/home';
							}
						}
					} else {
						activeRoute = true;
					}
					return resolve(activeRoute);
				} catch {
					return resolve(false);
				}
			}
			//
			//
			const updateToken = () => {
				this._authService.refreshToken().subscribe({
					next: () => {
						checkPermission();
					},
					error: (err) => {
						this._tokenService.clearToken();
						this.router.navigate([BaseConsts.redirectLoginPath]);
						return resolve(false);
					}
				});
			}

			const token = this._tokenService.getToken();
			const refreshToken = this._tokenService.getRefreshToken();
			if (token) {
				// Nếu đã có token thì tự động redirect về home khi truy cập trang login
				if (state.url === BaseConsts.redirectLoginPath) {
					this.router.navigate(['/home']);
					return resolve(false);
				}
				checkPermission();
			} else {
				if (refreshToken) {
					updateToken();
				} else {
					this._tokenService.clearToken();
					this.router.navigate([BaseConsts.redirectLoginPath]);
					return resolve(false);
				}
			}
		}
		);
	}

	canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		return this.canActivate(route, state);
	}
}
