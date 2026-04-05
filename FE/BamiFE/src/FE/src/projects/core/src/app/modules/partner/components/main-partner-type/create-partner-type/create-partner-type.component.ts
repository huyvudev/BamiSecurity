import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PartnerTypeService } from '../../../services/partner-type.service'
import { PartnerType, PartnerTypeUpdate } from '../../../model/partner.model';
import { required } from '@shared/validators/validator-common';
import { ComponentBase } from '@shared/component-base';

@Component({
    selector: 'app-create-partner-type',
    templateUrl: './create-partner-type.component.html',
    styleUrls: ['./create-partner-type.component.scss']
})
export class CreatePartnerTypeComponent extends ComponentBase implements OnInit {
    constructor(
        injector: Injector,
        private dialogConfig: DynamicDialogConfig,
        private dialogRef: DynamicDialogRef,
        private fb: FormBuilder,
        private partnerTypeService: PartnerTypeService,
    ) {
        super(injector)
    }

    partnerType: PartnerType
    postForm: FormGroup;

    ngOnInit(): void {
		this.partnerType = this.dialogConfig.data;
		this.setForm();
	}

    setForm() {
		this.postForm = this.fb.group({
			id: [this.partnerType?.id],
			name: [this.partnerType?.name, required()],
			
		});

		if (!this.partnerType?.id) {
			this.postForm.removeControl('id');
		}
	}

    onSumbit() {
		if (!this.inValidForm(this.postForm)) {
			const body: PartnerTypeUpdate = this.postForm.value;
			let successMessage = body.id ? "Cập nhật loại đối tác thành công !" : 'Thêm loại đối tác thành công !'
			if (body.id) {

				this.partnerTypeService.update(body).subscribe((res) => {
					if (this.checkStatusResponse(res, successMessage)) {
						this.dialogRef.close(true);
					}

				})
			}
			else {
				this.partnerTypeService.create(body).subscribe((res) => {
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

