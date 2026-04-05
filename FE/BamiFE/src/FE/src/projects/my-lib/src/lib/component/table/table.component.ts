import { animate, style, transition, trigger } from '@angular/animations';
import { afterRender, booleanAttribute, ChangeDetectorRef, Component, ElementRef, EventEmitter, Inject, Input, OnDestroy, OnInit, Output, Renderer2, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { Paginator } from 'primeng/paginator';
import { finalize, Subject } from 'rxjs';
import { SettingDisplayColumnTableComponent } from './setting-display-column-table/setting-display-column-table.component';
import { IAction, IColumn } from '../../shared/interfaces/lib-table.interface';
import { Page } from '../../shared/models/page';
import { BaseConsts, ContentTypeEView, DefaultImage, EFormatDate, EFormatDateDisplay, EYesNo } from '../../shared/consts/base.consts';
import { EImageTable, ETableColumnType, ETableFrozen, ValueType } from '../../shared/consts/lib-table.consts';
import { Utils } from '../../shared/utils';
import { NgxSpinnerService } from 'ngx-spinner';
import { LibHelperService } from '../../shared/services/lib-helper.service';
import moment from 'moment';
import { IEnvironment } from '../../shared/interfaces/environment.interface';
import { Image } from 'primeng/image';
import { OverlayPanel } from 'primeng/overlaypanel';
import { BatchService } from 'projects/core/src/app/modules/batch/services/batch.service';
import { OrderItemService } from 'projects/core/src/app/modules/order-item/services/order-item.service';

@Component({
    selector: 'lib-table',
    templateUrl: './table.component.html',
    animations: [
        trigger('animationRefresh', [
            // transition(':enter', [
            //   style({ opacity: 1 }),
            //   animate('300ms', style({ opacity: 1 })),
            // ]),
            transition(':leave', [
                style({ height: 0, opacity: 1 }),
                animate('100ms', style({ opacity: 1 })),
            ])
        ])],
    providers: [NgxSpinnerService]
})

export class TableComponent implements OnInit, OnDestroy {

    constructor(
        private dialogService: DialogService,
        private ref: ChangeDetectorRef,
        private spinner: NgxSpinnerService,
        private batchService: BatchService,
        private orderItemService: OrderItemService,
        private _libHelperService: LibHelperService,
        @Inject('env') public environment: IEnvironment
    ) {
    }


    @Input() rows: any[] = [];
    @Input() class: string;
    @Input() id: string;
    @Input() styleClassPaginator: string = "show-option-all";
    @Input() isLoading: boolean = false;
    @Input() rowHover: boolean = true;
    @Input() responsiveLayout: string = 'scroll';
    @Input() dataKey: string = 'id';
    @Input() reorderableColumns: boolean = false;
    @Input() showCurrentPageReport: boolean = true;
    @Input() paginator: boolean = false;
    @Input() scrollable: boolean = true;
    @Input() additionalHeight: number = 0;
    @Input() columnCacheName: string;
    @Input() columns: IColumn[] = [];
    @Input() listAction: IAction[][] = [];
    @Input() resetBorder: boolean = false;
    @Input() emptyDataMessage: string;

    @Input() onChangeAtributionData: boolean = true;
    @Input() fieldContentHtmls: string[] = [];


    @Input() perPageOptions: number[];
    @Input() page: Page = new Page('lib-table');
    @Output() pageChange = new EventEmitter<Page>();

    @Input() selectedItems: any[] = [];
    @Output() selectedItemsChange = new EventEmitter<any>();
    @Output() _onChangeCheckbox = new EventEmitter<any>();
    @Output() _onChangeCheckboxAll = new EventEmitter<any>();
    @Output() onRowReorder = new EventEmitter<any>();

    @Input() isShowPaginator: boolean = true;
    @Input() isShowIconFilter: boolean = true;
    @Input() isShowFilter: boolean = true;
    @Output() isShowFilterChange = new EventEmitter<boolean>();

    @Output() _onPageChange = new EventEmitter<any>();
    @Output() _onSort = new EventEmitter<any>();
    @Output() selectOption = new EventEmitter<any>();

    @Input() tabContentHeight: number = 0;
    @Input() idHeader: string;
    @Input() scrollHeightConst: number = 0;
    @Input() isHeightAuto: boolean = false;
    @Input() showTable: boolean = true;
    @Input({ transform: booleanAttribute }) tableInTab: boolean = false;
    @Input() isDisabledActionDropdown = true;
    @Input() disabled: boolean = false;

    @Input() updateSetHeight: boolean = false;
    @Input() selectedKey: string;

    columnShows: IColumn[] = [];

    permissibleErrorScrollHeight: number = 10; // Sai số cho phép
    divFilterHeight: number = 0;
    scrollHeight: number = 0;
    tableHeight: number;
    @Input() heightConst: number;
    isRowOdds: boolean = false;
    isShowTable: boolean = false;
    onResize: boolean = false;
    isShowLibLoading: boolean;
    api: string;

    iconResizeColumn = 'share/assets/layout/images/icon-resize-column.png';

    YesNo = EYesNo;

    ETableColumnType = ETableColumnType;
    ETableFrozen = ETableFrozen;
    EFormatDate = EFormatDate;
    EImageTable = EImageTable
    DefaultImage = DefaultImage;

    @ViewChild('eTable') eTable: ElementRef<HTMLElement>;
    @ViewChild('wrapperPaginator') wrapperPaginator: ElementRef<HTMLElement>;
    @ViewChild('paginatorEl', { static: false }) paginatorEl: Paginator;
    keywordSubject = new Subject<string>();

    sortData: any[] = [];

    columnOriginals: IColumn[] = [];
    setHeightComplete: boolean = false;
    BaseConsts = BaseConsts;
    @ViewChild('checkbox') checkbox: ElementRef;

    optionPage = [
        { name: 'Tất cả', code: true },
        { name: '1 trang', code: false },
    ];

    variableNameRerenders = {
        isShowTable: 'isShowTable',
        isShowPaginator: 'isShowPaginator'
    }

    typeDates = [];
    Utils = Utils;

    rowItemClone: any;
    showImageEmpty: boolean = false;
    isInit: boolean = true;


    mockupFrontEdit: string;
    toolTipCopy: string;
    isHorverToolTip: boolean;

    noteBatch: string;
    noteItem: string;
    ngOnInit(): void {
        this.api = `${this.environment.api}/media/`;
        if (!this.id) this.id = 'wrapper-table-' + new Date().getTime();
        //
        for (const [key, value] of Object.entries(EFormatDateDisplay)) {
            this.typeDates.push(value)
        }
    }

    extractTextFromHTML(htmlString: string): string {
        const tempElement = document.createElement('div');
        tempElement.innerHTML = htmlString;
        return tempElement.innerText || tempElement.textContent || '';
    }

    selectedOption = [];
    onChange(event) {
        if (event) {
            this.selectOption.emit(event)
            setTimeout(() => {
                this.isShowOptionPagePopup = false;
                this.selectedOption = [];
            }, 300);
        }
    }
    isShowOptionPagePopup: boolean = false;

    onCheckboxClick(event) {
        this.isShowOptionPagePopup = event?.checked;
    }

    countDifferenceHeight() {
        // if(this.updateSetHeight) {
        // 	this.setHeightComplete = true;
        // } else {
        // 	setTimeout(() => {
        // 		const paginatorHeight = 61;
        // 		const theadHeight = 40;
        // 		this.ref.detectChanges();
        // 		if(!this.heightConst) {
        // 		  const differenceCurrentHeight = Utils.countDifferencePageHeight();
        // 		  this.tableHeight = (this.tableHeight || 0) + differenceCurrentHeight + theadHeight;
        // 		} else {
        // 			  if(this.isShowPaginator) this.heightConst = this.heightConst;
        // 			this.tableHeight = this.heightConst;
        // 			// console.log('tableHeightConst', this.tableHeight, this.heightConst);
        // 		}
        // 		this.tableHeight = (this.tableHeight > 100 ? (this.tableHeight - (this.isShowPaginator ? paginatorHeight : 0)) : 500);
        // 		this.setHeightComplete = true;
        // 		// console.log('tableHeight', this.tableHeight);
        // 		//
        // 	}, 0);
        // }

    }

    checkValidColumnCaches(columnCaches): IColumn[] {
        if (columnCaches) {
            if (columnCaches.length !== this.columns.length) {
                localStorage.removeItem(this.columnCacheName);
                return [];
            } else {
                let isValid = true;
                for (let i = 0; i < columnCaches.length; i++) {
                    if (columnCaches[i].field !== this.columns[i].field) {
                        isValid = false;
                        localStorage.removeItem(this.columnCacheName);
                        break;
                    }
                }
                if (isValid) {
                    return columnCaches;
                }
                return [];
            }
        }
        return [];
    }

    ngOnChanges(changes: SimpleChanges) {
        // console.log('change', changes);
        if (changes?.['columns']) {
            this.columnOriginals = [...changes?.['columns'].currentValue];
            this.setAtributionColumn(changes?.['columns'].currentValue);
            let columnCaches = this.checkValidColumnCaches(JSON.parse(Utils.getLocalStorage(this.columnCacheName)));
            if (columnCaches?.length) {
                let columnCacheNews = this.columns.map((column, index) => {
                    return {
                        ...column,
                        isShow: !column.hasUpdateIsShow ? columnCaches[index].isShow : column.isShow,
                    }
                });
                this.setLocalStorageColumns(columnCacheNews);
                this.columns = columnCacheNews;
            }
            this.setDistanceFrozenColumn();
        }

        if (changes?.['rows']) {
            // UPDATE COLOR BACKGROUND ROW THEO SỐ DÒNG CHẴN LẺ (UI)

            if (this.rows?.length > 200) {
                Utils.log('Số lượng phần tử quá lớn', this.rows.length)
                this.rows = this.rows.slice(0, 200);
                console.log(this.rows.length);
            }

            if (this.rows?.length && this.rows?.length < this.page.perPageOptions[0]) {
                this.isRowOdds = !(this.rows?.length % 2 === 0);
            }

            // FIX DISPLAY RESET PAGINATOR VỀ TRANG ĐẦU KHI RELOAD DATA
            if (this.paginatorEl && this.page.pageNumber === 0) {
                this.reRenderHtml(this.variableNameRerenders.isShowPaginator);
            }

            if (changes?.['rows']?.currentValue?.length <= changes?.['rows']?.previousValue?.length) {
                const rowCurrentValue = changes?.['rows']?.currentValue?.map(item => item?.[this.dataKey]);
                const rowPreviousValue = changes?.['rows']?.previousValue?.map(item => item?.[this.dataKey]);
                if (!Utils.compareData(rowCurrentValue, rowPreviousValue)) {
                    this.resetPositionScrollBar();
                }
            }

            if (changes?.['rows']?.currentValue?.length) {
                // Tính value
                this.rowItemClone = Utils.cloneData(this.rows)[0];
                this.rows = Utils.cloneData(this.rows).map((row) => {
                    if (row) {
                        for (let col of this.columns) {
                            let value: any;
                            value = this.getValue(row, col, col.valueType);
                            if (this.typeDates.includes(col?.type)) {
                                value = (moment(value).isValid() && value) ? moment(value).format(col?.type) : ''
                            }
                            //
                            if (col?.type === ETableColumnType.STATUS) {
                                value = this.columnOriginals[col.position].getTagInfo(value);
                            }
                            if (col?.type === ETableColumnType.TEMPLATE) {
                                value.listStatus = this.columnOriginals[col.position].getTagInfo(value);
                            }
                            if (col?.type === ETableColumnType.LIST_STATUS) {
                                value.listStatus = this.columnOriginals[col.position].getTagInfo(value);
                            }
                            if (col?.type === ETableColumnType.CHECKBOX_CHANGE) {
                                const valueTemp = value
                                if (col?.valueType === ValueType.ROW) {
                                    value = {
                                        value: valueTemp,
                                        isDisableCheckbox: this.columnOriginals[col.position]?.getDisabled(row?.status)?.isDisableCheckbox,
                                    }
                                } else {
                                    value = {
                                        value: valueTemp,
                                        isDisableCheckbox: false,
                                    }
                                }
                            }

                            if (col?.type === ETableColumnType.ACTION_BUTTON) {
                                const valueTemp = value
                                if (col?.valueType === ValueType.ROW) {
                                    value = {
                                        value: valueTemp,
                                        isDisableButton: this.columnOriginals[col.position]?.disableButtonAction && this.columnOriginals[col.position]?.disableButtonAction(row),
                                    }
                                } else {
                                    value = {
                                        value: valueTemp,
                                        isDisableButton: false,
                                    }
                                }
                            }
                            //
                            if (row[col.field] !== value) {
                                row[col.field + '_customValue'] = value;
                            }
                        }
                        return row;
                    }
                    return {};
                })
                //
            }
            //
            if (!changes['rows'].firstChange) {
                setTimeout(() => {
                    this.showImageEmpty = true;
                }, 100);
            }

        }


        if (changes?.['isLoading']) {
            if (changes?.['isLoading']?.currentValue) {
                this.spinner.show();
                this.showImageEmpty = false;
            } else {
                this.spinner.hide();
            }
        }

        if (changes?.['showTable']) {
            if (changes?.['showTable']?.currentValue) {
                this.countDifferenceHeight();
            }
        }

        if (changes?.['heightConst']) {
            if (changes?.['heightConst']?.currentValue) {
                this.countDifferenceHeight();
            }
        }
    }

    resetPositionScrollBar() {
        const wrapperTableEl = document.getElementById(this.id);
        if (wrapperTableEl) wrapperTableEl.scrollTo(0, 0);
    }

    ngAfterViewInit() {
        this.countDifferenceHeight();
    }

    setAtributionColumn(columns) {
        this.columns = columns.map((col: IColumn, index) => {
            return {
                ...col,
                position: index,
                isShow: !(col?.isShow === false),
                hasUpdateIsShow: col?.isShow !== undefined,
                isSort: !!col?.isSort,
                type: col?.type || ETableColumnType.TEXT,
                cutText: col?.isCutText ? `cut-text-${col?.width}` : '',
                displaySettingColumn: !(col?.displaySettingColumn === false),
                isPermision: !(col?.isPermission === false),
                class: (
                    ([ETableColumnType.CHECKBOX_ACTION, ETableColumnType.CHECKBOX_SHOW, ETableColumnType.CHECKBOX_CHANGE,
                    ETableColumnType.ACTION_BUTTON, ETableColumnType.ACTION_ICON,
                    ETableColumnType.IMAGE, EFormatDateDisplay.DATE_DMY, EFormatDateDisplay.DATE_DMY_Hm, EFormatDateDisplay.DATE_DMY_Hms
                    ].includes(col?.type) && ('justify-content-center text-center'))
                    || (col?.type === ETableColumnType.CURRENCY && ('justify-content-end text-right'))
                    || (col?.type === ETableColumnType.ACTION_DROPDOWN)
                )
                    + ' ' + col?.class,
            };
        });
    }

    getStyleColumn = (column: IColumn) => {
        let maxWidth = column.width;
        let width = column.width;
        let minWidth = column.width || column.minWidth;
        return {
            'width': width ? width + 'rem' : '',
            'min-width': minWidth ? minWidth + 'rem' : '',
            'right': column?.right + 'rem',
            'left': column?.left + 'rem',
            'position': column?.isFrozen ? 'sticky' : 'static',
            'background': column?.isFrozen ? 'inherit' : 'static',
        };
    }

    getValue(row: any, column: IColumn, valueType: ValueType) {
        let property: string = column.field;
        let value: string | number | boolean = row[property?.trim()];
        let propertiesColumn = Object.getOwnPropertyNames(this.columnOriginals[column.position]);
        // XỬ LÝ KHI VALUE NẰM TRONG OBJECT CON HOẶC VALUE THUỘC MỘT TẬP GIÁ TRỊ (A || B || C ...)
        // A,B,C có thể là các objectValue VD: customer.name || customer.investor.fullName || businessCustomer.name
        if (property.includes('||') || property.includes('.') && value === undefined) {
            let properties = property.split('||');
            for (property of properties) {
                property = property?.trim();
                if (property.includes('.')) {
                    let properties = property.split('.');
                    let propertyValue: any = row[properties[0]];
                    if (this.isObject(propertyValue)) {
                        properties.forEach((propertyChild, index) => {
                            if (index > 0 && this.isObject(propertyValue)) {
                                propertyValue = propertyValue[propertyChild];
                            }
                        });
                        value = propertyValue;
                    }
                } else {
                    value = row[property];
                }
                if (value) break;
            }
        }
        //
        if (propertiesColumn.includes('customValue')) {
            let param: any = value;
            if (valueType === ValueType.ROW) {
                param = row;
            }
            value = this.columnOriginals[column.position].customValue(param);
        }
        //
        return value;
    }

    isObject(value) {
        return (typeof value === 'object') && value !== null;
    }

    paginatorChange(event?: any) {
        if (event) {
            this.page.pageNumber = event.page;
            this.page.pageSize = event.rows;
            this.resetPositionScrollBar()
            this.pageChange.emit(this.page);
            if (this.page.pageSize !== this.page.pageSizeAll) {
                if (this.page.pageNumber === 0) {
                    this.reRenderHtml(this.variableNameRerenders.isShowPaginator);
                }
                this._onPageChange.emit(this.page);
            } else {
                this.resetPositionScrollBar();
                this.loadMore();
            }
        }
    }

    reRenderHtml(variableName: string) {
        this[variableName] = false;
        this.ref.detectChanges();
        this[variableName] = true;
        if (this.page.pageSize === this.page.pageSizeAll) {
            this.renameLabelPageSizeAll();
        }
    }

    loadMore() {
        this.renameLabelPageSizeAll();
        this._onPageChange.emit();
    }

    renameLabelPageSizeAll() {
        setTimeout(() => {
            try {
                const elementWrapperPaginator = this.wrapperPaginator.nativeElement;
                const elementLabelPageSize = elementWrapperPaginator.querySelector(".p-dropdown-label");
                elementLabelPageSize.textContent = "Tất cả";

                this.ref.detectChanges();
            } catch { }
        });
    }

    // LOAD MORE DATA WITH SOLUTION EVENT SCROLL
    @ViewChild('wrapperETable') wrapperETable: ElementRef<HTMLElement>;
    scrollHeightMaxBefore: number = 0;
    onScroll(event) {
        const elementTable = this.wrapperETable.nativeElement;
        let pos = (elementTable.scrollTop) + elementTable.offsetHeight;
        let max = elementTable.scrollHeight;
        //
        if ((pos >= (max - 150)) && (max > this.scrollHeightMaxBefore || this.page.pageNumberLoadMore === 1) && this.rows.length < this.page.totalItems && this.page.pageSize === this.page.pageSizeAll && !this.isLoading) {
            this.scrollHeightMaxBefore = max;
            this.loadMore();
        }
    }

    setColumn() {
        this.dialogService.open(
            SettingDisplayColumnTableComponent,
            {
                header: "Cài đặt hiển thị",
                width: '300px',
                styleClass: 'dialog-setcolumn',
                data: {
                    cols: this.columns,
                },
            }
        ).onClose.subscribe((response) => {
            if (response?.accept) {
                this.columns = response.data;
                this.setLocalStorageColumns(this.columns);
                this.setDistanceFrozenColumn();
            }
        });
    }

    // set khoảng cách fixed các cột fix lỗi của theme khi xử lý động các cột frozen
    setDistanceFrozenColumn() {
        let rightValueStyleFrozenColumn = 0;
        let leftValueStyleFrozenColumn = 0;
        this.columns = this.columns.map((item: IColumn) => {
            if (item?.isFrozen && item?.alignFrozen == ETableFrozen.LEFT && item.isShow) {
                item.left = leftValueStyleFrozenColumn;
                leftValueStyleFrozenColumn += item?.width;
            }
            return item;
        });
        //
        this.columns = this.columns.reverse().map((item: IColumn) => {
            if (item?.isFrozen && item?.alignFrozen === ETableFrozen.RIGHT && item.isShow) {
                item.right = rightValueStyleFrozenColumn;
                rightValueStyleFrozenColumn += item?.width;
            }
            return item;
        }).reverse();

        this.columnShows = this.columns.filter(col => col?.isShow);
        this.colspan = this.columnShows?.length;
        this.reRenderHtml(this.variableNameRerenders.isShowTable);
    }

    colspan: number;
    setLocalStorageColumns(data) {
        return Utils.setLocalStorage(this.columnCacheName, JSON.stringify(data));
    }


    onSelectedChange() {
        let emittedData = this.selectedItems.filter(row => !row?.isDisabled);
        if (this.selectedKey) {
            emittedData = emittedData.map(item => item[this.selectedKey]);
        }
        this.selectedItemsChange.emit(emittedData);
        this._onChangeCheckbox.emit(emittedData);
    }

    changeDisplayFilter() {
        this.isShowFilter = !this.isShowFilter;
        this.scrollHeight = !this.isShowFilter ? (this.scrollHeight + this.divFilterHeight) : (this.scrollHeight - this.divFilterHeight);
        this.isShowFilterChange.emit(this.isShowFilter);
    }

    onSort(event: any) {
        if (JSON.stringify(this.sortData) != JSON.stringify(event?.multisortmeta)) {
            this.sortData = [];
            event?.multisortmeta?.forEach(meta => {
                this.sortData.push({
                    field: meta.field,
                    order: meta.order
                });
            });
            this._onSort.emit(this.sortData);
        }
    }

    showImage(src) {
        this._libHelperService.dialogViewerRef(src, ContentTypeEView.IMAGE);
    }

    counter(i: number) {
        i = i > 1 ? Math.ceil(i) : 0;
        return new Array(i);
    }

    ngOnDestroy() {
        this.spinner.hide();
    }

    clickCheckBox(row) {
        this._onChangeCheckbox.emit(row);
    }

    clickCheckBoxAll() {
        this._onChangeCheckboxAll.emit();
    }

    showActions() {
    }

    _onRowReorder() {
        const rowPropertyOriginals = this.rows.map((item) => {
            const newItem: any = {};
            for (let property in this.rowItemClone) {
                newItem[property] = item[property];
            }
            return newItem;
        });
        //
        this.onRowReorder.emit(rowPropertyOriginals);
    }

    editImage(event: any, overlay: OverlayPanel, data: string) {
        event.stopPropagation()
        overlay.toggle(event);
        if (data) {

            this.mockupFrontEdit = this.api + data
        }
        else {
            this.mockupFrontEdit = ''
        }
    }

    preventOverlay(event: MouseEvent): void {
        event.stopPropagation();
    }

    logValue(value: any) {
        console.log(value)
    }

    async copyUrl(url: string) {
        try {
            await navigator.clipboard.writeText(url);
            if (this.isHorverToolTip) {
                this.toolTipCopy = 'copied'

            }
            else {
                this.toolTipCopy = 'Copy to clipbroad'
            }
        } catch (err) {
            alert('Copy ERROR')
        }
    }

    checkStatusResponse(response, message?: string): boolean {
        return this._libHelperService.checkStatusResponse(response, message);
    }

    updateNoteBatch(data: any) {
        const body = {
            id: data.id,
            note: this.noteBatch
        }
        this.batchService.updateNoteBatch(body)
            .pipe((finalize(() => this.isLoading = false)))
            .subscribe((res) => {
                if (this.checkStatusResponse(res, "Cập nhật Note thành công")) {
                    window.location.reload();
                }
            })
    }

    toggleOverlay(event: Event, row: any, op: any) {
        this.noteItem = row?.note;
        this.noteBatch = row?.note;
        op.toggle(event);
    }

    updateNoteItem(data: any) {
        const body = {
            id: data.id,
            note: this.noteItem
        }
        this.orderItemService.updateNoteItem(body)
            .pipe((finalize(() => this.isLoading = false)))
            .subscribe((res) => {
                if (this.checkStatusResponse(res, "Cập nhật Note thành công")) {
                    window.location.reload();
                }
            })
    }

}
