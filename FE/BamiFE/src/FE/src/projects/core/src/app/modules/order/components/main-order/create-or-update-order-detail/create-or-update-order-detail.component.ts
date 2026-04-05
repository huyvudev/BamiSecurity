import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ComponentBase } from '@shared/component-base';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { OrderDetail } from '../../../models/order.models';
import { required } from '@shared/validators/validator-common';
import { OrderDetailService } from '../../../services/order-detail.service';
import { finalize } from 'rxjs';
import { DefaultImage, EAcceptFile } from '@mylib-shared/consts/base.consts';
import { IResponseDialogUpload } from '@mylib-shared/interfaces/base.interface';
import { environment } from '@shared/environments/environment';

@Component({
	selector: 'app-create-or-update-order-detail',
	templateUrl: './create-or-update-order-detail.component.html',
	styleUrls: ['./create-or-update-order-detail.component.scss']
})
export class CreateOrUpdateOrderDetailComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private fb: FormBuilder,
		private ref: DynamicDialogRef,
		private dialogService: DialogService,
		private orderDetailService: OrderDetailService,
		private dialogRef: DynamicDialogRef,
		private configDialog: DynamicDialogConfig

	) {
		super(inj)
	}
	postForm: FormGroup;
	orderDetail: OrderDetail;
	orderId: Number;
	isCreate: boolean;
	index: number;
	imageDefault = DefaultImage.MEDIA;
	mockUpFront: string;
	baseUrl = environment.api
	mediaUrl = this.baseUrl.concat('/media')
	maxSize: number = 4194304

	ngOnInit() {
		this.orderDetail = this.configDialog?.data?.item
		this.orderId = this.configDialog?.data?.orderId
		this.index = this.configDialog?.data?.index
		this.mockUpFront = this.orderDetail?.designFront
		console.log(this.orderId)
		this.setForm();
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.orderDetail?.id],
			orderId: [this.orderId],
			type: [this.orderDetail?.type, required()],
			title: [this.orderDetail?.title, required()],
			size: [this.orderDetail?.size,required()],
			sellerSku: [this.orderDetail?.sellerSku,required()],
			color: [this.orderDetail?.color,required()],
			quantity: [this.orderDetail?.quantity, required()],
			mockUpFront: [this.orderDetail?.mockUpFront, required()],
			mockUpBack: [this.orderDetail?.mockUpBack],
			saleDesignFront: [this.orderDetail?.saleDesignFront],
			saleDesignBack: [this.orderDetail?.saleDesignBack],
			saleDesignSleeves: [this.orderDetail?.saleDesignSleeves],
			saleDesignHood: [this.orderDetail?.saleDesignHood],
		});

		if (!this.orderDetail?.id) {
			this.postForm.removeControl('id');
		}
	}

	onSubmit() {
		if (!this.inValidForm(this.postForm)) {
			if (this.orderId) {
				const body: OrderDetail = this.postForm.value;
				this.isLoading = true;
				let successMessage = body?.id ? "Cập nhật orderDetail thành công !" : "Thêm orderDetail thành công"
				if (body.id) {
					this.orderDetailService.update(body)
						.pipe(finalize(() => this.isLoading = false))
						.subscribe((response) => {
							if (this.checkStatusResponse(response, successMessage)) {
								this.dialogRef.close(response)
							}
						})
				}
				else {
					this.orderDetailService.create(body)
						.pipe(finalize(() => this.isLoading = false))
						.subscribe((response) => {
							if (this.checkStatusResponse(response, successMessage)) {
								this.dialogRef.close(response)
							}
						})
				}
			}
			else {
				const body: OrderDetail = this.postForm.value;
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

	getMainImage(controlName: string) {
		let data = this.postForm.get(controlName).value
		if (data) {
			return data
		}
	}

	selectImg(event: any, controlName?: string) {
		let res = event.originalEvent
		this.postForm.get(controlName).setValue(res.body.data[0].s3Key)

	}


	onClose() {
		this.ref.close()
	}
}
