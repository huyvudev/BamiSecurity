import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainLabelsComponent } from './components/main-labels.component';
import { LablesRoutingModule } from './lables-routing.module';
import { CoreModule } from '../../core/core.module';



@NgModule({
  declarations: [
    MainLabelsComponent
  ],
  imports: [
    CommonModule,
    LablesRoutingModule,
    CoreModule
  ]
})
export class LabelsModule { }
