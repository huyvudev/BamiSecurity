import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { RequestModel } from '../../../shared/models/base.model';

@Component({
	selector: 'app-form-request',
	templateUrl: './form-request.component.html',
	styleUrls: ['./form-request.component.scss'],
})
export class FormRequestComponent implements OnInit {
	
	constructor(
		public ref: DynamicDialogRef,
		public configDialog: DynamicDialogConfig,
		private fb: FormBuilder,

	) { }

	postForm: FormGroup;
	formRequest = new RequestModel();

	ngOnInit(): void {
		this.formRequest = this.configDialog.data;
		this.postForm = this.fb.group({
			requestNote: [null],
		})
	}

	close() {
		this.ref.close();
	}

	onSubmit() {
		this.ref.close(this.postForm.value);
	}
}
