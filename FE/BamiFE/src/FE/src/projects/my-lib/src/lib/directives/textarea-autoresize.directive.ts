import { ChangeDetectorRef, Directive, ElementRef, EventEmitter, HostListener, Output } from '@angular/core';

@Directive({
  	selector: '[textareaAutoresize]'
})
export class TextareaAutoresizeDirective {

  	constructor(
    	private el: ElementRef,
	) { }

	scrollHeight: number = 0;
	
	@HostListener(':scroll')
	scroll() {
		const setScrollHeight = !!((this.el.nativeElement.offsetHeight === this.scrollHeight && this.scrollHeight) || !this.scrollHeight);
		this.resize(setScrollHeight);
	}

	ngOnInit() {
		if(this.el.nativeElement.scrollHeight) {
			setTimeout(() => this.resize());
		}
	}

	resize(setScrollHeight?: boolean) {
		const height = this.el.nativeElement.scrollHeight;
		if(height && this.scrollHeight !== height && setScrollHeight) {
			this.scrollHeight = height + 5;
			this.el.nativeElement.style.height = this.scrollHeight + 'px';
		}
	}

}
