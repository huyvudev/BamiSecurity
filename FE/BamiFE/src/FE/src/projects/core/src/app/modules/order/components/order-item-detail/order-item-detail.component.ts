import { OrderItemService } from './../../../order-item/services/order-item.service';
import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentBase } from '@shared/component-base';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { finalize } from 'rxjs';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { OrderItem } from '../../../order-item/models/order-item.model';
import { Page } from '@mylib-shared/models/page';

@Component({
	selector: 'app-order-item-detail',
	templateUrl: './order-item-detail.component.html',
	styleUrls: ['./order-item-detail.component.scss']
})
export class OrderItemDetailComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private routeActive: ActivatedRoute,
		private router: Router,
		private fb: FormBuilder,
		private orderItemService: OrderItemService,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Item" },
		]);
	}

	postForm: FormGroup;
	item: OrderItem;

	idItem: any;
	hasData: boolean = true;

	itemFilters: IItemFilter[] = [];
	template: string;

	ngOnInit() {
		if (this.routeActive.snapshot.paramMap.get('id')) {
			this.idItem = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'))
			if (this.idItem) {
				this.getDetail(this.idItem);
			}
		}
		this.setForm();
		this.genItemFilters();
	}

	setForm() {
		console.log('set')
		this.postForm = this.fb.group({
			itemIndex: [this.item?.itemIndex || 0],
			status: [this.item?.status || ''],
			size: [this.item?.size || ''],
			note: [this.item?.note || ''],
			orderId: [this.item?.orderId || null],
			namespace: [this.item?.namespace || ''],
			width: [this.item?.width || 0],
			length: [this.item?.length || 0],
			code: [this.item?.code || ''],
			errorMessage: [this.item?.errorMessage || ''],
			mockUpFront: [this.item?.mockUpFront || ''],
			mockUpBack: [this.item?.mockUpBack || ''],
			designFront: [this.item?.designFront || ''],
			designBack: [this.item?.designBack || ''],
			designSleeves: [this.item?.designSleeves || ''],
			designHood: [this.item?.designHood || ''],
			template: [this.template]
		});

		this.postForm.disable()

	}

	getItem(id?: any) {
		this.isLoading = true;
		this.hasData = true;
		let data = this.unpadId(id)
		console.log(data)
		this.orderItemService.getByTemplate(this.page, data)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((response) => {
				if (this.checkStatusResponse(response)) {
					this.item = response.data
					this.template = this.padId(this.item?.orderId, this.item?.itemIndex)
					this.setForm();
				}
				else {
					this.hasData = false
				}
			})
	}

	getDetail(id?: any) {
		this.isLoading = true;
		this.hasData = true;
		this.orderItemService.getById(id)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((response) => {
				if (this.checkStatusResponse(response)) {
					this.item = response.data
					console.log(this.item)
					this.template = this.padId(this.item?.orderId, this.item?.itemIndex)
					this.setForm();
				}
				else {
					this.hasData = false
				}
			})
	}

	padId(id: number, numberIndex: number): string {
		return `#${id.toString().padStart(6, '0')}_${numberIndex}`;
	}

	unpadId(paddedId: string): { orderId: number; itemIndex: number } {
		if (!paddedId) {
			return { orderId: 0, itemIndex: 0 };
		}
	
		if (paddedId.startsWith('#')) {
			paddedId = paddedId.slice(1);
		}
	
		const separatorIndex = paddedId.indexOf('_');
		if (separatorIndex === -1) {
			throw new Error('Invalid paddedId format');
		}

		const orderIdPart = paddedId.slice(0, separatorIndex);
		const itemIndexPart = paddedId.slice(separatorIndex + 1);
		const orderId = parseInt(orderIdPart, 10) || 0; 
		const itemIndex = parseInt(itemIndexPart, 10) || 0; 
		return { orderId, itemIndex };
	}

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'id',
				placeholder: 'Tìm kiếm',
			},
		]
	}

	back() {
		this.router.navigate([`home`]);
	}

	getMainImage(controlName: string) {
		let data = this.postForm.get(controlName).value
		if (data) {
			return data;
		}
	}

	selectImg(event: any, controlName?: string) {
		let res = event.originalEvent
		this.postForm.get(controlName).setValue(res.body.data[0].s3Key)
	}
	onSubmit() {

	}

}
