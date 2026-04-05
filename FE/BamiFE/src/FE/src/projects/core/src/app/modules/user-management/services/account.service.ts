import { DestroyRef, Injectable } from '@angular/core';
import { IResponseList } from '@mylib-shared/interfaces/response.interface';
import { Page } from '@mylib-shared/models/page';
import { BaseService } from '@shared/services/base.service';
import { IAccount, IAccountItemList, IAccountUpdate } from '../models/account.model';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'any'
})

export class AccountService extends BaseService {

	protected override featureName: string = 'api/core/user';

	getAll(page: Page, dataFilter: any, destroyRef: DestroyRef ): Observable<IResponseList<IAccountItemList>> {
		if(dataFilter?.keyword) {
			const keyword: string = dataFilter.keyword.trim();
			const isUsername = keyword.indexOf(' ') === -1;
			const isFullName = keyword.indexOf(' ') !== -1;
			const isEmail = keyword.indexOf('@') !== -1;
			let fieldSearch: string = 'keyword';
			if(isUsername) fieldSearch = 'username';
			if(isFullName) fieldSearch = 'fullname';
			if(isEmail) fieldSearch = 'email';
			//
			dataFilter = {
				...dataFilter,
				[fieldSearch]: dataFilter.keyword,
				keyword: ''
			}
		}
		//
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params, destroyRef })
	}

	create(body: IAccount) {
		return this.httpPost('add', { body })
	}

	update(body: IAccountUpdate) {
		return this.httpPut('update', { body })
	}

	setPassword(body) {
		return this.httpPut('set-password', { body })
	}

	findById(id: number) {
		return this.httpGet(`/${id}`)
	}

	changeStatus(id: number) {
		return this.httpPut(`change-status/${id}`)
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}

}
