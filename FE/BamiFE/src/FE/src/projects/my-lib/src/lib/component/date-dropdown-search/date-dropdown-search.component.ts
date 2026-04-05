import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'lib-date-dropdown-search',
  templateUrl: './date-dropdown-search.component.html',
  styleUrls: ['./date-dropdown-search.component.scss']
})
export class DateDropdownSearchComponent {

    @Input() filterArray: any[];
    @Input() dataFilter: {
        date: Date;
        typeDate: string;
    };
    @Input() optionLabel: string = "name";
    @Input() optionValue: string = "field";

    @Output() _onChange = new EventEmitter<object>();

    onChange() {
        const emitValue = {
            [this.dataFilter.typeDate]: this.dataFilter.date
        };
        this._onChange.emit(emitValue);
    }
}
