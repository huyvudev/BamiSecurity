import { DestroyRef, Injectable } from '@angular/core';
import { IResponseItemList, IResponseList } from '@mylib-shared/interfaces/response.interface';
import { Page } from '@mylib-shared/models/page';
import { BaseService } from '@shared/services/base.service';
import { Observable } from 'rxjs';
import { IRole, IRoleItemList, IRoleUpdate } from '../models/role.model';

@Injectable({
	providedIn: 'any'
})

export class RoleService extends BaseService {

	protected override featureName: string = 'api/auth/role';

	getAll(page: Page, dataFilter: any, destroyRef: DestroyRef ): Observable<IResponseList<IRoleItemList>> {
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params, destroyRef })
	}

	getAllNoPaging(destroyRef: DestroyRef ): Observable<IResponseItemList<IRoleItemList>> {
		return this.httpGet('get-all', { destroyRef })
	}

	create(body: IRole) {
		return this.httpPost('add', { body })
	}

	update(body: IRoleUpdate) {
		return this.httpPut('update', { body })
	}

	findById(id: number) {
		return this.httpGet(`find-by-id/${id}`)
	}

	changeStatus(id: number) {
		return this.httpPut(`change-status/${id}`)
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}
}
