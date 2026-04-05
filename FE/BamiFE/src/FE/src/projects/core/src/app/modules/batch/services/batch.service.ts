import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@shared/environments/environment';
import { BatchCreate } from '../models/batch-create.model';
import { Observable } from 'rxjs';
import { IResponseItem, IResponseList } from '@mylib-shared/interfaces/response.interface';
import { Page } from '@mylib-shared/models/page';
import { Batch, BatchDetail, ItemOrder } from '../models/batch.model';
import { Utils } from '@mylib-shared/utils';

@Injectable({
	providedIn: 'root'
})
export class BatchService {
	private api = `${environment.api}/api/order/batch`
	constructor(private http: HttpClient) { }

	create(body: BatchCreate): Observable<IResponseItem<any>> {
		return this.http.post<IResponseItem<any>>(`${this.api}/add`, body)
	}

	update(body: BatchCreate): Observable<IResponseItem<BatchCreate>> {
		return this.http.put<IResponseItem<BatchCreate>>(`${this.api}/update`, body);
	}

	delete(id: number): Observable<IResponseItem<any>> {
		return this.http.delete<IResponseItem<any>>(`${this.api}/delete/${id}`);
	}

	getAll(page: Page, dataFilter?: any): Observable<IResponseList<Batch>> {
		let params = Utils.setParamGetList(page, dataFilter);
		return this.http.get<IResponseList<Batch>>(`${this.api}/find-all`, { params })
	}

	getItemInBatch(ids?: any): Observable<any> {
		const params = ids.reduce((acc, id) => acc.append('id', id.toString()), new HttpParams());
		return this.http.get<Observable<any>>(`${this.api}/find-items-by-batch-ids`, { params })
	}

	getById(id: number): Observable<IResponseItem<BatchDetail>> {
		return this.http.get<IResponseItem<BatchDetail>>(`${this.api}/find/${id}`);
	}

	updateStatusItem(body: any): Observable<IResponseItem<any>> {
		return this.http.put<IResponseItem<BatchCreate>>(`${this.api}/update-item-status`, body);
	}

	updateNoteBatch(body: any): Observable<IResponseItem<any>> {
		return this.http.put<IResponseItem<BatchCreate>>(`${this.api}/update-note`, body);
	}
}
