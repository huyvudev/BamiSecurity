import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoreModule } from '../../core/core.module';
import { StatusUpdateComponent } from './components/status-update/status-update.component';
import { UpdateStatusRoutingModule } from './update-status-item-routing.module';

@NgModule({
	declarations: [
    StatusUpdateComponent
  ],
	imports: [
		CommonModule,
		UpdateStatusRoutingModule,
		CoreModule
	]
})
export class UpdateStatusItemModule { }