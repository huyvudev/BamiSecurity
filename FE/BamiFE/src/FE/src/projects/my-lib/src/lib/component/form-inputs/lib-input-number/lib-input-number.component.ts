import { ChangeDetectionStrategy, Component, EventEmitter, forwardRef, InjectionToken, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { LIB_NG_VALUE_ACCESSOR, LibBaseControl } from '../lib-base';
import { Utils } from '../../../shared/utils';
import { InputNumber } from 'primeng/inputnumber';

const keyNumbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
@Component({
	selector: 'lib-inputNumber',
	templateUrl: './lib-input-number.component.html',
	styleUrls: ['./lib-input-number.component.scss'],
	providers: [
		{
			provide: LIB_NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => LibInputNumberComponent),
			multi: true
		}
	],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class LibInputNumberComponent extends LibBaseControl implements OnInit, OnChanges {

	// Properties
	@Input() cursorMovementSupport: boolean = false;
    @Input() showButtons?: boolean = false;
    @Input() format?: boolean = true;
    @Input() buttonLayout?: "stacked" | "horizontal" | "vertical" = 'stacked';
    @Input() styleClass?: string | undefined;
    @Input() style?: Object;
    @Input() size?: number | undefined;
    @Input() maxlength?: number | undefined;
    @Input() tabindex?: number | undefined;
    @Input() title?: string | undefined;
	@Input() autocomplete?: string | undefined = 'off';
	@Input() min?: number | undefined = 0;
	@Input() max?: number | undefined = 999999999999999;
	@Input() incrementButtonClass?: string | undefined;
	@Input() decrementButtonClass?: string | undefined;
	@Input() incrementButtonIcon?: string | undefined;
	@Input() decrementButtonIcon?: string | undefined;
	@Input() readonly?: boolean = false;
	@Input() step: number = 1;
	@Input() allowEmpty?: boolean = true;
	@Input() locale?: string = 'vi-VN'; // 'de-DE | en-US | en-IN | ja-JP | vi-VN | zh-CN'
	@Input() localeMatcher?: "lookup" | "best fit";
	@Input() mode?: 'decimal' | 'currency' = 'decimal';
	@Input() variant?: 'filled' | 'outlined' = 'outlined';
	@Input() currency?: string | undefined;
	@Input() currencyDisplay?: string | undefined;
	@Input() useGrouping?: boolean = true;
	@Input() minFractionDigits?: number | undefined;
	@Input() maxFractionDigits?: number | undefined;
	@Input() isInteger: boolean = true;
	@Input() prefix?: string | undefined;
	@Input() suffix?: string | undefined;
	@Input() inputStyle?: any;
	@Input() inputStyleClass?: string | undefined;
	@Input() showClear?: boolean = false;
	@Input() autofocus?: boolean | undefined;
	@Input() ariaLabel?: string | undefined = "ariaLabel";

    // Emitters
	@Output() onInput = new EventEmitter<any>();
	@Output() onFocus = new EventEmitter<any>();
	@Output() onBlur = new EventEmitter<any>();
	@Output() onKeyDown = new EventEmitter<any>();
	@Output() onClear = new EventEmitter<any>();

	@ViewChild('inputNumber') inputNumber: InputNumber;

	decimalSeparator: string = ',';
	thousandSeparator: string = '.';

	cacheValue: number;
	onChangeTimeOut: any;
	inputEl: any;

	selectionEnd: number;
	keyNumberActives: string[];
	keyDown: string;

	count: number = 0;
	override ngOnInit(): void {
		super.ngOnInit()
		setTimeout(() => {
			if (this.max && this.value && this.value > this.max) {
				this.value = this.max;
				this._onChange(this.max);
			}
			//
			this.cacheValue = this.value;
			//
			this.cd.detectChanges();
		}, 0);
	}

	override ngOnChanges(changes: SimpleChanges): void {
		super.ngOnChanges(changes);
		if(changes['locale']?.currentValue) {
			if(['en-US', 'en-IN', 'ja-JP', 'zh-CN'].includes(this.locale)) {
				this.decimalSeparator = '.';
				this.thousandSeparator = ',';
			}
		}

		if(changes['isInteger']?.currentValue === false) {
			// Dùng biến isInteger để đặt giá trị default cho bớt các khai báo
			if(!Utils.valueExist(this.minFractionDigits)) this.minFractionDigits = 0;
			if(!Utils.valueExist(this.maxFractionDigits)) this.maxFractionDigits = 2;
			// this.cd.detectChanges();
		}
	}
	
	onValueChange(event): void {
		let isLimitValue: boolean;
		if(this.onChangeTimeOut) clearTimeout(this.onChangeTimeOut);
		if(this.max) {
			this.onChangeTimeOut = setTimeout(() => {
				// console.log('onValueChange-keyDown', this.keyDown, this.value, this.selectionEnd);
				if(keyNumbers.includes(this.keyDown)) {
					var modelValue = event.value;
					if (this.max && modelValue && modelValue > this.max) {
						isLimitValue = true;
						modelValue = this.cacheValue || this.max;
						this.value = this.cacheValue || this.max
					}
					//
					this.cacheValue = Utils.cloneData(modelValue);
				} else {
					this.value = this.cacheValue;
					modelValue = this.cacheValue;
				}
				this._onChange(modelValue);
				this.cd.markForCheck();
				if((this.minFractionDigits || this.maxFractionDigits) && isLimitValue) {
					setTimeout(() => {
						this.inputEl.setSelectionRange(this.selectionEnd + 1, this.selectionEnd + 1);
					}, 0);
				}
			}, 0);
		} else {
			this._onChange(event.value);
		}
		
	}
	
	_onKeyDown(event: KeyboardEvent) {
		//
		const getFormattedValue = (value) => {
			let formattedValue = Utils.formatCurrency(value);
			if(this.minFractionDigits) {
				if(Number.isInteger(value)) {
					for(let i = 1; i <= this.minFractionDigits; i++) {
						if(i === 1) {
							formattedValue = formattedValue + `${this.decimalSeparator}0`;
						} else {
							formattedValue = formattedValue + '0';
						}
					}
				}
			}
			return formattedValue;
		}
		//
		const getKeyActives = (formattedValue: string, positionCursor: number, event): string[] => {
			const keyNumberActives = []
			const prefixLength = this.prefix?.length || 0;
			positionCursor = positionCursor - prefixLength;

			// Fix lỗi minFractionDigits = 0 không nhập được số sau dấu thập phân
			if(this.minFractionDigits === 0) {
				if(event.key === this.decimalSeparator) {
					formattedValue = formattedValue+this.decimalSeparator;
					positionCursor = positionCursor + 1;
				} else {
					formattedValue = event.target.value;
				}
			}
			//
			for (const keyNumber of keyNumbers) {
				const formattedValueTest = (formattedValue && formattedValue.slice(0, positionCursor) + keyNumber + formattedValue.slice(positionCursor, formattedValue?.length)) || keyNumber;
				const formattedValueNumber = +formattedValueTest.replaceAll(this.thousandSeparator, '').replaceAll(this.decimalSeparator, '.');
				const checkValueMax = typeof(formattedValueNumber) === 'number' && formattedValueNumber > this.max;
				if(!checkValueMax) {
					keyNumberActives.push(keyNumber)
				} else {
					break;
				}
			};
			//
			return keyNumberActives;
		}

		this.keyDown = event.key;
		this.inputEl = event.target;

		if(event.key === 'Backspace') {
			// Fix lỗi của theme bị nhảy con trỏ khi có suffix và giá trị có phần thập phân
			setTimeout(() => {
				if(this.value?.toString()?.length === 1) {
					this.inputEl.setSelectionRange(1,1);
				}
			}, 0);
		}

		if(this.max) {
			const key = event.key;
			const input = event.target as HTMLInputElement;
			let value = this.value;
			var formattedValue = getFormattedValue(value);
			//
			this.selectionEnd = input?.selectionEnd || 0;
			let positionCursor: number;
			const moveCursor: boolean = this.selectionEnd === input?.selectionStart;
			// console.log('key', key, this.keyDown, this.selectionEnd);
			
			// Xử lý khi con trỏ thay đổi và nhập số vào một vị trí bất kỳ trong chuỗi
			// Chỉ xử lý khi di chuyển con trỏ và nhập số chứ không phải thao tác thay thế 1 nhóm số
			if(moveCursor) {
				positionCursor = this.selectionEnd;
				// Di chuyển con trỏ sang trái
				if(event.key && key === 'ArrowLeft') {
					positionCursor = this.selectionEnd >= 1 && this.selectionEnd - 1;
				} 
				// Di chuyển con trỏ sang phải
				else if(event.key && key === 'ArrowRight') {
					positionCursor = this.selectionEnd <= formattedValue?.length && this.selectionEnd + 1;
				}
				//
				this.keyNumberActives = [];
				this.keyNumberActives = getKeyActives(formattedValue, positionCursor, event);
				//
				if(key) {
					// Xử lý con trỏ tự động nhảy sang phần thập phân khi phần nguyên không thể điền thêm số
					// Cách 1 là tự động nhảy
					if(this.cursorMovementSupport) {
						setTimeout(() => {
							let formattedValue = getFormattedValue(this.value); 
							const keyNumberActives = getKeyActives(formattedValue, positionCursor + 1, event);
							const positionCursorDecimalSeparator = formattedValue.search(this.decimalSeparator);
							// console.log(this.selectionEnd, key, positionCursorDecimalSeparator, keyNumberActives);
							if(keyNumberActives.length === 0 && this.minFractionDigits && keyNumbers.includes(key)) {
								this.inputEl.setSelectionRange(positionCursorDecimalSeparator + 1, positionCursorDecimalSeparator + 1);
							}
						}, 0);
					} 
					// Cách 2 là khi đến giá trị max cho phần nguyên thì nhấn phím ứng với biến decimalSeparator thì sẽ nhảy sang phần thập phân
					else {
						const positionCursorDecimalSeparator = formattedValue.search(this.decimalSeparator) + (this.prefix?.length || 0);
						// console.log(key === this.decimalSeparator, positionCursorDecimalSeparator, this.selectionEnd , this.keyNumberActives, moveCursor);
						if(key === this.decimalSeparator && positionCursorDecimalSeparator === this.selectionEnd && this.keyNumberActives.length === 0 && this.minFractionDigits) {
							this.inputEl.setSelectionRange(positionCursorDecimalSeparator + 1, positionCursorDecimalSeparator + 1);
						}
					}
				}
			} else {
				this.keyNumberActives = keyNumbers;
			}
			//
			if (['ArrowLeft', 'ArrowRight', 'Backspace', 'Delete', 'Tab', 'ArrowDown', 'ArrowUp',, ...this.keyNumberActives, ...[this.decimalSeparator]].includes(event.key)) {
				this.keyDown = this.keyNumberActives.includes(this.keyDown) ? this.keyDown : '0';
				return;
			}

			if (event.ctrlKey && ['a', 'A', 'v', 'V','c', 'C'].includes(event.key)) {
				this.keyDown = this.keyNumberActives.includes(this.keyDown) ? this.keyDown : '0';
				return;
			}
			//
			event.preventDefault(); 
		}
		//
		
	}

	onPaste(event: ClipboardEvent) {
		event.preventDefault();
		// const input = event.target as HTMLInputElement;
		// const pastedInput: string = event.clipboardData.getData('text/plain');
		// const isDataNumber = typeof(+pastedInput) === 'number';
		if(this.value > this.max) {
			this.value = this.cacheValue;
			this._onChange(this.value);
		}
	}
}
