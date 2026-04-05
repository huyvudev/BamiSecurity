import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Page } from '@mylib-shared/models/page';
import { Utils } from '@mylib-shared/utils';

@Injectable({
	providedIn: 'root'
})
export class ManagerFileService {
	private api = 'http://localhost:3000/api/namenode'
	constructor(private http: HttpClient) { }

	getAll(page: Page, dataFilter?: any): any {
		let params = Utils.setParamGetList(page, dataFilter);
		return this.http.get(`${this.api}/listFile`, { params })
	}

	getMetaData(name: any): any {
		return this.http.get(`${this.api}/readFile?name=${name}`)
	}
}
