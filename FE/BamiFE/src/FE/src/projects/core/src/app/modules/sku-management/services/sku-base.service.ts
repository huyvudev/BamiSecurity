import { Injectable } from '@angular/core';
import { BaseService } from '@shared/services/base.service';
import { SkuBase } from '../model/skubase.models';
import { Page } from '@mylib-shared/models/page';
import { Observable } from 'rxjs';
import { IResponseList } from '@mylib-shared/interfaces/response.interface';

@Injectable({
	providedIn: 'root'
})
export class SkuBaseService extends BaseService {

	protected override featureName: string = 'api/sku/sku-base';

	create(body: SkuBase) {
		return this.httpPost('add', { body })
	}

	getAll(page: Page, dataFilter
		?: any): Observable<IResponseList<SkuBase>> {
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params })
	}

	update(body: SkuBase) {
		return this.httpPut('update', { body })
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}

	findById(id: number) {
		return this.httpGet(`/${id}`)
	}
}
