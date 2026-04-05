import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { IConfirmNavigation } from "../interfaces/base.interface";

@Injectable({
  providedIn: 'root'
})

export class NavigationConfirmService {

    constructor() {}

    // Xử lý isLoading khi upload hoặc move ảnh
    private navigationConfirm$ = new BehaviorSubject(null);
    private checkConfirmObs$ = new BehaviorSubject(null);
    private getStatusConfirm$ = new BehaviorSubject(null);
    private resetMenu$ = new BehaviorSubject(null);

    get change(): Observable<any> {
        return this.navigationConfirm$.asObservable();
    }

    confirm(active: boolean = true, message?: string) {
		const data: IConfirmNavigation = { active: active, message: message};
        this.navigationConfirm$.next(data);
    }

	get checkConfirm(): Observable<boolean> {
        return this.checkConfirmObs$.asObservable();
    }

    activeCheckConfirm(status: boolean = true) {
        this.checkConfirmObs$.next(status);
    }

    pushData(status: boolean = false, message?: string) {
		const data: IConfirmNavigation =  { active: status, message: '' };
        this.getStatusConfirm$.next(data);
    }

	get getDataConfirm(): Observable<IConfirmNavigation>{
		return this.getStatusConfirm$.asObservable();
	}

    get cancelNavigation(): Observable<any> {
        return this.resetMenu$.asObservable();
    }

    resetMenu() {
        this.resetMenu$.next(null);
    }
}
