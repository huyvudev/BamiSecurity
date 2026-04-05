import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainSkuComponent } from './components/main-sku/main-sku.component';
import { MaterialComponent } from './components/material/material.component';
import { MainSkuBaseComponent } from './components/main-sku-base/main-sku-base.component';
import { ProductMethodComponent } from './components/product-method/product-method.component';
import { CreateOrUpdateSkuComponent } from './components/main-sku/create-or-update-sku/create-or-update-sku.component';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: 'sku', component: MainSkuComponent },
            { path: 'sku/create', component: CreateOrUpdateSkuComponent },
            { path: 'sku/detail/:id', component: CreateOrUpdateSkuComponent },
            { path: 'material', component: MaterialComponent },
            { path: 'base-sku', component: MainSkuBaseComponent },
            { path: 'product-method', component: ProductMethodComponent },
        ],
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class SkuManagementRoutingModule { }
