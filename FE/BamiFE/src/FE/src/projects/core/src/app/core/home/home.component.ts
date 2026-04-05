import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { DashboardFilterModel } from '../models/home.model';
import { PageBase } from '@shared/core/component-bases.ts/page-base';
@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.scss'],
})
export class HomeComponent extends PageBase {
	constructor(
		private breadcrumbService: BreadcrumbService,
	) {
		super();
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Tổng quan' },
		]);
	}

	dataFilter = new DashboardFilterModel();

	avatarDefault = '../../../assets/layout/images/avatar.png';

	systemColor = '#0045DC';
	
	// Tạo Biểu đồ thực chi 
    refreshTemplate = true;

 	ngOnInit() {
		this.dataFilter.firstDate = new Date();
		this.dataFilter.endDate = new Date();
	}

	listProduct: {name: string, id: number}[] = [];
    listTrading: {name: string, tradingProviderId: number}[] = [];

}
