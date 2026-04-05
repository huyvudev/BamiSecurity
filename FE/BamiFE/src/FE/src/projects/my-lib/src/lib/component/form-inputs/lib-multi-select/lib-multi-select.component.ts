import { ChangeDetectionStrategy, Component, ContentChild, EventEmitter, forwardRef, Input, OnChanges, Output, signal, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { LIB_NG_VALUE_ACCESSOR, LibBaseControl } from '../lib-base';
import { MultiSelect } from 'primeng/multiselect';
import { Utils } from '../../../shared/utils';


@Component({
	selector: 'lib-multiSelect',
	templateUrl: './lib-multi-select.component.html',
	styleUrls: ['./lib-multi-select.component.scss'],
	providers: [
		{
			provide: LIB_NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => LibMultiSelectComponent),
			multi: true,
		}
	],
	changeDetection: ChangeDetectionStrategy.OnPush
})

export class LibMultiSelectComponent extends LibBaseControl implements OnChanges {

    @Input() data: any;	// Thay thế khai báo options, optionLabel, optionValue => [options, optionLabel, optionValue]
    @Input() itemDisableds: any[] = [];

    // private _options = signal<any[]>(null);
    // @Input() set options(value) {
    //     this._options.set(Utils.cloneData(value));
    // }
    // //
    // get options() {
    //     return this._options();
    // }

    @Input() options: any[] = [];
    @Input() optionLabel: string;
    @Input() optionValue: string;
    @Input() activeReferences: boolean = false; // data từ cha truyền sang vẫn giữ tính reference của javascipt - VD: options ...
    //
    @Input() scrollHeight: string = '200px';
    @Input() filter: boolean = false;
    @Input() style: Object = null;
    @Input() styleClass: string = null;
    @Input() panelStyle: Object = null;
    @Input() panelStyleClass: string = null;
    @Input() readonly: boolean = false;
    @Input() editable: boolean = false;	// Cho phép nhập như inpuy và giá trị lấy được = "giá trị nhập"
    @Input() appendTo: string = 'body';
    @Input() filterPlaceHolder: string;
    @Input() filterLocale: string;
    @Input() dataKey: string;
    @Input() filterBy: string;
    @Input() filterMatchMode: "endsWith" | "startsWith" | "contains" | "equals" | "notEquals" | "in" | "lt" | "lte" | "gt" | "gte" = "contains";
    @Input() displaySelectedLabel: boolean = true;
    @Input() maxSelectedLabels: number;
    @Input() selectionLimit: number;
    @Input() showToggleAll: boolean = true;
    @Input() resetFilterOnHide: boolean = true;
    @Input() filterValue: string;
    @Input() showHeader: boolean = true;
    @Input() display: string | 'comma' | 'chip' = 'comma';
	
    @Input() showTooltip: boolean = false;
	@Input() tooltip: string  = '';
	@Input() tooltipPosition: "left" | "top" | "bottom" | "right" = "right";	
	// @Input() tooltipPositionStyle: string = 'absolute';	
	// @Input() tooltipStyleClass: string = null;	

    @Input() optionDisabled: string = 'isDisabledItem';	// tên của property, nếu option chứa property này với giá trị !==['', undefined, null] sẽ bị disable
    @Input() groupConfig: any = {
        group: false,
        optionGroupLabel: 'label',
        optionGroupChildren: 'items'	
    };	
    @Input() showClear: boolean = false;	
    @Input() emptyFilterMessage: string = '';	
    @Input() emptyMessage: string = '';

    @Input() genLabelCode: string[];
    @Input() genLabelCodeSeparator: string = ' - ';

    @Output() onChange = new EventEmitter();
	@Output() onFilter = new EventEmitter();
	@Output() onFocus = new EventEmitter();
	@Output() onBlur = new EventEmitter();
	@Output() onClick = new EventEmitter();
	@Output() onClear = new EventEmitter();
	@Output() onPanelShow = new EventEmitter();
	@Output() onPanelHide = new EventEmitter();
	@Output() onLazyLoad = new EventEmitter();
	@Output() onRemove = new EventEmitter();
	@Output() onSelectAllChange = new EventEmitter();

    @ContentChild('selectedItemTemplate') selectedItemTemplate: TemplateRef<any>;
    @ContentChild('itemTemplate') itemTemplate: TemplateRef<any>
    @ContentChild('footerTemplate') footerTemplate: TemplateRef<any>

    @ViewChild('multiSelect') multiSelect: MultiSelect;

    override ngOnChanges(changes: SimpleChanges): void {
		super.ngOnChanges(changes);
		if(changes['data']?.currentValue) {
			if(Array.isArray(this.data) && this.data.length) {
                if(this.activeReferences) { 
                    this.options = this.data?.[0];
                } else {
                    this.options = Utils.cloneData(this.data?.[0]);
                }
				this.optionLabel = this.data?.[1];
				this.optionValue = this.data?.[2];
			}
			//
			this.genOptionLabel();
		}
		// set optionLabel và optionValue
		if(!this.optionLabel && !this.optionValue) {
			const firstItem = this.data?.[0]?.[0] || this.options?.[0];
			// console.log('firstItem', firstItem);
			this.optionLabel = (firstItem?.label && 'label') || (firstItem?.name && 'name');
			this.optionValue = (firstItem?.code && 'code') || (firstItem?.id && 'id');
		}

		if(changes['options']?.currentValue) {
            if(!this.activeReferences) {
                this.options = Utils.cloneData(this.options);
            }
			this.genOptionLabel();
		}

		if(changes['tooltip']?.currentValue) {
			this.showTooltip = true;
		}
	}

    ngAfterViewInit() {
		this.multiSelect.onFilterInputChange = this.customFilter.bind(this)
	}

	searchTimeOut: any;
	customFilter(event: Event) {
		if(this.searchTimeOut) clearTimeout(this.searchTimeOut);
		this.searchTimeOut = setTimeout(() => {
			const value: string = (event.target as HTMLInputElement).value;
			(event.target as HTMLInputElement).value = value.trim();
			MultiSelect.prototype.onFilterInputChange.apply(this.multiSelect, arguments);
			//
			this.multiSelect.cd.detectChanges();
			(event.target as HTMLInputElement).value = value;
		}, 200)
	}

	genOptionLabel() {
		if(this.genLabelCode) {
			this.optionLabel = 'labelGen';
			this.options = this.options.map((option) => {
				const data: string[] = this.genLabelCode.map(item => {
					const propertieLevels = item?.split('.');
					let value: any;
					propertieLevels.forEach((property, index) => {
						if(index === 0) {
							value = option?.[property];
						} else {
							value = value?.[property];
						}
					});
					return value;
				})
				option[this.optionLabel] = data.join(this.genLabelCodeSeparator);
				return option;
			})
		};
		//
		this.setTooltip(this.value, true);
	}

	isShowDropdown: boolean;
	_onPanelShow(event) {
		this.isShowDropdown = true;
	}

	_onPanelHide(event) {
		this.isShowDropdown = false;
	}

	setTooltip(values, detectChanges?: boolean) {
		const items = this.options.filter(item => values?.includes(item?.[this.optionValue]));
		const dataLabel = items?.map(item => item?.[this.optionLabel]);
		if(dataLabel?.length) {
			var content = '';
			const lastItemIndex = dataLabel.length - 1;
			dataLabel.forEach((label, index) => {
				const lastItem = index === lastItemIndex
				content += `<div> ${label} ${lastItem ? '' : ','} </div> </br>`;
			})
		}
		//		
		this.tooltip = content;
		if(detectChanges) {
			this.cd.detectChanges();
		}
	}
}
