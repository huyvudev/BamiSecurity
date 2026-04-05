import { ChangeDetectionStrategy, Component, ContentChild, EventEmitter, InjectionToken, Input, OnChanges, OnInit, Optional, Output, Self, signal, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';
import { LibBaseControl } from '../lib-base';
import { Dropdown } from 'primeng/dropdown';
import { Utils } from '../../../shared/utils';

const LIB_SELECT_VALUE_ACCESSOR = new InjectionToken<ControlValueAccessor>(
	'LibSelectValueAccessor'
);

@Component({
    selector: 'lib-select',
    templateUrl: './lib-select.component.html',
    styleUrls: ['./lib-select.component.scss'],
    providers: [
		{
			provide: LIB_SELECT_VALUE_ACCESSOR,
			useExisting: LibSelectComponent
		},
	],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class LibSelectComponent extends LibBaseControl implements OnChanges {

	@Input() data: any;	// Thay thế khai báo options, optionLabel, optionValue => [options, optionLabel, optionValue]
	@Input() itemDisableds: any[] = [];

    private _options = signal<any[]>(null);
    @Input() set options(value) {
        if(value) this._options.set(Utils.cloneData(value));
    }
    //
    get options() {
        return this._options();
    }
	
    @Input() optionLabel: string;
    @Input() optionValue: string;
	@Input() activeReferences: boolean = false;
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
	@Input() filterPlaceholder: string;
	@Input() filterLocale: string;
	@Input() dataKey: string;
	@Input() filterBy: string;
	@Input() filterValue: string;
	@Input() filterMatchMode: "endsWith" | "startsWith" | "contains" | "equals" | "notEquals" | "in" | "lt" | "lte" | "gt" | "gte" = "contains";

	@Input() virtualScroll: boolean = false;	
	@Input() virtualScrollItemSize: number;	

	@Input() showTooltip: boolean = false;
	@Input() tooltip: string = '';	
	@Input() tooltipPosition: "left" | "top" | "bottom" | "right" = "bottom";	
	// @Input() tooltipPositionStyle: string = 'absolute';	
	// @Input() tooltipStyleClass: string = null;	
	
	@Input() optionDisabled: string = 'isDisabledItem';	// tên của property, nếu option chứa property này với giá trị !==['', undefined, null] sẽ bị disable
	@Input() groupConfig: any = {
		group: false,
		optionGroupLabel: 'label',
		optionGroupChildren: 'items'	
	};	
	@Input() showClear: boolean = false;	
	@Input() emptyFilterMessage: string = 'Không có dữ liệu';	
	@Input() emptyMessage: string = 'Không có dữ liệu';

	@Input() genLabelCode: string[];
	@Input() genLabelCodeSeparator: string = ' - ';

	@Input() resetFilterOnHide: boolean = true;
	@Input() autoDisplayFirst: boolean = false;
	@Input() truncateLength: number;


	// @Input() tabindex: number = 0;
	// @Input() variant: "outlined" | "filled" = "outlined";
	// @Input() loadingIcon: string = null;
	// @Input() filterFields: any[] = null;
	// @Input() checkmark: boolean = false;
	// @Input() loading: boolean = false;
	// @Input() autofocus: boolean = false;
	// @Input() dropdownIcon: string = null;

	// @Input() lazy: boolean = false;	
	
	// @Input() virtualScrollOptions: ScrollerOptions = null;	
	// @Input() overlayOptions: OverlayOptions = null;	
	// @Input() ariaFilterLabel: string = null;	
	// @Input() ariaLabel: string = null;	
	
	// @Input() focusOnHover: boolean = false;	
	// @Input() selectOnFocus: boolean = false;	
	// @Input() autoOptionFocus: boolean = true;	
	// @Input() autofocusFilter: boolean = true;	
	// @Input() filterValue: string = null;	
	

	@Output() onChange = new EventEmitter();
	@Output() onBlur = new EventEmitter();
	@Output() onFilter = new EventEmitter();
	@Output() onFocus = new EventEmitter();
	@Output() onClick = new EventEmitter();
	@Output() onShow = new EventEmitter();
	@Output() onHide = new EventEmitter();
	@Output() onClear = new EventEmitter();
	@Output() onLazyLoad = new EventEmitter();

	@ContentChild('selectedItemTemplate') selectedItemTemplate: TemplateRef<any>;
	@ContentChild('itemTemplate') itemTemplate: TemplateRef<any>;

	@ViewChild('dropdown') dropdown: Dropdown;
	isInit: boolean = true;

	override writeValue(value: any): void {
		super.writeValue(value);
		this.setTooltip(value, true)
	}

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
			this.setFirstItem()
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
			this.setFirstItem()
		}
		//
		if(changes['tooltip']?.currentValue) {
			this.showTooltip = true;
		}
		//
		this.setVirtualScroll();
	}

	setVirtualScroll() {
		if(this.options?.length > 300) {
			this.virtualScroll = true;
			this.virtualScrollItemSize = this.virtualScrollItemSize || 30;
		}
	}

	ngAfterViewInit() {
		this.dropdown.onFilterInputChange = this.customFilter.bind(this)
	}

	searchTimeOut: any;
	customFilter(event: Event) {
		if(this.searchTimeOut) clearTimeout(this.searchTimeOut);
		this.searchTimeOut = setTimeout(() => {
			const value: string = (event.target as HTMLInputElement).value;
			(event.target as HTMLInputElement).value = value.trim();
			Dropdown.prototype.onFilterInputChange.apply(this.dropdown, arguments);
			//
			this.dropdown.cd.detectChanges();
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
		}
		//
		this.setTooltip(this.value, true);
	}

	setFirstItem() {
		if(!this.value) {
			if(this.options?.length && this.autoDisplayFirst) {
				const firstItem = this.options[0];
				let value: any;
				if(this.optionValue) {
					const propertieLevels = this.optionValue?.split('.');
					propertieLevels.forEach((property, index) => {
						if(index === 0) {
							value = firstItem?.[property];
						} else {
							value = value?.[property];
						}
					});
				} else {
					value = this.options[0];
				}
				//
				
				setTimeout(() => {
					this.value = value;
					this._onChange(value);
					this.cd.markForCheck();
				}, 0);
			}
		}
	}

	isShowDropdown: boolean;
	_onShow($event) {
		this.isShowDropdown = true;
	}

	_onHide($event) {
		this.isShowDropdown = false;
	}

	_onClear() {
		this.onClear.emit();
		setTimeout(() => {
			this.dropdown.hide();
		}, 0);
	}

	setTooltip(value, detectChanges?: boolean) {
		if(this.showTooltip) {
			const data = this.options?.find(item => item?.[this.optionValue] === value);
			this.tooltip = data?.[this.optionLabel] || '';
			if(detectChanges) {
				this.cd.detectChanges();
			}
		}
	}

	_handleOnChange(value) {
		// console.log('_handleOnChange', value);
		this.setTooltip(value)
		this.dropdown.hide()
		this.onChange.emit(value)
	}
}
