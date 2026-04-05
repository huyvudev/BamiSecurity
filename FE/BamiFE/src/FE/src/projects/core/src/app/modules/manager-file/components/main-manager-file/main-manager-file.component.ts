import { Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { Brand } from '../../../name-space/models/brand.model';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { Page } from '@mylib-shared/models/page';
import { ManagerFileService } from '../../services/manager-file.service';
import { finalize } from 'rxjs';
import { ManagerFile } from '../../models/manager-file.model';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

@Component({
	selector: 'app-main-manager-file',
	templateUrl: './main-manager-file.component.html',
	styleUrls: ['./main-manager-file.component.scss']
})

export class MainManagerFileComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
		private managerFileService: ManagerFileService,
		private http: HttpClient,
		private router: Router,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản file" },

		]);
	}

	columns: IColumn[] = []
	rows: ManagerFile[] = [];
	dataFilter: any

	ngOnInit() {
		this.setColumn()
		this.setPage();
	}

	setColumn() {
		this.columns = [
			{
				field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT,
			},
			{
				field: 'name', header: "Name", width: 15, type: ETableColumnType.TEXT, otherType: ETableColumnType.LINK,
				action: (value) => (this.getFile(value?.name))
			},
			{
				field: 'type', header: "type", width: 15, type: ETableColumnType.TEXT,
			},
			{
				field: 'fileName', header: "fileName", width: 15, type: ETableColumnType.TEXT,
			},
			{
				field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

			}
		]
	}

	public setPage(event?: Page) {
		this.dataFilter = { ...this.dataFilter, ...event }
		this.isLoading = true;
		this.managerFileService.getAll(this.page, this.dataFilter)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				console.log(res)
				this.rows = this.page.getRowLoadMore(this.rows, res.results);
				this.page.totalItems = res?.results?.totalResults;
				if (this.rows?.length) {

				}
			}
			);
	}

	getFile(name: any) {
		this.isLoading = true;
		this.managerFileService.getMetaData(name)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				if (res?.metadata) {
					let metadata = res?.metadata
					console.log(metadata)

					const readPromises = metadata.map(meta => {
						const url = `${meta.datanode}/api/datanode/read?name=${meta.name}&index=${meta.index}`;
						return lastValueFrom(this.http.get(url, { responseType: 'arraybuffer' }));
					});

					Promise.allSettled(readPromises).then(results => {
						const chunks = [];
						results.forEach((res, index) => {
							console.log(res);
							if (res.status === 'fulfilled') {
								chunks.push(new Uint8Array(res.value));
							} else {
								console.error(`Failed to fetch data for metadata at index ${index}:`, res.reason);
							}
						});

						const mergedBlob = new Blob(chunks, { type: metadata[0].type });
						const url = URL.createObjectURL(mergedBlob);

						const a = document.createElement('a');
						a.href = url;
						a.download = `${metadata[0].name}.${metadata[0].type.split('/')[1]}`;
						a.click();

						URL.revokeObjectURL(url);
					});
				}

			}
			);

	}
}
