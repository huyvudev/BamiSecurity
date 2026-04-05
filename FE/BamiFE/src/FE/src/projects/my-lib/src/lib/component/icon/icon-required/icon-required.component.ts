import { Component, Input } from '@angular/core';

@Component({
	selector: 'icon-required',
	template: `
		<span class="required-field" *ngIf="isShow"> * </span>
	`
})
export class IconRequiredComponent {
	@Input() isShow: boolean = true
}
