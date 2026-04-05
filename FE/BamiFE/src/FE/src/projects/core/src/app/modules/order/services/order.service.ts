import { Injectable } from '@angular/core';
import { IResponseItem, IResponseList } from '@mylib-shared/interfaces/response.interface';
import { Observable } from 'rxjs';
import { Order } from '../models/order.models';
import { Page } from '@mylib-shared/models/page';
import { HttpClient } from '@angular/common/http';
import { Utils } from '@mylib-shared/utils';
import { environment } from '@shared/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class OrderService {
	private api = `${environment.api}/api/order`
	constructor(private http: HttpClient) { }

	getAll(page: Page, dataFilter?: any): Observable<IResponseList<Order>> {
		let params = Utils.setParamGetList(page, dataFilter);
		return this.http.get<IResponseList<Order>>(`${this.api}/find-all`, { params })
	}

	create(body: Order): Observable<IResponseItem<any>> {
		return this.http.post<IResponseItem<any>>(`${this.api}/add`, body)
	}

	update(body: Order): Observable<IResponseItem<any>> {
		return this.http.put<IResponseItem<any>>(`${this.api}/update`, body)
	}


	getById(id: number): Observable<IResponseItem<Order>> {
		return this.http.get<IResponseItem<Order>>(`${this.api}/find/${id}`);
	}

	approve(id : number ) : Observable<IResponseItem<any>>{
		return this.http.put<IResponseItem<any>>(`${this.api}/approve-order?id=${id}`,null)
	}

	delete(id: number) : Observable<IResponseItem<any>> {
		return this.http.delete<IResponseItem<any>>(`${this.api}/delete/${id}`)
	}
}
