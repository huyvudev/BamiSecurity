import { AfterViewInit, ChangeDetectionStrategy, Component, EventEmitter, InjectionToken, Input, numberAttribute, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { LibBaseControl } from '../lib-base';
import { ControlValueAccessor } from '@angular/forms';
import { Utils } from '../../../shared/utils';
import { RadioButton } from 'primeng/radiobutton';
import { IRadioButtonValue } from '../../../shared/interfaces/base.interface';

const LIB_RADIO_BUTTON_VALUE_ACCESSOR = new InjectionToken<ControlValueAccessor>(
	'LibRadioButtonValueAccessor'
)

@Component({
    selector: 'lib-radioButton',
    templateUrl: './lib-radio-button.component.html',
    styleUrls: ['./lib-radio-button.component.scss'],
	providers: [
		{
			provide: LIB_RADIO_BUTTON_VALUE_ACCESSOR,
			useExisting: LibRadioButtonComponent
		}
	],
	changeDetection: ChangeDetectionStrategy.OnPush
})


export class LibRadioButtonComponent extends LibBaseControl implements OnInit {

	@Input() style: Object;
	@Input() styleClass: string = '';
	@Input() labelStyleClass: string = '';
	@Input({ transform: (value: number | undefined) => numberAttribute(value, undefined)}) defaultValueIndex: number | undefined;
	@Input() dataKey: string;
	@Input() displayType: 'horizontal' | 'vertical' = 'horizontal';
	@Input() values: IRadioButtonValue[] = [];

	@Output() onClick = new EventEmitter<any>(null);
	@Output() onFocus = new EventEmitter<any>(null);
	@Output() onBlur = new EventEmitter<any>(null);

	@ViewChild('radioButton') radioButton: RadioButton;

	override ngOnInit(): void {
		super.ngOnInit();
	}

	firstValueExist: boolean;
	

	override writeValue(value: any): void {
		if(typeof(this.defaultValueIndex) !== 'number' || Utils.valueExist(value)) { 
			this.value = value;
			this.cd.markForCheck();
		} else {
			this.handleWriteValue();
		}
	}

	isInitWrite: boolean = true;
	handleWriteValue() {
		if(this.isInitWrite) {
			this.value = this.radioValue(this.values?.[this.defaultValueIndex]);
			this._onChange(this.value);
			this.cd.markForCheck();
			this.isInitWrite = false;
		} else {
			setTimeout(() => {
				this._onChange(this.value);
			}, 0);
		}
	}

	radioValue = (item) => {
		if(this.dataKey) {
			const propertieLevels = this.dataKey?.split('.');
			let value: any;
			propertieLevels.forEach((property, index) => {
				if(index === 0) {
					value = item?.[property];
				} else {
					value = value?.[property];
				}
			});
			return value;
		} else {
			return item
		}
	}

	_onClick(event) {
		this._onChange(this.value);
	}

	_onFocus(event) {
		// console.log('_onFocus');
	}

	_onBlur(event) {
		// console.log('_onBlur');
	}
	
}
