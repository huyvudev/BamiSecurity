import { Injectable } from '@angular/core';
import { IResponseItem, IResponseList } from '@mylib-shared/interfaces/response.interface';
import { Page } from '@mylib-shared/models/page';
import { environment } from '@shared/environments/environment';
import { Observable } from 'rxjs';
import { OrderItem } from '../models/order-item.model';
import { Utils } from '@mylib-shared/utils';
import { HttpClient } from '@angular/common/http';
@Injectable({
	providedIn: 'root'
})
export class OrderItemService {
	private api = `${environment.api}/api/item`
	constructor(private http: HttpClient) { }

	getAll(page: Page, dataFilter?: any): Observable<IResponseList<OrderItem>> {
		let params = Utils.setParamGetList(page, dataFilter);
		return this.http.get<IResponseList<OrderItem>>(`${this.api}/find-all-order-item`, { params })
	}

	getById(id: any): Observable<IResponseItem<OrderItem>> {
		return this.http.get<IResponseItem<OrderItem>>(`${this.api}/find-order-item-by-id/${id}`)
	}

	getByTemplate(page: Page, template: any): Observable<IResponseItem<OrderItem>> {
		let params = Utils.setParamGetList(page, template);
		return this.http.get<IResponseItem<OrderItem>>(`${this.api}/find-order-item-by-template`, { params })
	}

	updateNoteItem(body: any): Observable<IResponseItem<any>> {
		return this.http.put<IResponseItem<any>>(`${this.api}/update-note`, body)
	}
}
