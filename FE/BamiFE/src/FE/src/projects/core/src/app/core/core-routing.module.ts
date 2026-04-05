import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from '../../../../shared/guard/auth.guard';

const routes: Routes = [
  {
    path: '',
    children: [
      	{ path: 'home', component: HomeComponent, canActivate: [AuthGuard] }
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class CoreRoutingModule { }
