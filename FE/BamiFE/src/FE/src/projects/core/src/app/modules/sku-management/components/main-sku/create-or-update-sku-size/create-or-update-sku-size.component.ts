import { SkuSizeService } from './../../../services/sku-size.service';
import { Component, Injector } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ComponentBase } from '@shared/component-base';
import { DialogService, DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { MockupSku, SkuSize } from '../../../model/sku-size.model';
import { DefaultImage } from '@mylib-shared/consts/base.consts';
import { environment } from '@shared/environments/environment';
import { finalize } from 'rxjs';

@Component({
	selector: 'app-create-or-update-sku-size',
	templateUrl: './create-or-update-sku-size.component.html',
	styleUrls: ['./create-or-update-sku-size.component.scss']
})
export class CreateOrUpdateSkuSizeComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private fb: FormBuilder,
		private ref: DynamicDialogRef,
		private dialogService: DialogService,
		private dialogRef: DynamicDialogRef,
		private configDialog: DynamicDialogConfig,
		private skuSizeService: SkuSizeService

	) {
		super(inj)
	}
	postForm: FormGroup;
	skuSize: SkuSize;
	idSku: number;
	index: number;
	imageDefault = DefaultImage.MEDIA;
	baseUrl = environment.api;
	mediaUrl = this.baseUrl.concat('/media')
	maxSize: number = 4194304


	ngOnInit() {
		this.skuSize = this.configDialog?.data?.item
		if (this.skuSize) {
			this.getSkuSize()
		}
		this.idSku = this.configDialog?.data?.orderId
		this.index = this.configDialog?.data?.index
		this.setForm();
	}

	getSkuSize() {
		this.skuSizeService.getById(this.skuSize?.id)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((response) => {
				if (this.checkStatusResponse(response)) {
					this.skuSize = response?.data;
					this.setForm();
				}
			})
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.skuSize?.id],
			skuId: [this.idSku],
			name: [this.skuSize?.name],
			width: [this.skuSize?.width],
			height: [this.skuSize?.height],
			length: [this.skuSize?.length],
			weight: [this.skuSize?.weight],
			additionalWeight: [this.skuSize?.additionalWeight],
			baseCost: [this.skuSize?.baseCost],
			costInMeters: [this.skuSize?.costInMeters],
			weightByVolume: [this.skuSize?.weightByVolume],
			packageDescription: [this.skuSize?.packageDescription],
			mockUpsList: this.fb.array(this.initMockups())
		});

		if (!this.skuSize?.id) {
			this.postForm.removeControl('id');
		}
		if (!this.idSku) {
			this.postForm.removeControl('skuId');
		}
	}

	private createMockupForm(mockup: MockupSku): FormGroup {
		if(!this.skuSize?.id){
			return this.fb.group({
				mockupUrl: [mockup?.mockupUrl || null],
			});
		}
		
		return this.fb.group({
			id: [mockup?.id],
			mockupUrl: [mockup?.mockupUrl || null],
			skuSizeId: [mockup?.skuSizeId || null]
		});
	}

	private initMockups(): FormGroup[] {
		return (this.skuSize?.mockUpsList || []).map(mockup => this.createMockupForm(mockup));
	}

	addMockup(): void {
		let newMockup
		if(this.skuSize?.id){
			 newMockup = this.fb.group({
				mockupUrl: [null],
				skuSizeId: [this.skuSize?.id]
			});

		}
		else{
			 newMockup = this.fb.group({
				mockupUrl: [null],
			});
		}
		this.mockups.push(newMockup);
	}

	removeMockup(index: number): void {
		this.mockups.removeAt(index);
	}

	get mockups(): FormArray {
		return this.postForm.get('mockUpsList') as FormArray;
	}

	onSubmit() {
		if (!this.inValidForm(this.postForm)) {
			if (this.idSku) {
				const body: SkuSize = this.postForm.value;
				this.isLoading = true;
				let successMessage = body?.id ? "Cập nhật sku size thành công !" : "Thêm sku size thành công"
				if (body.id) {
					this.skuSizeService.update(body)
						.pipe(finalize(() => this.isLoading = false))
						.subscribe((response) => {
							if (this.checkStatusResponse(response, successMessage)) {
								this.dialogRef.close(response)
							}
						})
				}
				else {
					this.skuSizeService.create(body)
						.pipe(finalize(() => this.isLoading = false))
						.subscribe((response) => {
							if (this.checkStatusResponse(response, successMessage)) {
								this.dialogRef.close(response)
							}
						})
				}
			}
			else {
				const body: SkuSize = this.postForm.value;
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

	getMainImage(controlName: string, index: number) {
		const control = this.mockups.at(index).get(controlName).value;
		// console.log(control)
		if (control) {
			return control;
		}
		return null;
	}

	selectImg(event: any, index: number): void {
		const res = event.originalEvent?.body?.data[0].s3Key;
		console.log(res)
		this.mockups.at(index).get('mockupUrl')?.setValue(res);
	}


	onClose() {
		this.ref.close()
	}

}
