import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountManagementComponent } from './components/account-management/account-management.component';
import { AuthGuard } from '@shared/guard/auth.guard';
import { RoleManagementComponent } from './components/role-management/role-management.component';
import { MenuAccount, MenuRole } from '@shared/consts/permissionWeb/permission-key.const';

const routes: Routes = [
	{
		path: 'accounts',
		component: AccountManagementComponent,
		canActivate: [AuthGuard],
		data: {
			permissionKey: MenuAccount
		}
	},
	{
		path: 'roles',
		component: RoleManagementComponent,
		canActivate: [AuthGuard],
		data: {
			permissionKey: MenuRole
		}
	}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserManagementRoutingModule { }
