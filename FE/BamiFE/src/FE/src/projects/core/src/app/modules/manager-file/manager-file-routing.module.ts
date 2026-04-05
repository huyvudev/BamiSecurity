
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainManagerFileComponent } from './components/main-manager-file/main-manager-file.component';



const routes: Routes = [
    {
        path: '', component: MainManagerFileComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ManagerFileRoutingModule { }
