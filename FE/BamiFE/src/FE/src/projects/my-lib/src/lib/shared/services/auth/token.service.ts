import { Inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { BaseConsts } from '../../consts/base.consts';
import { IEnvironment } from '../../interfaces/environment.interface';
import jwt_decode from "jwt-decode";
import { IUserTokenDecode } from '../../interfaces/token.interface';

@Injectable({
    providedIn: 'root'
})
//
export class TokenService {
    constructor(
        private _cookieService: CookieService,
		@Inject('env') environment
    ) {
		this.environment = environment;
	}

	environment: IEnvironment

    getToken() {
        return this._cookieService.get(BaseConsts.authorization.accessToken);
    }

    getRefreshToken() {
        return this._cookieService.get(BaseConsts.authorization.refreshToken);
    }

    clearToken() {
        this._cookieService.delete(BaseConsts.authorization.refreshToken, '/');
        this._cookieService.delete(BaseConsts.authorization.accessToken, '/');
    }

    setToken(accessToken: string, tokenExpireDate: Date | number) {
        this._cookieService.set(BaseConsts.authorization.accessToken, accessToken, tokenExpireDate, '/');
    }

	  setRefreshToken(refreshToken: string, tokenExpireDate: Date | number) {
        this._cookieService.set(BaseConsts.authorization.refreshToken, refreshToken, tokenExpireDate, '/');
    }

    clearAllCookie() {
        this._cookieService.deleteAll();
    }

    setCodeVerifier(codeVerifier: string) {
        this._cookieService.set(BaseConsts.authorization.codeVerifier, codeVerifier, null, '/');
    }

    getCodeVerifier() {
        return this._cookieService.get(BaseConsts.authorization.codeVerifier);
    }

    setState(state: string) {
        this._cookieService.set(BaseConsts.authorization.state, state, null, '/');
    }

    getState() {
        return this._cookieService.get(BaseConsts.authorization.state);
    }

    clearStateAndCodeVerifier() {
        this._cookieService.delete(BaseConsts.authorization.state, '/');
        this._cookieService.delete(BaseConsts.authorization.codeVerifier, '/');
    }

	decode() : IUserTokenDecode {
		const token = this.getToken();
		if (token) {
			const userInfo: any = jwt_decode(token);
			return {
				name: userInfo?.name,
				username: userInfo?.username,
				email: userInfo?.email,
				userId: userInfo?.sub,
				userType: userInfo?.user_type,
			};
		}
		return null;
	}
}
