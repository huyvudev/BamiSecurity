import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpParams, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Observable, EMPTY, of } from 'rxjs';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import { TokenService } from '../../my-lib/src/lib/shared/services/auth/token.service';
import { AppAuthService } from 'projects/my-lib/src/lib/shared/services/auth/app-auth.service';
import { environment } from '../environments/environment';
import { BaseConsts } from 'projects/my-lib/src/lib/shared/consts/base.consts';
import { ITokenResponse } from 'projects/my-lib/src/lib/shared/interfaces/token.interface';
import { Utils } from 'projects/my-lib/src/lib/shared/utils';
import { ErrorCode } from 'projects/my-lib/src/lib/shared/consts/error-code.const';
import { Router } from '@angular/router';

@Injectable()
export class HanldeHttpInterceptor implements HttpInterceptor {
    constructor(
        private messageService: MessageService,
        private _tokenService: TokenService,
        private _authService: AppAuthService,
        private router: Router,
    ) {}

    requestPostApi: string;
	env = environment;

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let token: string = this._tokenService.getToken();
        let refreshToken: string = this._tokenService.getRefreshToken();
        const method: string = request.method;

        const methodCreateOrUpdates = ['POST', 'PUT', 'PATCH'];
        const paramRemoves = ['unAuthorize', 'enableMultileRequest'];
        const unAuthorize = request.params.get('unAuthorize') === true.toString();
        const enableMultileRequest = !!request.params.get('enableMultileRequest');

        // BLOCK DOUBLE CLICK CALL API POST, PUT, PATH
        if(methodCreateOrUpdates.includes(method) && !enableMultileRequest) {
            if(request.url !== this.requestPostApi) {
                this.requestPostApi = request.url;
            } else {
                return EMPTY;
            }
        }

        let params = new HttpParams();
        let paramKeys = request.params.keys();
        //
        paramKeys.forEach(key => {
            // Get value full của key nếu nhiều value thì for append mới truyền key nhiều giá trị được
            if(!paramRemoves.includes(key)){
                request.params.getAll(key).forEach(value => {
                    params = params.append(key, value);
                })
            }
        })
        // Add Token to api
        if(token) {
            let headers = !unAuthorize ? {'Authorization': `Bearer ${token}`} : {};
            request = request.clone({
                setHeaders: headers,
                params: params,
            });
        }

        return next.handle(request).pipe(
            finalize(() => {
                this.requestPostApi = '';
            }),
            catchError(err => {
                if (err instanceof HttpErrorResponse) {
                    if(err?.status === 401 && !token) {
						if(refreshToken) {
							return this.refreshToken(request, next);
						} else {
							this.router.navigate([BaseConsts.redirectLoginPath]);
							return EMPTY;
						}
						//
                    } else {
                        let message: string = '';
                        let statusCode: number = err?.status;
                        switch (statusCode) {
                            case 404:
                                message = 'Đường dẫn không tồn tại';
                                break;
                            case 401:
                                message = 'Tài khoản không có quyền truy cập';
                                break;
                            case 403:
                                message = err?.error?.message || err?.message;
                                break;
                            default:
                                message = BaseConsts.messageError;
                        }
                        //
						Utils.log(`statusCode ${err?.status}: ${ErrorCode.list[err?.status] || ''}`, `${message || err?.error?.message || err?.message}`)
						if(statusCode !== 200 && message) {
							this.messageService.add({
								severity: 'error',
								detail: message,
								life: 2000
							});
						} 
                    }
                }
                throw (err);
            }),

        );
    }

    refreshToken(request: HttpRequest<any>, next: HttpHandler) : Observable<HttpEvent<any>> {
		return this._authService.refreshToken().pipe(
			switchMap((token: ITokenResponse) => {
				request = request.clone({
					setHeaders: {
						'Authorization': `Bearer ${token.access_token}`,
					},
				});
				return next.handle(request);
			}),
			catchError(() => {
				this._tokenService.clearToken();
				return next.handle(request);
			})
		);
    }
}
