import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'input-dropdown-search',
    templateUrl: './input-dropdown-search.component.html',
    styleUrls: ['./input-dropdown-search.component.scss'],
})
export class InputDropdownSearchComponent {  

    @Input() filterArray: any[];
    @Input() dataFilter: {
        keyword: string;
        keywordType: string;
    };
    @Input() optionLabel: string = "name";
    @Input() optionValue: string = "field";

    @Output() _onChange = new EventEmitter<object>();

    onChange() {
        const emitValue = {
            [this.dataFilter.keywordType]: this.dataFilter.keyword
        };
        this._onChange.emit(emitValue);
    }
}
