import { Injectable } from '@angular/core';
import { BaseService } from '@shared/services/base.service';
import { Partner, PartnerCreate, PartnerUpdate } from '../../partner/model/partner.model';
import { Page } from '@mylib-shared/models/page';
import { Observable } from 'rxjs';
import { IResponseList } from '@mylib-shared/interfaces/response.interface';
import { Store } from '../models/store.model';

@Injectable({
	providedIn: 'root'
})
export class StoreService extends BaseService {

	protected override featureName: string = 'api/brand/store';

	create(body: Store) {
		return this.httpPost('add', { body })
	}

	getAll(page: Page, dataFilter: any): Observable<IResponseList<Store>> {
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params })
	}

	update(body: Store) {
		return this.httpPut('update', { body })
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}

	findById(id: number) {
		return this.httpGet(`/${id}`)
	}

}
