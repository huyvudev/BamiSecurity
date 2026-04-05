import { Injectable } from '@angular/core';
import { BaseService } from '@shared/services/base.service';
import { PartnerType, PartnerTypeCreate, PartnerTypeUpdate } from '../model/partner.model';
import { Observable } from 'rxjs';
import { Page } from '@mylib-shared/models/page';
import { IResponseList } from '@mylib-shared/interfaces/response.interface';

@Injectable()
export class PartnerTypeService extends BaseService {

	protected override featureName: string = 'api/partner/partner-type';

	getAll(page: Page, dataFilter?: any): Observable<IResponseList<PartnerType>> {
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params })
	}

	create(body: PartnerTypeCreate) {
		return this.httpPost('add', { body })
	}

	update(body: PartnerTypeUpdate) {
		return this.httpPut('update', { body })
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}

	findById(id: number) {
		return this.httpGet(`/${id}`)
	}
}
