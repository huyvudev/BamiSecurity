import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";
import { TokenService } from "projects/my-lib/src/lib/shared/services/auth/token.service";
import { AppAuthService } from "projects/my-lib/src/lib/shared/services/auth/app-auth.service";
import { PermissionsService } from "projects/my-lib/src/lib/shared/services/auth/permission.service";
import { BaseConsts } from "@mylib-shared/consts/base.consts";
import { tap } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})

export class PermissionGuard {
    constructor(
        private _permissionService: PermissionsService,
		private _tokenService: TokenService,
		private _authService: AppAuthService,
		private router: Router,
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> | boolean {
        return new Promise<boolean>((resolve) => {
            // 
            this._permissionService.getPermission()
            .pipe(tap(() => this._authService.setUserInfo(this._tokenService.getToken())))
            .subscribe({
                next: (response: any) => {
                    const permissions = response?.data;
                    this._permissionService.setPermissions(permissions);
                    return resolve(true)
                },
                error: async () => {
					this._tokenService.clearToken();
					this.router.navigate([BaseConsts.redirectLoginPath]);
                    return resolve(false);
                }
            });
        })
    }
}
