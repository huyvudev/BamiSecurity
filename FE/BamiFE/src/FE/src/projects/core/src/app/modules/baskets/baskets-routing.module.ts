import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { MainBasketsComponent } from './components/main-baskets.component';


const routes: Routes = [
    {
        path: '',
        component: MainBasketsComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BasketsRoutingModule { }
