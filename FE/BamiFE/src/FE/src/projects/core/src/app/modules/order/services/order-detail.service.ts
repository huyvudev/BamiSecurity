import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OrderDetail } from '../models/order.models';
import { Observable } from 'rxjs';
import { IResponseItem, IResponseList } from '@mylib-shared/interfaces/response.interface';
import { Page } from '@mylib-shared/models/page';
import { Utils } from '@mylib-shared/utils';
import { environment } from '@shared/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class OrderDetailService {
	private api = `${environment.api}/api/order-detail`

	constructor(private http: HttpClient) { }

	create(body: OrderDetail): Observable<IResponseItem<any>> {
		return this.http.post<IResponseItem<any>>(`${this.api}/create`, body)
	}

	getById(id: number): Observable<IResponseItem<OrderDetail>> {
		return this.http.get<IResponseItem<OrderDetail>>(`${this.api}/find/${id}`);
	}

	update(body: OrderDetail): Observable<IResponseItem<OrderDetail>> {
		return this.http.put<IResponseItem<OrderDetail>>(`${this.api}/update`, body);
	}

	updatBase(body: OrderDetail): Observable<IResponseItem<any>> {
		return this.http.put<IResponseItem<any>>(`${this.api}/update-basic-information`, body)
	}

	delete(id: number): Observable<IResponseItem<any>> {
		return this.http.delete<IResponseItem<any>>(`${this.api}/delete/${id}`);
	}

	getAllOrderDetailDone(page: Page, dataFilter?: any): Observable<IResponseList<OrderDetail>> {
		let params = Utils.setParamGetList(page, dataFilter);
		return this.http.get<IResponseList<OrderDetail>>(`${this.api}/find-all-order-detail`, { params })
	}

	approved(idDetail: number): Observable<IResponseList<any>> {
		return this.http.put<IResponseList<any>>(`${this.api}/approve/${idDetail}`, null)
	}

}
