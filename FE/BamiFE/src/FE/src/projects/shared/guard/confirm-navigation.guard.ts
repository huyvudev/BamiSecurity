import { Component, HostListener, Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from "@angular/router";
import { LibHelperService, NavigationConfirmService } from "projects/my-lib/src/public-api";
import { Observable, Subscription, SubscriptionLike } from "rxjs";
import { Location } from "@angular/common";
import { DynamicDialogRef } from "primeng/dynamicdialog";
import { IConfirmNavigation } from "projects/my-lib/src/lib/shared/interfaces/base.interface";

@Injectable({
  providedIn: 'root',
})

export class ConfirmNavigationGuard {
    constructor(
		private router: Router,
		private _libService: LibHelperService,
		private _navigationConfirmService: NavigationConfirmService,
		private location: Location,
	) {
		
	}

	isShowConfirmNavigate: boolean = false;
	statusConfirmSub: Subscription;
	locationSub: SubscriptionLike;

    canDeactivate(component: Component, currentRoute: ActivatedRouteSnapshot, currentState: RouterStateSnapshot, nextState: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
		let isConfirmNavigation = false;
		const currentUrl = component?.['currentUrlInit'] || currentState.url;
		
		if(component['isSkipConfirmNavigation'] && !this.isShowConfirmNavigate) {
			return true;
		}

		if(component?.['isBackBrowser']) {
			this.location.go(currentUrl);
			if(this.isShowConfirmNavigate && component['isSkipConfirmNavigation'] !== undefined) {
				component['isSkipConfirmNavigation'] = true;
			}
			component['isBackBrowser'] = false;	// reset status event back browser
		}


		if(this.isShowConfirmNavigate) {
			return false;
		}
		//
		return new Promise<boolean>((resolve) => { 
			let messageConfirm = 'Bạn chắc chắn muốn hủy thao tác?';
			if(!this.isShowConfirmNavigate) {
				if(this.statusConfirmSub) this.statusConfirmSub.unsubscribe();
				this._navigationConfirmService.activeCheckConfirm();
				//
				this.statusConfirmSub = this._navigationConfirmService.getDataConfirm.subscribe((res: IConfirmNavigation) => {
					console.log('checkConfirm', res);
					if(res?.active !== undefined) isConfirmNavigation = !!res?.active;
					if(isConfirmNavigation) {
						this.isShowConfirmNavigate = true;
						this._libService.dialogConfirmRef(messageConfirm).onClose
							.subscribe((res) => {
								this.isShowConfirmNavigate = false;
								resolve(!!res?.accept);
								console.log('passCheckNavigationConfirm', !!res?.accept, component['isSkipConfirmNavigation']);
								// Xử lý case đang hiện popup mà click tiếp vào button back của trình duyệt
								if(res?.accept) {
									component['currentUrlInit'] = undefined;
									if(component['isSkipConfirmNavigation']) {
										this.router.navigateByUrl(nextState.url);
									}
								} else {
									this._navigationConfirmService.resetMenu();
									component['isSkipConfirmNavigation'] = false;
									setTimeout(() => this.location.go(currentUrl));
								}
							}
						)
						//
					} else {
						setTimeout(() => resolve(true));
					}
				})
			} else {
				this._navigationConfirmService.activeCheckConfirm(false);
			}
		});
    }
}
