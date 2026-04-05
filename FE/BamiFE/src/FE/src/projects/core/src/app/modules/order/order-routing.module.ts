import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainOrderComponent } from './components/main-order/main-order.component';
import { CreateOrUpdateOrderComponent } from './components/main-order/create-or-update-order/create-or-update-order.component';
import { MainOrderDetailComponent } from './components/main-order-detail/main-order-detail.component';
import { AuthGuard } from '@shared/guard/auth.guard';
import { ConfirmNavigationGuard } from '@shared/guard/confirm-navigation.guard';
import { UpdateOrderDetailComponent } from './components/main-order-detail/update-order-detail/update-order-detail.component';
import { OrderItemDetailComponent } from './components/order-item-detail/order-item-detail.component';



const routes: Routes = [
    {
        path: '',
        children: [
            {
                path: 'order',
                component: MainOrderComponent,
                canActivate: [AuthGuard]
            },
            {
                path: 'order/create',
                component: CreateOrUpdateOrderComponent,
                canActivate: [AuthGuard],
                canDeactivate: [ConfirmNavigationGuard]
            },
            {
                path: 'order/detail/:id',
                component: CreateOrUpdateOrderComponent,
                canActivate: [AuthGuard],
                canDeactivate: [ConfirmNavigationGuard]
            }

        ],
    },
    {
        path: '',
        children: [
            {
                path: 'detail-order', component: MainOrderDetailComponent,
                canActivate: [AuthGuard],
            },
            {
                path: 'detail-order/detail/:id', component: UpdateOrderDetailComponent,
                canActivate: [AuthGuard],
            }
        ]
    },
    {
        path: '',
        children: [
            {
                path: 'item/detail/:id', component: OrderItemDetailComponent,
                canActivate: [AuthGuard],
            },
           
        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class OrderRoutingModule { }
