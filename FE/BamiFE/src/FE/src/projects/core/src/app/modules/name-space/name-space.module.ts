import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainStoreComponent } from './components/main-store/main-store.component';
import { CoreModule } from '../../core/core.module';
import { NameSpaceRoutingModule } from './name-space-routing.module';
import { MainBrandComponent } from './components/main-brand/main-brand.component';
import { CreateOrUpdateBrandComponent } from './components/main-brand/create-or-update-brand/create-or-update-brand.component';



@NgModule({
	declarations: [
		MainStoreComponent,
		MainBrandComponent,
		CreateOrUpdateBrandComponent
	],
	imports: [
		CommonModule,
		NameSpaceRoutingModule,
		CoreModule,

	]
})
export class NameSpaceModule { }
