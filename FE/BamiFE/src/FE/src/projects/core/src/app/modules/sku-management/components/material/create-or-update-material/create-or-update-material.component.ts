import { MaterialService } from './../../../services/material.service';
import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ComponentBase } from '@shared/component-base';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Material } from '../../../model/material.model';
import { required } from '@shared/validators/validator-common';
import { finalize } from 'rxjs';

@Component({
	selector: 'app-create-or-update-material',
	templateUrl: './create-or-update-material.component.html',
	styleUrls: ['./create-or-update-material.component.scss']
})
export class CreateOrUpdateMaterialComponent extends ComponentBase {
	constructor(
		injector: Injector,
		private dialogConfig: DynamicDialogConfig,
		private dialogRef: DynamicDialogRef,
		private fb: FormBuilder,
		private materialService: MaterialService,
	) {
		super(injector)
	}

	postForm: FormGroup;

	materail: Material;

	ngOnInit() {
		this.materail = this.dialogConfig.data;
		this.setForm();
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.materail?.id],
			name: [this.materail?.name, required()],
			code: [this.materail?.code, required()],
			description: [this.materail?.description],
		});

		if (!this.materail?.id) {
			this.postForm.removeControl('id');
		}
	}

	onSumbit() {
		if (!this.inValidForm(this.postForm)) {
			const body: Material = this.postForm.value;
			this.isLoading = true;
			let successMessage = body.id ? "Cập nhật chất liệu thành công !" : 'Thêm chất liệu thành công !'
			if (body.id) {
				this.materialService.update(body)
					.pipe((finalize(() => this.isLoading = false)))
					.subscribe((res) => {
						if (this.checkStatusResponse(res, successMessage)) {
							this.dialogRef.close(true);
						}
					})
			}
			else {
				this.materialService.create(body)
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
