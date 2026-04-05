import { Injectable } from '@angular/core';
import { BaseService } from '@shared/services/base.service';
import { ProductMethod } from '../model/product-method.model';
import { Page } from '@mylib-shared/models/page';
import { Observable } from 'rxjs';
import { IResponseList } from '@mylib-shared/interfaces/response.interface';

@Injectable({
	providedIn: 'root'
})
export class ProductMethodService extends BaseService {

	protected override featureName: string = 'api/sku/production-method';

	create(body: ProductMethod) {
		return this.httpPost('add', { body })
	}

	getAll(page: Page, dataFilter?: any): Observable<IResponseList<ProductMethod>> {
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params })
	}

	update(body: ProductMethod) {
		return this.httpPut('update', { body })
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}

	findById(id: number) {
		return this.httpGet(`/${id}`)
	}
}
