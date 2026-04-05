import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ComponentBase } from '@shared/component-base';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Store } from '../../models/store.model';
import { StoreService } from '../../services/store.service';
import { finalize } from 'rxjs';
import { required } from '@shared/validators/validator-common';

@Component({
	selector: 'app-main-store',
	templateUrl: './main-store.component.html',
	styleUrls: ['./main-store.component.scss']
})
export class MainStoreComponent extends ComponentBase {

	constructor(
		inj: Injector,
		private router: Router,
		private fb: FormBuilder,
		private ref: DynamicDialogRef,
		private dialogRef: DynamicDialogRef,
		private configDialog: DynamicDialogConfig,
		private storeService: StoreService,

	) {
		super(inj)
	}
	postForm: FormGroup;
	store: Store
	brandId: number;
	isCreate: boolean;
	index: number;

	ngOnInit() {
		this.store = this.configDialog?.data?.item
		this.brandId = this.configDialog?.data?.brandId
		this.index = this.configDialog?.data?.index

		console.log(this.brandId)
		this.setForm();
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.store?.id],
			name: [this.store?.name, required()],
			brandId:[this.brandId]
		});

		if (!this.store?.id) {
			this.postForm.removeControl('id');
		}

	}

	onSubmit() {
		if (!this.inValidForm(this.postForm)) {
			if (this.brandId) {
				const body: Store = this.postForm.value;
				this.isLoading = true;
				let successMessage = body?.id ? "Cập nhật store thành công !" : "Thêm store thành công"
				if (body.id) {
					this.storeService.update(body)
						.pipe(finalize(() => this.isLoading = false))
						.subscribe((response) => {
							if (this.checkStatusResponse(response, successMessage)) {
								this.dialogRef.close(response)
							}
						})
				}
				else {
					this.storeService.create(body)
						.pipe(finalize(() => this.isLoading = false))
						.subscribe((response) => {
							if (this.checkStatusResponse(response, successMessage)) {
								this.dialogRef.close(response)
							}
						})
				}
			}
			else {
				const body: Store = this.postForm.value;
				if (this.index !== undefined && this.index != null) {
					this.dialogRef.close(
						{
							body,
							isCreate: true,
							index: this.index,
						}
					)
				}
				else {
					this.dialogRef.close(
						{
							body,
							isCreate: true,
						}
					)
				}
			}
		}
	}

	onClose() {
		this.ref.close()
	}
}

