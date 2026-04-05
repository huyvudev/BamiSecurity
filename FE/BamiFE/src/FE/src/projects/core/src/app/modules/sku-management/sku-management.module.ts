import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainSkuComponent } from './components/main-sku/main-sku.component';
import { MainSkuBaseComponent } from './components/main-sku-base/main-sku-base.component';
import { ProductMethodComponent } from './components/product-method/product-method.component';
import { MaterialComponent } from './components/material/material.component';
import { SkuManagementRoutingModule } from './sku-management-routing.module';
import { CoreModule } from '../../core/core.module';
import { CreateOrUpdateMaterialComponent } from './components/material/create-or-update-material/create-or-update-material.component';
import { CreateOrUpdateSkuBaseComponent } from './components/main-sku-base/create-or-update-sku-base/create-or-update-sku-base.component';
import { CreateOrUpdateProductMethodComponent } from './components/product-method/create-or-update-product-method/create-or-update-product-method.component';
import { CreateOrUpdateSkuComponent } from './components/main-sku/create-or-update-sku/create-or-update-sku.component';
import { DialogService } from 'primeng/dynamicdialog';
import { CreateOrUpdateSkuSizeComponent } from './components/main-sku/create-or-update-sku-size/create-or-update-sku-size.component';
import { PrimeNGModuleBase } from '@shared/primeng-module-base.ts/primeng-module-base';




@NgModule({
  declarations: [
    MainSkuComponent,
    MainSkuBaseComponent,
    ProductMethodComponent,
    MaterialComponent,
    CreateOrUpdateMaterialComponent,
    CreateOrUpdateSkuBaseComponent,
    CreateOrUpdateProductMethodComponent,
    CreateOrUpdateSkuComponent,
    CreateOrUpdateSkuSizeComponent,
  ],
  imports: [
    CommonModule,
    SkuManagementRoutingModule,
    CoreModule,
    PrimeNGModuleBase

  ],
  providers: [
    DialogService
  ]
})
export class SkuManagementModule { }
