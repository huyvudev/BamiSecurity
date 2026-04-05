import { Directive, ElementRef, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[clickOutside]'
})
export class ClickOutsideDirective {

  	constructor(
    	private el: ElementRef,
	) { }

	@Output() clickOutside = new EventEmitter<MouseEvent>();

	@HostListener('document:click', ['$event']) onClick(event: PointerEvent) {
		const inside = this.el.nativeElement.contains(event.target);
		if(!inside) {
			this.clickOutside.emit();
		}

	}
}
