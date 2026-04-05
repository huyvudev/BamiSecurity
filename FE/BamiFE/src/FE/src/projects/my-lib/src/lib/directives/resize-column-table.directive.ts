import { ChangeDetectorRef, Directive, ElementRef, EventEmitter, Input, Output, Renderer2 } from '@angular/core';

@Directive({
    selector: '[resizeColumn]'
})
export class ResizeColumnTableDirective {

    @Input("resizeColumn") resizable: boolean;
    @Output() _onResize = new EventEmitter<{checked: boolean, element: HTMLElement}>();

    onResize: boolean;
    column: HTMLElement;
    startX: number;
    table: HTMLElement;
    startWidth: number;
    tableStartWidth: number;
    columnResizes: any[];
    listenerFuncs: Function[] = [];
    indexColumn: number;

    constructor(
        private el: ElementRef<HTMLElement>,
        private ref: ChangeDetectorRef,
        private renderer: Renderer2,
    ) {
        this.column = this.el.nativeElement;
    }

    ngOnInit() {
        this.ref.detectChanges();
        if(this.resizable) {
            const row = this.column.parentNode;
            const thead = row.parentNode;
            this.table = this.renderer.parentNode(thead);
            //
            const resizer = this.column.querySelector(".icon-resize");
            if(resizer) {
               const columnListenerResize: Function = this.renderer.listen(resizer, "mousedown", this.onMouseDown);
                this.listenerFuncs.push(columnListenerResize);
                //
                const tableListener: Function = this.renderer.listen(this.table, "mousemove", this.onMouseMove);
                this.listenerFuncs.push(tableListener);
            }
        }
    }

    onMouseDown = (event: MouseEvent) => {
        event.preventDefault();
        this.onResize = true;
        this._onResize.emit({checked: this.onResize, element: this.column});
        this.startX = event.pageX;
        this.startWidth = this.column.offsetWidth;
        this.tableStartWidth = this.table.offsetWidth;
        // Tìm ra danh sách các ô của cột cần resize width dựa vào index của cột resize
        const trEl: Node = this.column.parentNode;
        const childNodes = Array.from(trEl.childNodes).filter(node => node instanceof HTMLElement);
        this.indexColumn = childNodes.findIndex(c => c === this.column);
        this.columnResizes = Array.from(this.table.querySelectorAll("tr")).map(el => {
            const childNodes = Array.from(el.childNodes).filter(node => node instanceof HTMLElement);
            return childNodes[this.indexColumn];
        });
    }

    onMouseMove = (event: MouseEvent) => {
        if(this.onResize) {
            const widthChange = event.pageX - this.startX;
            const resizeWidth = widthChange + this.startWidth;
            this.renderer.setStyle(this.table, 'width', this.tableStartWidth + widthChange + 'px')
            this.columnResizes.forEach((columnEl: HTMLElement) => {
                if(columnEl) {
                    // this.renderer.setStyle(columnEl, 'minWidth', resizeWidth+'px');
                    // this.renderer.setStyle(columnEl, 'maxWidth', resizeWidth+'px');
                    this.renderer.setStyle(columnEl, 'width', resizeWidth+'px');
                    this.renderer.setStyle(columnEl, 'borderRight', '2px solid #0000ff75');
                }
            });
            //
            if(event.buttons === 0) {
                this.onResize = false;
                this._onResize.emit({checked: this.onResize, element: this.column});
                if(this.columnResizes) {
                    this.columnResizes.forEach((columnEl: HTMLElement) => {
                        if(columnEl) {
                            this.renderer.removeStyle(columnEl, 'borderRight');
                        }
                    });
                }
            }
        }
    }

    // onMouseUp = (event: MouseEvent) => {
    //     if(this.onResize) {
    //         this.onResize = false;
    //     }
    // }

    ngOnDestroy() {
        for(const func of this.listenerFuncs) {
            func
        }
    }
}
