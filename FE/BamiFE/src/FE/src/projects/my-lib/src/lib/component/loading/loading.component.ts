import { Component, Input, SimpleChanges } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
    selector: 'lib-loading',
    template: `
        <ngx-spinner
            type="ball-spin-clockwise"
            size="default"
            [fullScreen]="true">
            <p class="text-white" > {{ message }} </p>
        </ngx-spinner>
    `,
    providers: [NgxSpinnerService]
})
export class LoadingComponent {

    @Input() message: string = 'Đang xử lý';
    @Input() isSpinner: boolean = false;

    constructor(
        private spinner: NgxSpinnerService,
    ) {}

    layoutContentElement: HTMLElement;

    ngOnChanges(changes: SimpleChanges) {
        if(changes['isSpinner'].currentValue) {
            this.spinner.show();
        } else {
            this.spinner.hide();
        }
    }

    ngOnDestroy(): void {
        this.spinner.hide();
    }
}
