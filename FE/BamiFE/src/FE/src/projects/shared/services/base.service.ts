
import { DestroyRef, inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { defer, iif, Observable } from "rxjs";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { Utils } from "@mylib-shared/utils";
import { Page } from "@mylib-shared/models/page";
import { IEnvironment } from "@mylib-shared/interfaces/environment.interface";
import { environment } from "@shared/environments/environment";

export interface ApiRequestConfig {
	body?: any,
	params?: HttpParams,
	url?: string, 
	destroyRef?: DestroyRef,
}

@Injectable({
	providedIn: 'root'
})

export abstract class BaseService {
	//
	http: HttpClient = inject(HttpClient);
    protected abstract featureName: string;
	Utils = Utils;

	constructor() {
        this.domainApi = environment.api;
	}

    domainApi: string;
	componentName: string;

	setParams(page: Page, dataFilter: any) {
		return Utils.setParamGetList(page, dataFilter);
	}

	httpGet(endpoint: string, config?: ApiRequestConfig): Observable<any> {
		const { url, params, destroyRef } = this.getApiRequestConfig(endpoint, config);
		return iif(
			() => !!destroyRef,
			defer(() => this.http.get(url, { params }).pipe(takeUntilDestroyed(destroyRef))),
			defer(() => this.http.get(url, { params }))
		)
	}

	httpPost(endpoint: string, config: ApiRequestConfig) {
        const { url, body, params } = this.getApiRequestConfig(endpoint, config);
		return this.http.post(url, body, { params });
	}

	httpPut(endpoint: string, config?: ApiRequestConfig) {
        const { url, body, params } = this.getApiRequestConfig(endpoint, config);
		return this.http.put(url, body, { params });
	}

	httpPatch(endpoint: string, config: ApiRequestConfig) {
        const { url, body, params } = this.getApiRequestConfig(endpoint, config);
		return this.http.patch(url, body, { params });
	}

	httpDelete(endpoint: string, params: HttpParams = new HttpParams()) {
        const { url } = this.getApiRequestConfig(endpoint);
		return this.http.delete(url, { params });
	}

	
	getApiRequestConfig(endpoint: string, config: ApiRequestConfig = {}): ApiRequestConfig {
		let url = `${this.domainApi}/${this.featureName}/${endpoint}`;
        if(endpoint.includes('https://') || endpoint.includes('http://')) {
            url = endpoint;
        }

		let params: HttpParams = config?.params || new HttpParams();
        let body: any = config?.body || null;
        let destroyRef: DestroyRef = config?.destroyRef || null;
		//
		return { url, body, params, destroyRef };
	}

}