import { Injectable } from '@angular/core';
import { BaseService } from '@shared/services/base.service';

@Injectable({
	providedIn: 'any'
})
export class DashboardService extends BaseService {

	protected override featureName: string = '/api/core/dashboard';
}
