import { Component, Inject, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '@mylib-shared/services/auth/token.service';
import { LibHelperService } from '@mylib-shared/services/lib-helper.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AppAuthService } from 'projects/my-lib/src/lib/shared/services/auth/app-auth.service';
import { mergeMap } from 'rxjs';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent {

    submitting = false;
    dark: boolean;
    environment: any

    isForgotPass: boolean = false

    user: {
        username: string,
        password: string
    }

    constructor(
        private authService: AppAuthService,
        private _tokenService: TokenService,
        private router: Router,
        private spinner: NgxSpinnerService,
        @Inject('env') environment
    ) {
        this.user = {
            username: '',
            password: ''
        }
        this.environment = environment
    }

    forgotPass() {
        this.isForgotPass = true
    }

    login(): void {
		this.spinner.show();
		this.authService.connectToken(this.user.username, this.user.password)
        .subscribe({
			next: (response) => { 
                window.location.href = '/home';
			},
			error: () => {
				this.spinner.hide();
				this._tokenService.clearToken(); 
				this.router.navigate(['auth/login']);
			}
		});
    }

    isMobile() {
        return window.innerWidth <= 991;
    }
}

