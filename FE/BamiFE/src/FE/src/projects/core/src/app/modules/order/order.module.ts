import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainOrderComponent } from './components/main-order/main-order.component';
import { OrderRoutingModule } from './order-routing.module';
import { CoreModule } from '../../core/core.module';
import { CreateOrUpdateOrderComponent } from './components/main-order/create-or-update-order/create-or-update-order.component';
import { CreateOrUpdateOrderDetailComponent } from './components/main-order/create-or-update-order-detail/create-or-update-order-detail.component';
import { MainOrderDetailComponent } from './components/main-order-detail/main-order-detail.component';
import { TreeModule } from 'primeng/tree';
import { ContextMenuModule } from 'primeng/contextmenu';
import { UpdateOrderDetailComponent } from './components/main-order-detail/update-order-detail/update-order-detail.component';
import { PrimeNGModuleBase } from '@shared/primeng-module-base.ts/primeng-module-base';
import { OrderItemDetailComponent } from './components/order-item-detail/order-item-detail.component';



@NgModule({
	declarations: [
		MainOrderComponent,
		CreateOrUpdateOrderComponent,
		CreateOrUpdateOrderDetailComponent,
		MainOrderDetailComponent,
		UpdateOrderDetailComponent,
		OrderItemDetailComponent
	],
	imports: [
		CommonModule,
		OrderRoutingModule,
		CoreModule,
		TreeModule,
		ContextMenuModule,
		PrimeNGModuleBase
	]
})
export class OrderModule { }
