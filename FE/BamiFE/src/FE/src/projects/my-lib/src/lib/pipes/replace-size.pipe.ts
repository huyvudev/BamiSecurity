import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'replaceSize'
})
export class ReplaceSizePipe implements PipeTransform {

  transform(value: string): string {
    return value.replace(',', 'x');  // Thay dấu phẩy bằng dấu x
  }

}
