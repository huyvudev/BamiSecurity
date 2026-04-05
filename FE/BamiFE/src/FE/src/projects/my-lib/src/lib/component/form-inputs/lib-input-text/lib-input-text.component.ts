import { BootstrapOptions, ChangeDetectionStrategy, Component, EventEmitter, forwardRef, InjectionToken, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';
import { LIB_NG_VALUE_ACCESSOR, LibBaseControl } from '../lib-base';
import { InputText } from 'primeng/inputtext';
import { InputTextarea } from 'primeng/inputtextarea';

const LIB_INPUT_TEXT_VALUE_ACCESSOR = new InjectionToken<ControlValueAccessor>(
	'LibInputTextValueAccessor'
);

@Component({
	selector: 'lib-inputText',
	templateUrl: './lib-input-text.component.html',
	styleUrls: ['./lib-input-text.component.scss'],
	providers: [
		{
			provide: LIB_NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => LibInputTextComponent),
			multi: true,
		}
	],
	changeDetection: ChangeDetectionStrategy.OnPush
})

export class LibInputTextComponent extends LibBaseControl implements OnChanges {
	
	// InputText
	@Input() type: string = 'text';
	@Input() maxlength: string;

	// InputTextarea
	@Input() textarea: boolean;
	@Input() rows: number = 2;
	@Input() autoResize: boolean = false;
	@Input() autocomplete: 'on' | 'off' = 'off';

	@Input() showTooltip: boolean = false;
	@Input() tooltip: string = '';	
	@Input() tooltipPosition: "left" | "top" | "bottom" | "right" = "bottom";

	@Output() onInput = new EventEmitter<any>();
	@Output() blur = new EventEmitter<any>();
	@Output() keyup = new EventEmitter<any>();

	@ViewChild('inputText') inputText: InputText;
	@ViewChild('inputTextarea') inputTextarea: InputTextarea;

	firstValue: any;
	isInit: boolean = true;

	override writeValue(value: any): void {
		if(this.isInit && !value) this.firstValue = value;
        this.value = value ? value.trim() : null;
		this.isInit = false;
		this.cd.markForCheck();
    }

	override ngOnChanges(changes: SimpleChanges): void {
		super.ngOnChanges(changes);
		//
		if(changes['tooltip']?.currentValue) {
			this.showTooltip = true;
		}
	}
}
