import { SkuBase } from './../../model/skubase.models';
import { IAction, IColumn } from '@mylib-shared/interfaces/lib-table.interface';
import { SkuBaseService } from './../../services/sku-base.service';
import { Component, Injector } from '@angular/core';
import { ComponentBase } from '@shared/component-base';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'projects/my-lib/src/public-api';
import { ETableTopBar, IItemFilter } from '@mylib-shared/consts/filter-bar.const';
import { Page } from '@mylib-shared/models/page';
import { finalize } from 'rxjs';
import { ETableColumnType, ETableFrozen } from '@mylib-shared/consts/lib-table.consts';
import { CreateOrUpdateSkuBaseComponent } from './create-or-update-sku-base/create-or-update-sku-base.component';

@Component({
    selector: 'app-main-sku-base',
    templateUrl: './main-sku-base.component.html',
    styleUrls: ['./main-sku-base.component.scss']
})
export class MainSkuBaseComponent extends ComponentBase {
    constructor(
        inj: Injector,
        private dialogService: DialogService,
        private skuBaseService: SkuBaseService,
        private breadcrumbService: BreadcrumbService,

    ) {
        super(inj)
        this.breadcrumbService.setItems([
            { label: "Trang chủ", routerLink: ['/home'] },
            { label: "Quản lý Sku base" },
            { label: "SkuBase" }
        ]);
    }

    columns: IColumn[] = []
    itemFilters: IItemFilter[] = [];
    listAction: IAction[][] = [];
    rows: SkuBase[] = [];
    dataFilter: any

    ngOnInit() {
        this.setColumn()
        this.genListAction(this.rows);
        this.genItemFilters();
        this.setPage();
    }

    public setPage(event?: Page) {
        this.dataFilter = { ...this.dataFilter, ...event }
        this.isLoading = true;
        this.skuBaseService.getAll(this.page, this.dataFilter)
            .pipe((finalize(() => this.isLoading = false)))
            .subscribe((res) => {
                this.rows = this.page.getRowLoadMore(this.rows, res?.data?.items);
                this.page.totalItems = res?.data?.totalItems;
                if (this.rows?.length) {
                    this.genListAction(this.rows);
                }
            }
            );
    }

    setColumn() {
        this.columns = [
            {
                field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: ETableFrozen.LEFT,
            },
            {
                field: 'code', header: "Code", width: 15, type: ETableColumnType.TEXT,
            },
            {
                field: 'description', header: "Mô tả", width: 15,

            },
            {
                field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: ETableFrozen.RIGHT, type: ETableColumnType.ACTION_DROPDOWN,

            }
        ]
    }

    genListAction(data: any) {
        this.listAction = data.map(item => {
            const actions = [];

            actions.push({
                label: 'Cập nhật SkuBase',
                icon: 'pi pi-exclamation-circle',
                command: () => {
                    this.createOrUpdateSkuBase(item)
                }
            });

            actions.push({
                label: 'Xóa',
                icon: 'pi pi-trash',
                command: () => {
                    this.onDelete(item)
                }
            });

            return actions;
        });
    }

    genItemFilters() {
        this.itemFilters = [
            {
                type: ETableTopBar.INPUT_TEXT,
                variableReference: 'keyword',
                placeholder: 'Tìm kiếm',
            },

        ]
    }

    createOrUpdateSkuBase(skuBase?: SkuBase) {
        this.dialogService.open(CreateOrUpdateSkuBaseComponent, {
            header: skuBase ? "Cập nhật Sku Base" : "Tạo Sku Base",
            styleClass: `p-dialog-custom customModal`,
            width: '586px',
            data: skuBase
        }).onClose.subscribe(result => {
            if (result) {

                this.setPage();
            }
        })
    }

    async onDelete(skuBase?: SkuBase) {
        const accept = await this.confirmAction('Bạn có chắc chắn muốn xóa?')
        if (accept) {

            this.isLoading = true;
            this.skuBaseService.delete(skuBase.id)
                .pipe((finalize(() => this.isLoading = false)))
                .subscribe((res) => {
                    if (this.checkStatusResponse(res, "Xóa thành công")) {
                        this.setPage();
                    }
                })
        }
    }

}

