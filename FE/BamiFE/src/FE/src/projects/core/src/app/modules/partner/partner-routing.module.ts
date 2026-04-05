import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainPartnerComponent } from './components/main-partner.component';
import { MainPartnerTypeComponent } from './components/main-partner-type/main-partner-type.component';


const routes: Routes = [
  {
    path: '',
    children: [
      { path: 'partner', component: MainPartnerComponent },
      { path: 'type-partner', component: MainPartnerTypeComponent }
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class PartnerRoutingModule { }
