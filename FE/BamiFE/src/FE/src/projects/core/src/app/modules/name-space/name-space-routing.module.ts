
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainStoreComponent } from './components/main-store/main-store.component';
import { MainBrandComponent } from './components/main-brand/main-brand.component';
import { CreateOrUpdateBrandComponent } from './components/main-brand/create-or-update-brand/create-or-update-brand.component';



const routes: Routes = [
    {
        path: '',
        children: [
            { path: 'store', component: MainStoreComponent },
            { path: 'brand', component: MainBrandComponent },
            { path: 'brand/create', component: CreateOrUpdateBrandComponent },
            { path: 'brand/detail/:id', component: CreateOrUpdateBrandComponent },
        ],
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class NameSpaceRoutingModule { }
