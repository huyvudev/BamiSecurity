import { Injectable } from '@angular/core';
import { BaseService } from '@shared/services/base.service';
import { Partner, PartnerCreate, PartnerUpdate } from '../model/partner.model';
import { Page } from '@mylib-shared/models/page';
import { Observable } from 'rxjs';
import { IResponseList } from '@mylib-shared/interfaces/response.interface';
@Injectable({
	providedIn: 'root'
})
export class PartnerService extends BaseService {

	protected override featureName: string = 'api/partner';

	create(body: PartnerCreate) {
		return this.httpPost('add', { body })
	}

	getAll(page: Page, dataFilter?: any): Observable<IResponseList<Partner>> {
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params })
	}

	update(body: PartnerUpdate) {
		return this.httpPut('update', { body })
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}

	findById(id: number) {
		return this.httpGet(`/${id}`)
	}
}
