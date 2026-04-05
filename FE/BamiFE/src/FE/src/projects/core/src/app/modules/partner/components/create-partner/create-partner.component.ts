import { PartnerUpdate } from './../../model/partner.model';
import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ComponentBase } from '@shared/component-base';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PartnerTypeService } from '../../services/partner-type.service';
import { PartnerService } from '../../services/partner.service';
import { Partner, PartnerType } from '../../model/partner.model';
import { Page } from '@mylib-shared/models/page';
import { finalize } from 'rxjs';
import { required } from '@shared/validators/validator-common';
import { BaseConsts } from 'projects/my-lib/src/lib/shared/consts/base.consts';
@Component({
	selector: 'app-create-partner',
	templateUrl: './create-partner.component.html',
	styleUrls: ['./create-partner.component.scss']
})
export class CreatePartnerComponent extends ComponentBase implements OnInit {
	constructor(
		injector: Injector,
		private dialogConfig: DynamicDialogConfig,
		private dialogRef: DynamicDialogRef,
		private fb: FormBuilder,
		private partnerTypeService: PartnerTypeService,
		private partnerService: PartnerService,
	) {
		super(injector)
	}

	partnerTypes: PartnerType[] = [];
	pagePartnerType: Page = new Page()
	partner: Partner
	postForm: FormGroup;

	ngOnInit(): void {
		this.partner = this.dialogConfig.data;
		this.getPartnerType();
		this.setForm();
	}

	getPartnerType() {
		this.isLoading = true;
		this.pagePartnerType.pageSize = -1
		this.pagePartnerType.pageNumber = 1
		this.partnerTypeService.getAll(this.pagePartnerType)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.partnerTypes = res?.data.items
			})
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.partner?.id],
			name: [this.partner?.name, required()],
			partnerTypeId: [this.partner?.partnerTypeId]
		});

		if (!this.partner?.id) {
			this.postForm.removeControl('id');
		}
	}

	onSumbit() {
		if (!this.inValidForm(this.postForm)) {
			const body: PartnerUpdate = this.postForm.value;
			let successMessage = body.id ? "Cập nhật đối tác thành công !" : 'Thêm đối tác thành công !'
			if (body.id) {

				this.partnerService.update(body).subscribe((res) => {
					if (this.checkStatusResponse(res, successMessage)) {
						this.dialogRef.close(true);
					}

				})
			}
			else {
				this.partnerService.create(body).subscribe((res) => {
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
