import { HttpClient, HttpParams } from "@angular/common/http";
import { MessageService } from "primeng/api";
import { LibHelperService } from "./lib-helper.service";
import { Inject, Injectable } from "@angular/core";
import { BehaviorSubject, Observable, catchError, finalize, forkJoin, map, of, timeout } from "rxjs";
import { BaseConsts } from "../consts/base.consts";
import { IEnvironment } from "../interfaces/environment.interface";

@Injectable({
    providedIn: 'root',
})
export class FileHelperService{
    constructor(
        public messageService: MessageService,
        public http: HttpClient,
		private _libHelper: LibHelperService,
        @Inject('env') environment
	) {
        this.environment = environment;
    }
    environment: IEnvironment;

    // Xử lý isLoading khi upload hoặc move ảnh
	private $isUploadMedia = new BehaviorSubject(false);

	public get IsUploadMediaObservable(): Observable<any> {
		return this.$isUploadMedia.asObservable();
	}

	public setUploadMedia(value?: boolean) {
		this.$isUploadMedia.next(value);
	}

    messageTimeout = 'Vui lòng kiểm tra lại đường truyền mạng. Thời gian phản hồi tối đa 15s!';
    uploadFile(body, folderFnc = ''): Observable<any> {
        this.setUploadMedia(true);
        let params = new HttpParams();
        params = params.set("enableMultileRequest", true);
		let folder = `${BaseConsts.folder}/${folderFnc}`;
        const formData = new FormData();
        for (const key in body) {
            if (body.hasOwnProperty(key) && body[key]) {
                formData.append(key, body[key]);
            }
        }
        return this.http.post(`${this.environment}/api/file/upload?folder=${folder}`, formData, {params}).pipe(
            timeout(10000),
            catchError(error => {
                if (!error?.error?.message) {
                    this._libHelper.messageError(this.messageTimeout)
                }
                return error;
            }),
            finalize(() => {
                this.setUploadMedia(false);
            })
        );
    }

    uploadFiles(body, folderFnc = ''){
        this.setUploadMedia(true);
        const url = `${this.environment}/api/file/uploads?folder=${folderFnc}`;
        return this.http.post<any>(url, body).pipe(
            timeout(10000),
            catchError(error => {
                if (!error?.error?.message) {
                    this._libHelper.messageError(this.messageTimeout)
                }
                return error;
            }),
            finalize(() => {
                this.setUploadMedia(false);
            })
        );
    }

    getImageFileFromUrl(imageUrl: string): Observable<(File | null)> {
        return this.convertImageUrlToFile(imageUrl);
    }

    getMultiImageFileFromUrl(imageUrls: string[]): Observable<(File | null)[]> {
        const observables = imageUrls.filter(item => item != null).map(imageUrl => this.convertImageUrlToFile(imageUrl));
        return forkJoin(observables)
    }

    private convertImageUrlToFile(imageUrl: string): Observable<File> {
        return this.http.get(`${this.environment.api}${imageUrl}&enableMultileRequest=true`, { responseType: 'blob' })
            .pipe(
                map(blob => {
                    const fileName = imageUrl.substring(imageUrl.lastIndexOf('/') + 1);
                    const file = new File([blob], fileName, { type: blob.type })
                    const objectURL = URL.createObjectURL(blob);
                    (file as any)['objectURL'] = objectURL;
                    return file;
                })
            );
    }
}