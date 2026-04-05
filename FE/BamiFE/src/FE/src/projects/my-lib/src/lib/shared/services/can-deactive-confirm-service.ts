import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class CanDeactiveConfirmService {

    constructor() {}

    // Xử lý isLoading khi upload hoặc move ảnh
    private canDeactive$ = new BehaviorSubject(null);

    public get changeCanDeactive(): Observable<any> {
        return this.canDeactive$.asObservable();
    }

    public activeConfirm(data?: any) {
        this.canDeactive$.next(data);
    }
}
