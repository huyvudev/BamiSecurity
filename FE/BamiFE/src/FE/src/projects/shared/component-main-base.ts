import { Directive, inject, NgZone, Renderer2 } from "@angular/core";

/**
 * Component base cho tất cả app
 */

@Directive()
export abstract class ComponentMainBase {
	
    constructor() {
		setTimeout(() => {
			this.onInitBase()
		}, 0);
    }

	ngZone: NgZone = inject(NgZone);
	renderer: Renderer2 = inject(Renderer2);
	bodyEl: HTMLElement;

	onInitBase() {
		this.bodyEl = document.body;
		this.ngZone.runOutsideAngular(() => {
			this.bodyEl.addEventListener('click', () => this.checkHasScroll());
		});
	}
   
	checkHasScroll() {
		const heightContent = this.bodyEl.scrollHeight; 
		const innerHeight = window.innerHeight;
		const hasScrollBar = heightContent > innerHeight;
		if(hasScrollBar) {
			this.renderer.addClass(this.bodyEl, 'hasScrollBar')
		} else {
			this.renderer.removeClass(this.bodyEl, 'hasScrollBar')
		}
	}
}
