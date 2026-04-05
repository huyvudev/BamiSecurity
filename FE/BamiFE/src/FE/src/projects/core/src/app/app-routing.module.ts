import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppMainComponent } from './core/layout/main/app.main.component';
import { AuthGuard } from '../../../shared/guard/auth.guard';
import { PageNotFoundComponent } from 'projects/my-lib/src/public-api';
import { PermissionGuard } from 'projects/shared/guard/permission.guard';

const routes: Routes = [
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    {
        path: 'auth',
        canActivate: [],
        loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule),
    },
    {
        path: '',
        component: AppMainComponent,
        canActivate: [PermissionGuard],
        children: [
            {
                path: '',
                loadChildren: () => import('./core/core.module').then(m => m.CoreModule),
            },
            {
                path: 'user-management',
                loadChildren: () => import('./modules/user-management/user-management.module').then(m => m.UserManagementModule),
            },
            {
                path: 'partner-management',
                loadChildren: () => import('./modules/partner/partner.module').then(m => m.PartnerModule),
            },
            {
                path: 'labels',
                loadChildren: () => import('./modules/labels/labels.module').then(m => m.LabelsModule),
            },
            {
                path: 'baskets',
                loadChildren: () => import('./modules/baskets/baskets.module').then(m => m.BasketsModule),
            },
            {
                path: 'batch-management',
                loadChildren: () => import('./modules/batch/batch.module').then(m => m.BatchModule),
            },
            {
                path: 'order-management',
                loadChildren: () => import('./modules/order/order.module').then(m => m.OrderModule),
            },
            {
                path: 'sku-management',
                loadChildren: () => import('./modules/sku-management/sku-management.module').then(m => m.SkuManagementModule),
            },
            {
                path: 'name-space',
                loadChildren: () => import('./modules/name-space/name-space.module').then(m => m.NameSpaceModule),
            },
            {
                path: 'status-management',
                loadChildren: () => import('./modules/update-status-item/update-status-item.module').then(m => m.UpdateStatusItemModule),
            },
            {
                path: 'manager-file',
                loadChildren: () => import('./modules/manager-file/manager-file.module').then(m => m.ManagerFileModule),
            },
        ]
    },

    { path: '**', component: PageNotFoundComponent },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})

export class AppRoutingModule { }
