import { Inject, Injectable, Injector } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import * as CryptoJS from 'crypto-js';
import { BehaviorSubject, Observable, concatMap, map, mergeMap, of, tap } from 'rxjs';
import { TokenService } from './token.service';
import { AuthParamConsts } from '../../consts/base.consts';
import { ITokenDecode, ITokenResponse } from '../../interfaces/token.interface';
import { Utils } from '../../utils';
import jwtDecode from 'jwt-decode';

export interface UserRegister {
    email: string;
    password: string;
    fullName: string;
    userCode: string;
}

export interface PasswordResetRequest {
    email: string;
    code: string;
    newPassword: string;
    confirmPassword: string;
}

@Injectable({
    providedIn: 'root'
})
export class AppAuthService {

    rememberMe: boolean;
    environment: any;
    constructor(
        private _tokenService: TokenService,
        private http: HttpClient,
        @Inject('env') environment
    ) {
        this.environment = environment;
        this.api = environment.api;
        this.urlAuthLogin = environment.api;
    }

    api: string;
    urlAuthLogin: string;
	userSubject = new BehaviorSubject<any>(null);

    logout(): Observable<any> {
        const headers = new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded',
            Accept: "text/plain",
        });
        //
        let params = new HttpParams();
        return this.http.post<any>(`${this.api}/connect/logout`, params, { headers: headers });
    }

    register(user: UserRegister | any) {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            Accept: "text/plain",
        });
        return this.http.post<any>(`${this.api}/api/core/user/register`, user, { headers: headers });
    }

    verifyOTP(user: UserRegister | any, otp: string) {

        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            Accept: "text/plain",
        });

        return this.http.put<any>(`${this.api}/api/core/user/verify?username=${user.email}&otp=${otp}`, {});
    }

    resendOTP(user: UserRegister | any) {
        return this.http.get<any>(`${this.api}/api/core/user/resend-otp?username=${user.email}`);
    }

    getAuthenticateLogout(returnUrl: string): Observable<any> {
        const headers = new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded',
            Accept: 'text/plain',
        });

        const params = new HttpParams().set('returnUrl', returnUrl);

        return this.http.get<any>(`${this.api}/authenticate/logout`, { headers: headers, params: params });
    }

    connectToken(username: string, password: string) {
        let url = `${this.urlAuthLogin}/connect/token`;
        const params = new HttpParams()
            .set('grant_type', AuthParamConsts.grantTypeAuthorization)
            .set('client', AuthParamConsts.client)
            .set('secret', AuthParamConsts.clientSecret)
            .set('scope', AuthParamConsts.scope)
            .set('username', username)
            .set('password', password);
        //
        const headers = new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded',
            Accept: "text/plain",
        });

        return this.http.post(url, params, { headers: headers }).pipe(
            concatMap((response: ITokenResponse) => {                //
                this.setTokenExpire(response?.expires_in, response?.access_token, response?.refresh_token);

                this._tokenService.clearStateAndCodeVerifier();
                //
                return of(response);
            })
        );
    }

    setUserInfo(token) {
        const tokenDecode: ITokenDecode = jwtDecode(token);
        const userInfo = {
            username: tokenDecode?.username,
            userType: tokenDecode?.user_type,
            id: +(tokenDecode?.sub || tokenDecode?.user_id),
            fullName: tokenDecode?.name,
        }
        this.userSubject.next(userInfo);
        return of(userInfo)
    }

    getUserInfo() {
        return this.http.get<any>(`${this.api}/api/core/user/find-by-user`);
    }

    refreshToken() {
        let url = `${this.urlAuthLogin}/connect/token`;
        const refreshToken = this._tokenService.getRefreshToken();
        const params = new HttpParams()
            .set("refresh_token", refreshToken)
            .set("grant_type", AuthParamConsts.grantTypeRefreshToken)
            .set("client", AuthParamConsts.clientId)
            .set("secret", AuthParamConsts.clientSecret);
        //
        const headers = new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded',
            Accept: "text/plain",
        });
        //
        return this.http.post(url, params, { headers: headers }).pipe(
            tap((response: ITokenResponse) => {
                console.log('setTokenExpire');
                this.setTokenExpire(response?.expires_in, response?.access_token, response?.refresh_token);
            })
        );
    }

    setTokenExpire(expiresIn, token, refreshToken) {
        const tokenExpireDate = (expiresIn ? (+expiresIn / 3600) : 1) / 24; // Quy đổi thời gian sang đơn vị ngày
        this._tokenService.setToken(token, tokenExpireDate);
        this._tokenService.setRefreshToken(refreshToken, 3);	// Refresh token thời hạn 3 ngày
    }


    // /api/core/user/forgot-password/send-email

    forgotPasswordRequest(email: string) {
        return this.http.post<any>(`${this.environment.api}/api/core/user/forgot-password/send-email?email=${email}`, {});
    }

    changePassword(request: PasswordResetRequest) {
        return this.http.put<any>(`${this.api}/api/core/user/forgot-password/confirm`, request);

    }
}
