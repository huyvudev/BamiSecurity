import { Injectable } from '@angular/core';
import { BaseService } from '@shared/services/base.service';
import { Sku } from '../model/sku.model';
import { Page } from '@mylib-shared/models/page';
import { Observable } from 'rxjs';
import { IResponseItem, IResponseList } from '@mylib-shared/interfaces/response.interface';
import { environment } from '@shared/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class SkuService extends BaseService {
	private api = `${environment.api}/api/sku`
	protected override featureName: string = 'api/sku';

	create(body: Sku): Observable<IResponseItem<any>> {
		return this.http.post<IResponseItem<any>>(`${this.api}/add`, body)
	}

	getAll(page: Page, dataFilter?: any): Observable<IResponseList<Sku>> {
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params })
	}

	update(body: Sku) {
		return this.httpPut('update', { body })
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}

	findById(id: number) {
		return this.httpGet(`find/${id}`)
	}
}
