import { Injectable } from '@angular/core';
import { BaseService } from '@shared/services/base.service';
import { Brand } from '../models/brand.model';
import { IResponseItem, IResponseList } from '@mylib-shared/interfaces/response.interface';
import { Observable } from 'rxjs';
import { Page } from '@mylib-shared/models/page';
import { environment } from '@shared/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class BrandService extends BaseService {

	protected override featureName: string = 'api/brand';
	private api = `${environment.api}/api/brand`

	create(body: Brand): Observable<IResponseItem<any>> {
		return this.http.post<IResponseItem<any>>(`${this.api}/add`, body)
	}

	getAll(page: Page, dataFilter?: any): Observable<IResponseList<Brand>> {
		const params = this.Utils.setParamGetList(page, dataFilter);
		return this.httpGet('find-all', { params })
	}

	update(body: Brand): Observable<IResponseItem<any>> {
		return this.http.put<IResponseItem<any>>(`${this.api}/update`, body)
	}

	delete(id: number) {
		return this.httpDelete(`delete/${id}`)
	}

	findById(id: number): Observable<IResponseItem<Brand>> {
		return this.httpGet(`find/${id}`)
	}
}