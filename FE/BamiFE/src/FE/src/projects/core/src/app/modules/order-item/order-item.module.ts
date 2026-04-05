import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainOrderItemComponent } from './components/main-order-item.component';
import { MyLibModule } from "../../../../../my-lib/src/lib/my-lib.module";
import { CoreModule } from '../../core/core.module';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';



@NgModule({
  declarations: [
    MainOrderItemComponent,
  ],
  imports: [
    CommonModule,
    MyLibModule,
    ButtonModule ,
    CalendarModule ,
    
    
],
  exports :[MainOrderItemComponent]
})
export class OrderItemModule { }
