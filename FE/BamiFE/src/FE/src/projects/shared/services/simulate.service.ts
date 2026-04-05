
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
	providedIn: 'root'
})

export class SimulateService {

	constructor() {
	}

	fakeApi(timer: number = 500) {
		return new Observable(subcriber => {
			const timeoutId = setTimeout(() => {
				subcriber.next(`response with timer ${timer}`)
				subcriber.complete();
			}, timer);
			// const value = value?.id ?? value ?? '-';
			//
			return () => {
				clearTimeout(timeoutId);
			}
		})
	}
}