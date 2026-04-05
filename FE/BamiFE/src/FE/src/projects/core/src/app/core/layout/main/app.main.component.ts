import { Component } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ConfirmationService, PrimeNGConfig } from 'primeng/api';
import { MenuService } from '../menu/app.menu.service';
import { AppComponent } from '../../../app.component';
import { LibHelperService, NavigationConfirmService } from 'projects/my-lib/src/public-api';
import { IResponseItem } from 'projects/my-lib/src/lib/shared/interfaces/response.interface';
import { TokenService } from 'projects/my-lib/src/lib/shared/services/auth/token.service';
import { environment } from 'projects/shared/environments/environment';
import { PermissionsService } from 'projects/my-lib/src/lib/shared/services/auth/permission.service';
import { ComponentMainBase } from 'projects/shared/component-main-base';

@Component({
    selector: 'app-main',
    templateUrl: './app.main.component.html',
    animations: [
        trigger('mask-anim', [
            state('void', style({
                opacity: 0
            })),
            state('visible', style({
                opacity: 0.8
            })),
            transition('* => *', animate('250ms cubic-bezier(0, 0, 0.2, 1)'))
        ])
    ],
    providers: [ConfirmationService]
})
//
export class AppMainComponent extends ComponentMainBase {

    rightPanelClick: boolean;

    rightPanelActive: boolean;

    menuClick: boolean;

    staticMenuActive: boolean = true;

    menuMobileActive: boolean;

    megaMenuClick: boolean;

    megaMenuActive: boolean;

    megaMenuMobileClick: boolean;

    megaMenuMobileActive: boolean;

    topbarItemClick: boolean;

    topbarMobileMenuClick: boolean;

    topbarMobileMenuActive: boolean;

    sidebarActive: boolean = true;

    activeTopbarItem: any;

    topbarMenuActive: boolean;

    menuHoverActive: boolean;

    configActive: boolean;

    isShowMenu: boolean = true;

    constructor(
        private menuService: MenuService,
        private primengConfig: PrimeNGConfig,
        public app: AppComponent,
		private _permissionService: PermissionsService,
		private _libHelpers: LibHelperService,
		private _tokenService: TokenService,
        private _navigationService: NavigationConfirmService,
    ) {
		super()
	}

    // isShow: boolean = false;
    permissions: string[];
    onInit: boolean = true;
    confirmQuestionUpdatePermission: boolean = false;
	env = environment;

    ngOnInit() {
        // this._permissionService.fetchPermission$.subscribe({
        //     next: (currentPermisison: string[]) => {
        //         if(!this.onInit) {
        //             this._permissionService.getPermissionInWeb(WebKeys.Core).subscribe({
		// 				next: (response: IResponseItem<any>) => {
		// 					if(this._libHelpers.checkStatusResponse(response)) {
		// 						const differenceCurrent = (currentPermisison || []).filter(p => !response?.['data'].includes(p));
		// 						const differenceRespone = (response?.['data'] || [])?.filter(p => !currentPermisison.includes(p));
		// 						if(differenceCurrent?.length || differenceRespone?.length) {
		// 							location.reload();
		// 						}
		// 					}
		// 				},
		// 				error: () => {
		// 					setTimeout(() => {
		// 						this._tokenService.clearToken();
		// 						window.location.href = `${this.env.urlAuthLogin}/authenticate/logout?returnUrl=${this.env.baseUrlCore}`;
		// 					}, 1500);
		// 				}
		// 			});
        //         }
        //         this.onInit = false;
        //     }
        // })
        // //
        // this._navigationService.cancelNavigation.subscribe(() => {
        //     this.isShowMenu = false;
        //     setTimeout(() => this.isShowMenu = true);
        // });
    }

    onLayoutClick() {
        if (!this.topbarItemClick) {
            this.activeTopbarItem = null;
            this.topbarMenuActive = false;
        }

        if (!this.rightPanelClick) {
            this.rightPanelActive = false;
        }

        if (!this.megaMenuClick) {
            this.megaMenuActive = false;
        }

        if (!this.megaMenuMobileClick) {
            this.megaMenuMobileActive = false;
        }

        if (!this.menuClick) {
            if (this.isHorizontal()) {
                this.menuService.reset();
            }

            if (this.menuMobileActive) {
                this.menuMobileActive = false;
            }

            this.menuHoverActive = false;
        }

        this.menuClick = false;
        this.topbarItemClick = false;
        this.megaMenuClick = false;
        this.megaMenuMobileClick = false;
        this.rightPanelClick = false;
    }

    onMegaMenuButtonClick(event) {
        this.megaMenuClick = true;
        this.megaMenuActive = !this.megaMenuActive;
        event.preventDefault();
    }

    onMegaMenuClick(event) {
        this.megaMenuClick = true;
        event.preventDefault();
    }

    onTopbarItemClick(event, item) {
        this.topbarItemClick = true;

        if (this.activeTopbarItem === item) {
            this.activeTopbarItem = null; } else {
            this.activeTopbarItem = item; }

        event.preventDefault();
    }

    onRightPanelButtonClick(event) {
        this.rightPanelClick = true;
        this.rightPanelActive = !this.rightPanelActive;

        event.preventDefault();
    }

    onRightPanelClose(event) {
        this.rightPanelActive = false;
        this.rightPanelClick = false;

        event.preventDefault();
    }

    onRightPanelClick(event) {
        this.rightPanelClick = true;

        event.preventDefault();
    }

    onTopbarMobileMenuButtonClick(event) {
        this.topbarMobileMenuClick = true;
        this.topbarMobileMenuActive = !this.topbarMobileMenuActive;

        event.preventDefault();
    }

    onMegaMenuMobileButtonClick(event) {
        this.megaMenuMobileClick = true;
        this.megaMenuMobileActive = !this.megaMenuMobileActive;

        event.preventDefault();
    }

    onMenuButtonClick(event) {
        this.menuClick = true;
        this.topbarMenuActive = false;

        // if (this.isMobile()) {
        //     this.menuMobileActive = !this.menuMobileActive;
        // }

        event.preventDefault();
    }

    onSidebarClick(event: Event) {
        this.menuClick = true;
    }

    onToggleMenuClick(event: Event) {
        this.staticMenuActive = !this.staticMenuActive;
        this.sidebarActive = !this.sidebarActive;
        if(!this.staticMenuActive) this.menuClick = false;
        event.preventDefault();
    }

    onRippleChange(event) {
        this.app.ripple = event.checked;
        this.primengConfig = event.checked;
    }

    isDesktop() {
        return window.innerWidth > 991;
    }

    isMobile() {
        return window.innerWidth <= 991;
    }

    isHorizontal() {
        return this.app.horizontalMenu === true;
    }

}
