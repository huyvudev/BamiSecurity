import { EStatusOderItem } from './../../../order-item/const/status-item.const';
import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentBase } from '@shared/component-base';
import { required } from '@shared/validators/validator-common';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { finalize } from 'rxjs';
import { BatchService } from '../../../batch/services/batch.service';
import { Item } from '../../../batch/models/batch-create.model';

@Component({
	selector: 'app-status-update',
	templateUrl: './status-update.component.html',
	styleUrls: ['./status-update.component.scss']
})
export class StatusUpdateComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private fb: FormBuilder,
		private dialogService: DialogService,
		private routeActive: ActivatedRoute,
		private batchService: BatchService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Trạng thái " }
		]);
	}

	postForm: FormGroup;
	EStatusOderItem = EStatusOderItem
	status: any

	ngOnInit(): void {
		this.status = this.routeActive.snapshot.data['status'];
		this.setForm();
	}

	setForm() {
		this.postForm = this.fb.group({
			templates: ["", required()],
			itemStatus: [this.status, required()]
		});
	}

	onSubmit() {
		if (!this.inValidForm(this.postForm)) {
			let template = this.postForm.get('templates')?.value
			let items: Item[]
			items = this.getOrderItem(template)
			let body: any = {
				items,
				itemStatus: this.postForm.get('itemStatus').value
			}
			let successMessage = "Cập nhật trạng thái item thành công !"
			this.batchService.updateStatusItem(body)
				.pipe(finalize(() => this.isLoading = false))
				.subscribe((response) => {
					if (this.checkStatusResponse(response, successMessage)) {
						this.setForm()
					}
				})
		}
	}

	getOrderItem(input: string): Item[] {
		const items = input.split(/[\n,]/).map(item => item.trim());
		return items.map(item => {
			const cleanedItem = item.replace(/^#0*/, '');
			const [orderId, itemIndex] = cleanedItem.split('_').map(Number);
			return new Item(orderId, itemIndex);
		});
	}

}

