import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLabelsComponent } from './components/main-labels.component';


const routes: Routes = [
    {
        path: '',
        component: MainLabelsComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class LablesRoutingModule { }
