import { SkuService } from './../../../../sku-management/services/sku.service';
import { BrandService } from './../../../../name-space/services/brand.service';
import { OrderDetailService } from './../../../services/order-detail.service';
import { Component, Injector } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentBase } from '@shared/component-base';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { OrderDetail } from '../../../models/order.models';
import { required } from '@shared/validators/validator-common';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { finalize } from 'rxjs';
import { Page } from '@mylib-shared/models/page';
import { Brand } from '../../../../name-space/models/brand.model';
import { Sku } from '../../../../sku-management/model/sku.model';
import { environment } from '@shared/environments/environment.dev';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { EImageTable } from '@mylib-shared/consts/lib-table.consts';

@Component({
	selector: 'app-update-order-detail',
	templateUrl: './update-order-detail.component.html',
	styleUrls: ['./update-order-detail.component.scss']
})

export class UpdateOrderDetailComponent extends ComponentBase {
	constructor(
		inj: Injector,
		private router: Router,
		private fb: FormBuilder,
		private dialogService: DialogService,
		private routeActive: ActivatedRoute,
		private orderDetailService: OrderDetailService,
		private brandService: BrandService,
		private skuService: SkuService,
		private breadcrumbService: BreadcrumbService,
	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Quản lý order" },
			{ label: "Cập nhật Order detail" }
		]);
	}

	postForm: FormGroup;
	orderDetail: OrderDetail;

	idDetailOrder: any;

	isEdit: boolean = false;
	hasData: boolean = true;
	isCreate: boolean = false;

	pageSearch: Page = new Page();
	listBrands: Brand[] = [];
	listSkus: Sku[] = [];

	imageDefault = this.DefaultImage.MEDIA;
	mockUpFront: string;
	baseUrl = environment.api
	mediaUrl = this.baseUrl.concat('/media')
	maxSize: number = 4194304
	isDisableUpload: boolean = true;

	itemFilters: IItemFilter[] = [];

	EImageTable = EImageTable

	ngOnInit() {
		this.pageSearch.pageSize = -1
		this.pageSearch.pageNumber = 1
		this.getAllBrand();
		this.getAllSku();
		if (this.routeActive.snapshot.paramMap.get('id')) {
			this.idDetailOrder = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'))
			console.log(this.idDetailOrder)
			if (this.idDetailOrder) {
				this.getDetail(this.idDetailOrder);
			}
		}
		this.setForm();
		this.genItemFilters();
	}

	setForm() {
		this.postForm = this.fb.group({
			id: [this.orderDetail?.id],
			type: [this.orderDetail?.type],
			title: [this.orderDetail?.title],
			size: [this.orderDetail?.size],
			sellerSku: [this.orderDetail?.sellerSku],
			color: [this.orderDetail?.color],
			quantity: [this.orderDetail?.quantity, required()],
			skuId: [this.orderDetail?.skuId, required()],
			mockUpFront: [this.orderDetail?.mockUpFront],
			width: [this.orderDetail?.width, required()],
			length: [this.orderDetail?.length, required()],
			mockUpBack: [this.orderDetail?.mockUpBack],
			designFront: [this.orderDetail?.designFront],
			designBack: [this.orderDetail?.designBack],
			designSleeves: [this.orderDetail?.designSleeves],
			designHood: [this.orderDetail?.designHood],
			saleDesignFront: [this.orderDetail?.saleDesignFront],
			saleDesignBack: [this.orderDetail?.saleDesignBack],
			saleDesignSleeves: [this.orderDetail?.saleDesignSleeves],
			saleDesignHood: [this.orderDetail?.saleDesignHood],
		});
		this.postForm.disable()
	}

	getDetail(id?: any) {
		this.isLoading = true;
		this.hasData = true;
		this.isEdit = false;
		let idOrderDetail = this.unpadId(id)

		this.orderDetailService.getById(idOrderDetail)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((response) => {
				if (this.checkStatusResponse(response)) {
					this.orderDetail = response.data
					console.log(this.orderDetail)
					this.setForm();
				}
				else {
					this.hasData = false
				}
			})

	}

	handEnalbelField() {
		this.isDisableUpload = false;
		this.postForm.get('width').enable();
		this.postForm.get('length').enable();
		this.postForm.get('skuId').enable();
	}

	onSubmit() {
		if (!this.inValidForm(this.postForm)) {
			const body: OrderDetail = this.postForm.getRawValue();
			console.log(body)
			this.isLoading = true;
			let successMessage = "Cập nhật orderDetail thành công !"
			this.orderDetailService.updatBase(body)
				.pipe((finalize(() => this.isLoading = false)))
				.subscribe((res) => {
					if (this.checkStatusResponse(res, successMessage)) {
						this.cancel()
						this.router.navigate([`/order-management/detail-order/detail/${this.cryptEncode(res?.data?.id)}`])
					}
				})
		}
	}

	activeEdit() {
		this.isEdit = true;
		this.handEnalbelField();
	}

	back() {
		this.router.navigate([`order-management/detail-order`]);
	}

	cancel() {
		this.isEdit = false;
		this.postForm.disable()
		this.isDisableUpload = true;
	}

	getAllBrand() {
		this.brandService.getAll(this.pageSearch)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((res) => {
				this.listBrands = res?.data.items;
			})
	}

	getAllSku() {
		this.skuService.getAll(this.pageSearch)
			.pipe(finalize(() => this.isLoading = false))
			.subscribe((res) => {
				this.listSkus = res?.data.items;
			})
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

	genItemFilters() {
		this.itemFilters = [
			{
				type: ETableTopBar.INPUT_TEXT,
				variableReference: 'id',
				placeholder: 'Tìm kiếm',
			},
		]
	}

	unpadId(paddedId: string): number {
		if (paddedId != null) {
			const cleanedId = paddedId.startsWith('#') ? paddedId.slice(1) : paddedId;
			return parseInt(cleanedId, 10);
		}
		else {
			return 0
		}
	}

}