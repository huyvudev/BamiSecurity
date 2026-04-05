import { AfterViewInit, ChangeDetectorRef, Directive, Input, OnChanges, OnInit, Renderer2, SimpleChanges } from '@angular/core';
import Inputmask from "inputmask";

@Directive({
  	selector: '[inputMask]',
})
export class InputMaskDirective implements OnChanges {

	constructor(
        
    ) {}

    @Input() mask: string;
    @Input() inputId: string;

	ngOnChanges(changes: SimpleChanges): void {
		if(changes['mask']?.currentValue) {
			if(this.inputElement) {
				new Inputmask(this.mask).mask(this.inputElement);
			}
		}
	}

    get inputElement(): HTMLInputElement {
        return document.getElementById(this.inputId) as HTMLInputElement;
    }
}
