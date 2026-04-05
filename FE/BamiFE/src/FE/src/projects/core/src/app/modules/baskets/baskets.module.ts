import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainBasketsComponent } from './components/main-baskets.component';
import { BasketsRoutingModule } from './baskets-routing.module';
import { CoreModule } from '../../core/core.module';



@NgModule({
  declarations: [
    MainBasketsComponent
  ],
  imports: [
    CommonModule,
    BasketsRoutingModule,
    CoreModule
  ]
})
export class BasketsModule { }
