import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@shared/environments/environment';
import { SkuSize } from '../model/sku-size.model';
import { Observable } from 'rxjs';
import { IResponseItem } from '@mylib-shared/interfaces/response.interface';

@Injectable({
	providedIn: 'root'
})
export class SkuSizeService {

	private api = `${environment.api}/api/sku/sku-size`
	constructor(private http: HttpClient) { }

	create(body: SkuSize): Observable<IResponseItem<any>> {
		return this.http.post<IResponseItem<any>>(`${this.api}/add`, body)
	}

	getById(id: number): Observable<IResponseItem<SkuSize>> {
		return this.http.get<IResponseItem<SkuSize>>(`${this.api}/find/${id}`);
	}

	update(body: SkuSize): Observable<IResponseItem<SkuSize>> {
		return this.http.put<IResponseItem<SkuSize>>(`${this.api}/update`, body);
	}

	delete(id: number): Observable<IResponseItem<any>> {
		return this.http.delete<IResponseItem<any>>(`${this.api}/delete/${id}`);
	}
}
