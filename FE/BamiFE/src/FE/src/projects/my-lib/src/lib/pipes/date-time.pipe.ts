import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'libDateTime'
})
export class DateTimePipe implements PipeTransform {

    constructor() {}

    transform(value: string, format = 'DD/MM/YYYY'): string {

        return (moment(value).isValid() && value) ? moment(value).format(format) : '';
    }

}
