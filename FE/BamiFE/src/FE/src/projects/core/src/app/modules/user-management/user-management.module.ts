import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountManagementComponent } from './components/account-management/account-management.component';
import { RoleManagementComponent } from './components/role-management/role-management.component';
import { AccountFormComponent } from './components/account-management/account-form/account-form.component';
import { RoleFormComponent } from './components/role-management/role-form/role-form.component';
import { CoreModule } from '../../core/core.module';
import { TreeModule } from 'primeng/tree';
import { ContextMenuModule } from 'primeng/contextmenu';
import { SetPasswordComponent } from './components/account-management/set-password/set-password.component';
import { UserManagementRoutingModule } from './user-management-routing.module';

@NgModule({
	declarations: [
		AccountManagementComponent,
		RoleManagementComponent,
		AccountFormComponent,
		RoleFormComponent,
  		SetPasswordComponent
	],
	imports: [
		CommonModule,
		UserManagementRoutingModule,
		CoreModule,
		TreeModule,
		ContextMenuModule
	]
})
export class UserManagementModule { }
