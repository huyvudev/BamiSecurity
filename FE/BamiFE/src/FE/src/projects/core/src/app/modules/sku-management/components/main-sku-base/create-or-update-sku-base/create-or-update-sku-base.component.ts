import { SkuBaseService } from './../../../services/sku-base.service';
import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ComponentBase } from '@shared/component-base';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { SkuBase } from '../../../model/skubase.models';
import { required } from '@shared/validators/validator-common';
import { finalize } from 'rxjs';

@Component({
	selector: 'app-create-or-update-sku-base',
	templateUrl: './create-or-update-sku-base.component.html',
	styleUrls: ['./create-or-update-sku-base.component.scss']
})
export class CreateOrUpdateSkuBaseComponent extends ComponentBase {

	constructor(
		injector: Injector,
		private dialogConfig: DynamicDialogConfig,
		private dialogRef: DynamicDialogRef,
		private fb: FormBuilder,
		private skuBaseService: SkuBaseService,
	) {
		super(injector)
	}

	postForm: FormGroup;

	skuBase: SkuBase;

	ngOnInit() {
		this.skuBase = this.dialogConfig.data;
		this.setForm();
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.skuBase?.id],
			code: [this.skuBase?.code, required()],
			description: [this.skuBase?.description],
		});

		if (!this.skuBase?.id) {
			this.postForm.removeControl('id');
		}
	}

	onSumbit() {
		if (!this.inValidForm(this.postForm)) {
			const body: SkuBase = this.postForm.value;
			this.isLoading = true;
			let successMessage = body.id ? "Cập nhật skuBase thành công !" : 'Thêm skuBase thành công !'
			if (body.id) {
				this.skuBaseService.update(body)
					.pipe((finalize(() => this.isLoading = false)))
					.subscribe((res) => {
						if (this.checkStatusResponse(res, successMessage)) {
							this.dialogRef.close(true);
						}
					})
			}
			else {
				this.skuBaseService.create(body)
					.pipe((finalize(() => this.isLoading = false)))
					.subscribe((res) => {
						if (this.checkStatusResponse(res, successMessage)) {
							this.dialogRef.close(true);
						}
					})
			}
		}
	}

	close() {
		this.dialogRef.close();
	}
}
