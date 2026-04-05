import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ComponentBase } from '@shared/component-base';
import { required } from '@shared/validators/validator-common';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { EPriorityBatch, StatusBatch } from '../../const/batch.const';
import { Partner } from '../../../partner/model/partner.model';
import { PartnerService } from '../../../partner/services/partner.service';
import { Page } from '@mylib-shared/models/page';
import { finalize } from 'rxjs';
import { BatchService } from '../../services/batch.service';
import { BatchCreate, Item } from '../../models/batch-create.model';

@Component({
	selector: 'app-create-batch',
	templateUrl: './create-batch.component.html',
	styleUrls: ['./create-batch.component.scss']
})
export class CreateBatchComponent extends ComponentBase {
	constructor(
		injector: Injector,
		private dialogConfig: DynamicDialogConfig,
		private dialogRef: DynamicDialogRef,
		private partnerService: PartnerService,
		private batchService: BatchService,
		private fb: FormBuilder,
	) {
		super(injector)
	}

	postForm: FormGroup;
	listPartner: Partner[] = [];
	listItems: Item[] = [];

	pageCustom: Page = new Page();

	typeBatch = StatusBatch.TypeBatch;
	listPriority = StatusBatch.Prioritize
	batchType: number = 1;
	dataFilter: any;

	ngOnInit() {
		this.pageCustom.pageSize = -1
		this.pageCustom.pageNumber = 1
		this.getPartner();
		this.setForm();
	}

	setForm() {
		this.postForm = this.fb.group({
			batchs: [''],
			batchType: [this.batchType],
			numberBatch: [],
			creatorName: ["", required()],
			partnerId: [required()],
			priority: [EPriorityBatch.NORMAL],
			items: [],
		});

		this.postForm.get('batchType').valueChanges.subscribe(value => {
			this.batchType = value
		});
	}

	getPartner() {
		this.isLoading = true;
		this.partnerService.getAll(this.pageCustom)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				this.listPartner = res?.data.items
			})
	}

	onSumbit() {
		if (!this.inValidForm(this.postForm)) {
			let data = this.postForm.value;
			console.log(data)
			this.listItems = this.getOrderItem(data?.items)
			const body: BatchCreate = this.postForm.value
			data.items = this.listItems
			this.batchService.create(body)
				.pipe((finalize(() => this.isLoading = false)))
				.subscribe((res) => {
					if (this.checkStatusResponse(res, "Tạo Lô thành công")) {
						this.dialogRef.close();
					}
				})
		}
	}

	close() {
		this.dialogRef.close();
	}

	getOrderItem(input: string): Item[] {
		const items = input.split(/[\n,]/).map(item => item.trim());
		return items.map(item => {
			const cleanedItem = item.replace(/^#0*/, '');
			const [orderId, itemIndex] = cleanedItem.split('_').map(Number);
			return new Item(orderId, itemIndex);
		});
	}

	getItemInBatch() {
		let batchIds = this.extractNumbers(this.postForm.get('batchs').value);
		console.log(batchIds)
		this.batchService.getItemInBatch(batchIds)
			.pipe((finalize(() => this.isLoading = false)))
			.subscribe((res) => {
				console.log(res)
				const data = res.data.map(item => `#${item.orderId.toString().padStart(6, '0')}_${item.itemIndex}`).join('\n');
				this.postForm.get('items').setValue(data)
			})
	}

	extractNumbers(input: string): number[] {
		const items = input.split(/[\n,]/).map(item => item.trim());
		return items.map(item => parseInt(item.replace(/^B-/, ''), 10));
	}

}
