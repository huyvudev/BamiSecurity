
import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BatchService } from '../../services/batch.service';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { StatusOrderItem } from '../../../order-item/const/status-item.const';
import { FormBuilder, FormGroup } from '@angular/forms';
import { required } from '@shared/validators/validator-common';
import { finalize } from 'rxjs';
import { BatchDetail } from '../../models/batch.model';
import { OrderItem } from '../../../order-item/models/order-item.model';
import { EPriorityBatch, StatusBatch } from '../../const/batch.const';
import moment from 'moment';
import html2canvas from 'html2canvas'; 
import jsPDF from 'jspdf';
@Component({
	selector: 'app-update-status-batch',
	templateUrl: './update-status-batch.component.html',
	styleUrls: ['./update-status-batch.component.scss']
})
export class UpdateStatusBatchComponent extends ComponentBase {
	@ViewChild('canvasContainer', { static: true }) canvasContainer!: ElementRef;
	constructor(
		inj: Injector,
		private dialogService: DialogService,
		private batchService: BatchService,
		private fb: FormBuilder,
		private breadcrumbService: BreadcrumbService,

	) {
		super(inj)
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Trạng thái" },
		]);
	}
	
	itemFilters: IItemFilter[] = [];
	statusOrderItem = StatusOrderItem.statusItems
	batch: BatchDetail
	listItem: OrderItem[] = [];
	postForm: FormGroup;
	prioriy: String;
	Prioritize = StatusBatch.Prioritize
	createDate : String;
	finishDate : String;

	ngOnInit() {
		this.setForm()
	}

	setForm() {
		this.postForm = this.fb.group({
			idBatch: ["", required()],
		});
	}

	onSubmit() {
		if (!this.inValidForm(this.postForm)) {
			let idBatch = this.extractNumber(this.postForm.get('idBatch')?.value)
			this.batchService.getById(idBatch)
				.pipe(finalize(() => this.isLoading = false))
				.subscribe((response) => {
					this.batch = response.data
					this.createDate = moment.utc(this.batch?.createdDate).local().format('DD/MM/YYYY')
					this.finishDate = moment.utc(this.batch?.finishDate).local().format('DD/MM/YYYY')
					if (this.batch.priority == EPriorityBatch.NORMAL) {
						this.prioriy = "Bình thường"
					}
					if (this.batch.priority == EPriorityBatch.HIGH) {
						this.prioriy = "Cao"
					}
					if (this.batch.priority == EPriorityBatch.HIGHEST) {
						this.prioriy = "Cao nhất"
					}
					this.listItem = this.batch.orderItems
				})
		}
	}

	extractNumber(input: string): number | null {
		const match = input.match(/^B-(\d+)$/); // Tìm số sau B-
		return match ? parseInt(match[1], 10) : null; // Trả về số nếu khớp, null nếu không
	}

	padId(id: number, numberIndex: number): string {
		return `#${id.toString().padStart(6, '0')}_${numberIndex}`;
	}

	padBatch(id: number): string {
		return `B-${id.toString()}`;
	}

	downloadCanvas(): void {
		if (!this.canvasContainer || !this.canvasContainer.nativeElement) {
			console.error('Canvas container is not defined or empty.');
			alert('Không tìm thấy nội dung để tải xuống.');
			return;
		}
	
		const container = this.canvasContainer.nativeElement;
	
		if (!container.innerHTML.trim()) {
			alert('Canvas trống. Vui lòng kiểm tra nội dung.');
			return;
		}
	
		html2canvas(container, { scale: 2 })
			.then((canvas) => {
				const imgData = canvas.toDataURL('image/png'); 
				const pdf = new jsPDF('p', 'mm', 'a4'); 
								
				const pdfWidth = pdf.internal.pageSize.getWidth();
				const pdfHeight = (canvas.height * pdfWidth) / canvas.width;
	
				pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight); 
				pdf.save('canvas-output.pdf'); 
			})
			.catch((error) => {
				console.error('Error generating canvas:', error);
				alert('Có lỗi xảy ra khi tải canvas.');
			});
	}
}
