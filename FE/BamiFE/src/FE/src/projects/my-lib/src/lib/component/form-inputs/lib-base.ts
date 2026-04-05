import { ChangeDetectorRef, Directive, DoCheck, InjectionToken, Input, NgZone, OnChanges, OnDestroy, OnInit, Optional, Self, SimpleChanges } from "@angular/core";
import { AbstractControl, ControlValueAccessor, NgControl } from "@angular/forms";
import { v4 as uuidv4 } from 'uuid';

export const LIB_NG_VALUE_ACCESSOR = new InjectionToken<ReadonlyArray<ControlValueAccessor>>('LibNgValueAccessor');

@Directive()
export abstract class LibBaseControl implements OnInit, DoCheck, OnDestroy, ControlValueAccessor {
  
	constructor(
        @Optional() @Self() public controlDir: NgControl,
        public cd: ChangeDetectorRef,
        public ngZone: NgZone,
    ) {
        if (controlDir) {
            controlDir.valueAccessor = this;
        }
    }

	@Input() label: string = '';
	@Input() pTooltipLabel: string = '';
    @Input() index: number;
	@Input() inputId: string;
	@Input() placeholder: string;
	@Input() isShowMessageInvalid: boolean = true;
	@Input() name: string;
	@Input() required: boolean = false;
	@Input() disableWhenRemoved?: boolean = false;
	@Input() disableAndResetValueWhenRemoved?: boolean = false;

	@Input() value: any;
	@Input() disabled: boolean;
	@Input() dynamicRequired: any;

    isRequired: boolean;

	get control() {
		return this.controlDir?.control;
	}

	changeDetectionCount: number = 0;
	
	_onChange = (value: any) => { };
	onTouched = () => { };

	ngOnInit(): void {
		this.setPlaceHolder();
		this.onInit();
		// Đo changeDetection
		// this.ngZone.onMicrotaskEmpty.subscribe(() => {
		// 	this.changeDetectionCount++;
		// 	console.log('Change detection triggered:', this.changeDetectionCount);
		// })
	}

	ngOnChanges(changes: SimpleChanges) {
		if(changes['required']) {
			this.isRequired = changes['required']?.currentValue;
		}
	}

	onInit() {
		const index = (this.index || this.index === 0) ? `-${this.index}` : '';
        this.inputId = (this.inputId || this.controlDir?.name || uuidv4()) + index;
        // this.inputId = (this.inputId || this.controlDir?.name || 'input-'+ performance.now()) + index;
		this.setValidators();
	}

	setPlaceHolder() {
		this.placeholder = this.placeholder === undefined ? this.label : this.placeholder;
	}

	writeValue(value: any): void {
		// console.log('value', value, this.controlDir?.name);
        this.value = value;
		this.cd.markForCheck();
    }
  
	registerOnChange(fn: any): void {
	  	this._onChange = fn;
	}
  
	registerOnTouched(fn: any): void {
	  	this.onTouched = fn;
	}

	isDisabled: boolean = false;
    setDisabledState(disabled: boolean): void {
		// console.log('setDisabledState', disabled, this.controlDir?.name);
        this.isDisabled = disabled;
		this.cd.markForCheck();
		// console.log('setDisabledState', disabled, this.isDisabled);
    }

	validators: any;
	setValidators() {
		this.validators = this.control?.validator ? [this.control.validator] : [];
		this.setRequired();	
		//
		this.control?.setValidators(this.validators);
		this.control?.updateValueAndValidity();
	}

	setRequired() {
		// console.log('setRequired', validators);
		const validators = this.control?.validator;
		if(validators) {
			const validatorFn = validators({} as AbstractControl);
			// console.log('validatorFn', validatorFn, this.controlDir?.name);
			this.isRequired = !!(validators && validatorFn?.['required']);
		} else {
			this.isRequired = false;
		}
		//
		this.cd.markForCheck();
	}

	compareValidator(validator1, validator2) {
		return validator1.toString() === validator2.toString();
	}

	pristineStatus: boolean;
	compareValidatorStatusBefore: boolean;
	updateValidatorComplete: boolean;
	timeoutUpdateValidator: any
	ngDoCheck() {
		// Nếu phát sinh hiệu suất ảnh hưởng cần tối ưu thêm về hiệu suất thì sẽ thêm Input cho từng dynamicRequired để chỉ cập nhật đúng input đó khi thay đổi
		if(this.dynamicRequired) {
			const validators = this.control?.validator ? [this.control?.validator] : [];
			const updateValidator = !this.compareValidator(this.validators, validators);
			// console.log('validators', validators, this.controlDir?.name);
			
			if(updateValidator && !this.updateValidatorComplete) {
				// console.log('updateValidator', this.controlDir?.name);
				this.updateValidatorComplete = true;
				this.setValidators();
				if(this.timeoutUpdateValidator) clearTimeout(this.timeoutUpdateValidator)
				this.timeoutUpdateValidator = setTimeout(() => {
					this.updateValidatorComplete = false;
				}, 300);
				// console.log(this.controlDir?.name + '--ngDoCheck--' + Utils.compareData(this.validators, validators));
			}
		}
		//
		// if(this.controlDir.name === 'code') {
		// 	console.log('pristine', this.control?.pristine);
		// }
		// if(!this.control?.pristine && this?.pristineStatus !== this.control?.pristine) {
		// 	this.pristineStatus = this.control.pristine;
		// 	console.log('pristine');
		// 	this.cd.markForCheck();
		// }
	}

	ngOnDestroy(): void {
		if(this.disableWhenRemoved) {
			// this.control.disable();
		}
	}
}