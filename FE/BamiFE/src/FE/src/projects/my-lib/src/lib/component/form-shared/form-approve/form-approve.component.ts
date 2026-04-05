import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { ApproveModel } from '../../../shared/models/base.model';

@Component({
	selector: 'app-form-approve',
	templateUrl: './form-approve.component.html',
	styleUrls: ['./form-approve.component.scss'],
})
export class FormApproveComponent{
	constructor(
		public ref: DynamicDialogRef,
		private fb: FormBuilder,
	) {
	 
	}

	formApprove = new ApproveModel();
	postForm: FormGroup;

	ngOnInit(): void {
		this.postForm = this.fb.group({
			note: ['', Validators.required],
			approve: [true, []]
		});
	}

	accept() {
		if (this.postForm.get('note')?.value) {
			const formValue = this.postForm.value;
			let body: ApproveModel = {
				approve: formValue?.approve,
				approveNote: formValue?.approve ? this.postForm.get('note')?.value : '',
				cancelNote: !formValue?.approve ? this.postForm.get('note')?.value : ''
			};
			//
			this.ref.close(body);
		} else {
			this.postForm.get('note').markAsDirty();
		}
	}
}
