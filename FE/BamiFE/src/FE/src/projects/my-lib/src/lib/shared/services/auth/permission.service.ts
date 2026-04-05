import { HttpClient, HttpParams } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/internal/BehaviorSubject";
import { Observable } from "rxjs/internal/Observable";
import { IEnvironment } from "../../interfaces/environment.interface";
import { IResponseItem, IResponseItemList } from "../../interfaces/response.interface";
import { IPermissionConfig } from "@mylib-shared/models/base.model";

@Injectable({
    providedIn: 'root'
})
export class PermissionsService {

    constructor(
		private http: HttpClient,
		@Inject('env') environment
	) {
		this.environment = environment;
	}

    permissions: string[] = [];
	environment: IEnvironment;

    permissionSubject = new BehaviorSubject<any>(null);
    getPermissions$ = this.permissionSubject.asObservable();
    //

    setPermissions(permissions) {
        this.permissionSubject.next(permissions)
    }

    getPermissions() {
        this.getPermissions$.subscribe((response) => {
            this.permissions = response || [];
        });
    }

    activeGetPermisionSubject = new BehaviorSubject<any>(null);
    fetchPermission$ = this.activeGetPermisionSubject.asObservable();

    activeCheckPermission() {
        this.activeGetPermisionSubject.next(this.permissions);
    }

    /**
     * Kiểm tra có permission không
     * @param permissionKey
     * @returns
     */
    isGrantedRoot(permissionKey: string): boolean {
        const permissions = this.permissionSubject.getValue() || [];
        return permissions.includes(permissionKey);
    }

    getPermission() {
        return this.http.get(`${this.environment.api}/api/auth/permission/get-permissions`);
    }

	checkPermission(keys:string | string[]) : Observable<IResponseItem<boolean>> {
        const api = `${this.environment.api}/api/users/permission/check`;
        let params = new HttpParams();
        if(typeof keys === 'string') keys = [keys];
        if(Array.isArray(keys)) {
            keys.forEach((key) => {
                params = params.append('permissionKeys', key);
            })
        }
        //
        return this.http.get<IResponseItem<boolean>>(api, {params});
    }
    
    checkListPermission(keys:string[]): Observable<IResponseItem<Record<string, boolean>>>{
        const api = `${this.environment.api}/api/users/permission/check-list`;
        let params = new HttpParams();
        keys.forEach((key) => {
            params = params.append('permissionKeys', key);
        })
        return this.http.get<IResponseItem<Record<string, boolean>>>(api, {params});
    }

	getAllPermisionKey(): Observable<IResponseItemList<IPermissionConfig>> {
		const api: string = `${this.environment.api}//api/auth/permission/find-all`;
		return this.http.get<IResponseItemList<IPermissionConfig>>(api)
	}
}
