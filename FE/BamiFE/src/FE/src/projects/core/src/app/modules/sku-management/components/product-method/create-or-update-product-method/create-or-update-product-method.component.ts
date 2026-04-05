import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ComponentBase } from '@shared/component-base';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ProductMethodService } from '../../../services/product-method.service';
import { ProductMethod } from '../../../model/product-method.model';
import { required } from '@shared/validators/validator-common';
import { finalize } from 'rxjs';

@Component({
	selector: 'app-create-or-update-product-method',
	templateUrl: './create-or-update-product-method.component.html',
	styleUrls: ['./create-or-update-product-method.component.scss']
})
export class CreateOrUpdateProductMethodComponent extends ComponentBase {
	constructor(
		injector: Injector,
		private dialogConfig: DynamicDialogConfig,
		private dialogRef: DynamicDialogRef,
		private fb: FormBuilder,
		private productMethodService: ProductMethodService,
	) {
		super(injector)
	}
	postForm: FormGroup;

	productMethod: ProductMethod;

	ngOnInit() {
		this.productMethod = this.dialogConfig.data;
		this.setForm();
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.productMethod?.id],
			name: [this.productMethod?.name, required()],
			code: [this.productMethod?.code, required()],
		});

		if (!this.productMethod?.id) {
			this.postForm.removeControl('id');
		}
	}

	onSumbit() {
		if (!this.inValidForm(this.postForm)) {
			const body: ProductMethod = this.postForm.value;
			this.isLoading = true;
			let successMessage = body.id ? "Cập nhật phương thức sản xuất thành công !" : 'Thêm phương thức sản xuất thành công !'
			if (body.id) {
				this.productMethodService.update(body)
					.pipe((finalize(() => this.isLoading = false)))
					.subscribe((res) => {
						if (this.checkStatusResponse(res, successMessage)) {
							this.dialogRef.close(true);
						}
					})
			}
			else {
				this.productMethodService.create(body)
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
