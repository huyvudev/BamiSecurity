import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[clickDropdown]'
})
export class ClickDropdownDirective {

	constructor(
    	private el: ElementRef,
	) { }

	@Input() filterMaxlength: number;

	@HostListener('document:click', ['$event']) onClick(event: PointerEvent) {
		try {
			const wrapperFilterEl: any = document.getElementsByClassName("dropdown-content");
			let inputSearchElExsit: boolean;
			if(wrapperFilterEl?.length > 0 && this.filterMaxlength) {
				for(let el of wrapperFilterEl) {
					const elInputSearch = el.getElementsByClassName("p-dropdown-filter")[0];
					if(elInputSearch && !inputSearchElExsit) {
						inputSearchElExsit = true;
						elInputSearch.maxLength = this.filterMaxlength;
					}
				}
			}
		} catch {}
	}
}
