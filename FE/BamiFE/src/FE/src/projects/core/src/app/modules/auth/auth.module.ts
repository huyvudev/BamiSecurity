import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AccountRoutingModule } from './auth-routing.module';
import { AccountComponent } from './auth.component';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ToastModule } from 'primeng/toast';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { NgxSpinner, NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        AccountRoutingModule,
        ButtonModule,
        InputTextModule,
        PasswordModule,
        ToastModule,
		NgxSpinnerModule
    ],
    declarations: [
        AccountComponent,
        LoginComponent,
        RegisterComponent,
        ForgotPasswordComponent,
    ],
    providers: [],
})

export class AuthModule {}
