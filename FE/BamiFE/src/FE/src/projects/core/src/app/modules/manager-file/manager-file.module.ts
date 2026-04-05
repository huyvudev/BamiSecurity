import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainManagerFileComponent } from './components/main-manager-file/main-manager-file.component';
import { ManagerFileRoutingModule } from './manager-file-routing.module';
import { CoreModule } from '../../core/core.module';



@NgModule({
	declarations: [
		MainManagerFileComponent
	],
	imports: [
		CommonModule,
		ManagerFileRoutingModule,
		CoreModule,
	]
})
export class ManagerFileModule { }
