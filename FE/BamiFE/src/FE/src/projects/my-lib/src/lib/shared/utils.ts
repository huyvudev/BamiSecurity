import * as moment from "moment";
import { BaseConsts, ETypeUrlYoutube } from "./consts/base.consts";
import { FormArray, FormGroup } from "@angular/forms";
import { IResponseItem } from "./interfaces/response.interface";
import { HttpParams } from "@angular/common/http";
import { Page } from "./models/page";

export class Utils {

    public static getSessionStorage(key: string) {
        return JSON.parse(sessionStorage.getItem(key))
    }

    public static setSessionStorage(key: string, data: any,) {
        sessionStorage.setItem(key, JSON.stringify(data));
    }

    public static removeSessionStorage(key: string) {
        sessionStorage.removeItem(key);
    }

    public static clearSessionStorage() {
        sessionStorage.clear();
    }

    // LOCAL STORAGE
    public static getLocalStorage(key: string) {
        return JSON.parse(localStorage.getItem(key))
    }

    public static setLocalStorage(key: string, data: any) {
        localStorage.setItem(key, JSON.stringify(data));
    }

    public static removeLocalStorage(key: string) {
        localStorage.removeItem(key);
    }

    public static clearLocalStorage() {
        localStorage.clear();
    }

    public static cloneData(data) {
        return JSON.parse(JSON.stringify(data));
    }

    public static formatDateMonth(value) {
        return (moment(value).isValid() && value) ? moment(value).format('DD/MM') : '';
    }

    public static formatDate(value) {
        return (moment(value).isValid() && value) ? moment(value).format('DD/MM/YYYY') : '';
    }

    public static convertLowerCase(string: string = '') {
        if (string.length > 0) {
            return string.charAt(0).toLocaleLowerCase() + string.slice(1);
        }
        return '';
    }

    /**
     * tạo một thẻ a download file
     * @param fileName tên file
     * @param href đường dẫn
     */

    public static makeDownload(body, url: string, type?: string, name?: string) {
        const contentDisposition = body?.headers.get('Content-Disposition');
        const fileName = name || this.getFileNameFromContentDisposition(contentDisposition) || this.truncatedString(url.split('/').pop(), 100);
        const blob = new Blob([body?.body], { type: type || body?.headers.get('Content-Type') });
        const a = document.createElement('a');
        a.href = window.URL.createObjectURL(blob);
        a.download = fileName;
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
    }

    private static getFileNameFromContentDisposition(contentDisposition: string | null): string | null {
        if (contentDisposition) {
            const matches = contentDisposition.match(/filename\*?=['"]?UTF-8''([^"'\s]+)['"]?/);

            if (matches && matches.length > 1) {
                return decodeURIComponent(matches[1]);
            }
        }
        return null;
    }

    private static truncatedString(originalString, maxLength) {
        const truncatedString = originalString.length > maxLength ? originalString.slice(-maxLength) : originalString;
        return truncatedString;
    }


    public static isPdfFile(file) {
        var parts = file.split('.');
        var typeFile = parts[parts.length - 1];
        switch (typeFile.toLowerCase()) {
            case 'pdf':
                return true;
        }
        return false;
    }

    public static replaceAll(str, find, replace) {
        var escapedFind = find.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
        return str.replace(new RegExp(escapedFind, 'g'), replace);
    }

    public static transformMoney(num: number, ...args: any[]): string {
        const value = `${num}`;
        if (value === '' || value === null || typeof value === 'undefined') {
            return '';
        }

        let locales = 'vi-VN';
        const cur = Number(value);

        if (args.length > 0) {
            locales = args[0];
        }

        const result = new Intl.NumberFormat(locales).format(cur);
        return result === 'NaN' ? '' : result;
    }

    public static transformPercent(num: number, ...args: any[]): string {
        const value = `${num}`;
        if (value === '' || value === null || typeof value === 'undefined') {
            return '';
        }

        let locales = 'vi-VN';
        const cur = Number(value);

        if (args.length > 0) {
            locales = args[0];
        }

        const result = new Intl.NumberFormat(locales).format(cur);
        return result === 'NaN' ? '' : this.replaceAll(result, '.', ',');
    }

    // BLOCK REQUEST API AFTER 3s
    public static doubleClickRequest(url): boolean {
        setTimeout(() => {
            this.removeLocalStorage(url);
        }, 3000);
        //
        const beforeRequestTime = Utils.getLocalStorage(url);
        if (((new Date().getTime() - +beforeRequestTime) < 1500) && beforeRequestTime) {
            return true;
        }

        this.setLocalStorage(url, new Date().getTime());
        //
        return false;
    }

    public static countDifferencePageHeight(): number {
        const diffenceScreenHeight = window.innerHeight - document.body.offsetHeight;
        const layoutContentEl = document.getElementById("layout-content");
        const pageContentEl = document.getElementById(BaseConsts.pageContentId);

        let diffenceContentHeight: number = 0;
        if (layoutContentEl && pageContentEl) {
            diffenceContentHeight = layoutContentEl.offsetHeight - pageContentEl.offsetHeight;
        }
		//
        return diffenceScreenHeight || diffenceContentHeight;
    }

    public static checkLinkYoutube(link: string) {
        let isCheck: boolean = false;
        const urlYoutubes = {
            ...ETypeUrlYoutube,
            ORIGIN: "https://www.youtube.com"
        }
        //
        try {
            if (typeof link === 'string') {
                for (const [key, url] of Object.entries(urlYoutubes)) {
                    isCheck = link?.includes(url);
                    if (isCheck) break;
                }
            }
            //
            return isCheck;
        } catch (error) {
            this.log('checkLinkYoutube', error);
            return false;
        }
    }

    public static log(titleError: string, error?: any) {
        console.log(`%c ${titleError} `, 'background:black; color: red; height: 20', error);
    }

    public static isExtensionImage(path: string) {
        const extension = path.split('.').pop();
        return BaseConsts.imageExtensions.includes(extension.toLowerCase());
    }

    public static isExtensionVideo(path: string) {
        const extension = path.split('.').pop();
        return BaseConsts.videoExtensions.includes(extension);
    }

    public static convertParamUrl(name: string, value: number | string | boolean) {
        return name + "=" + encodeURIComponent("" + value) + "&";
    }

    public static hidePhone(phone: any, index: any, replacement: any) {
        let result;
        if (phone) {
            result = phone.substring(0, index) + replacement + phone.substring(index + replacement.length)
            return result
        } else {
            return '';
        }
    }

    public static counter(i: number) {
        i = Math.ceil(i) || 5;
        return new Array(i);
    }

    public static makeRandom(lengthOfCode: number = 100, possible?: string) {
        possible = "AbBCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890-_";
        let text = "";
        for (let i = 0; i < lengthOfCode; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }
        //
        return text;
    }

    public static convertFormGroupToModel(source, target) {
        for (const key in target) {
            if (typeof target[key] === 'object') {
                for (const keyChild in target[key]) {
                    if (typeof source[keyChild] === 'string') {
                        target[key][keyChild] = source[keyChild].trim();
                    } else {
                        target[key][keyChild] = source[keyChild];
                    }
                }
            } else if (typeof source[key] === 'string') {
                target[key] = source[key].trim();
            } else {
                target[key] = source[key];
            }
        }
    }

    public static compareData(data1, data2): boolean {
        return JSON.stringify(data1) === JSON.stringify(data2);
    }

    // So sánh 2 data truyền vào nếu truyền fieldCompare thì so sánh theo fieldCompare nếu không tìm những trường chung của 2 data rồi so sánh
    public static compareDataByField(data1, data2, fieldCompare?: string[]): boolean {
        // Lấy danh sách các trường chung của cả hai đối tượng
        const commonFields = fieldCompare ?? Object.keys(data1).filter(field => Object.keys(data2).includes(field));

        // Lặp qua từng trường chung để so sánh
        for (const field of commonFields) {
            if (data1[field] !== data2[field]) {
                return false;
            }
        }
        return true;
    }

    // Truyền 2 data so sánh 2 data trả về mảng trường có giá trị khác nhau
    public static getDifferenceFields(data1, data2) {
        let differenceFields: string[] = [];
        for (const key in data1) {
            if (data1.hasOwnProperty(key) && data2.hasOwnProperty(key)) {
                if (data1[key] !== data2[key]) {
                    differenceFields.push(key);
                }
            }
        }
        return differenceFields;
    }

    public static trimFormGroup(formGroup: FormGroup): void {
        for (const controlName in formGroup.controls) {
            const control = formGroup.get(controlName);
            if (control && control.value && typeof control.value === 'string') {
                control.setValue(control.value.trim());
            }
        }
    }

    public static removeVietnameseTones(str: string, optionMore: { upperCase: boolean, lowerCase: boolean }): string {
        let result = str.normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .replace(/[đĐ]/g, 'd');
        //
        if (optionMore.upperCase) result = result.toUpperCase();
        if (optionMore.lowerCase) result = result.toLowerCase();
        //
        return result;
    }

    // thay thế ký tự có dấu thành không dấu
    static removeVietnamese(str, isKeepCase = false, isTrimSpace = false) {
        if (!isKeepCase) {
            str = str.toLowerCase();
        }
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");

        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
        str = str.replace(/Đ/g, "D");

        str = str.replace(/\s+/g, isTrimSpace ? '' : ' ');
        return isKeepCase ? str : str.toUpperCase();
    }

    // Sử dụng api trả về file bob pdf xử lý để có thể view được file pdf
    static viewWordConvertToPdf(response) {
        const blob = new Blob([response?.body], { type: 'application/pdf' });
        const url = URL.createObjectURL(blob);
        window.open(url, "_blank");
    }

    /**
     * loại bỏ ký tự emojis
     * @param str 
     * @returns 
     */
    public static removeEmojis(str: string): string {
        if (str?.length > 0) {
            let result = str.replace(/[\u{1F600}-\u{1F6FF}\u{1F300}-\u{1F5FF}\u{1F680}-\u{1F6FF}\u{1F700}-\u{1F77F}\u{1F780}-\u{1F7FF}\u{1F800}-\u{1F8FF}\u{1F900}-\u{1F9FF}\u{1FA00}-\u{1FA6F}\u{2600}-\u{26FF}\u{2700}-\u{27BF}]/gu, '')
            return result;
        }
        return '';
    }

    /**
     * chuyển về số điện thoại việt nam
     * @param phoneNumber 
     * @returns 
     */
    public static convertToVietNamesePhone(phoneNumber: string = ''): string {
        const phoneLength = 11;
        const countryCode = '84';
        const digitsOnly = phoneNumber?.replace(/\D/g, '');

        if (digitsOnly?.length === phoneLength) {
            if (digitsOnly.startsWith(countryCode)) {
                return '0' + digitsOnly.slice(2);
            } else if (!digitsOnly.startsWith('0')) {
                return digitsOnly;
            }
        }
        return phoneNumber;
    }

    public static getNameFile(url) {
        if (url) {
            return url.split('/').pop();
        }
        return '';
    }

    public static checkInputNumber(postForm: FormGroup, fieldName: string, value: number, maxLength: number = 15, maxValue?: number) {
        const stringValue: string = Math.floor(value).toString();
        const length: number = stringValue.length;
        if (length > maxLength) {
            const truncatedValue: number = +stringValue.substring(0, maxLength);
            postForm.patchValue({
                [fieldName]: truncatedValue
            })
            return postForm;
        } else if (maxValue && (value > maxValue)) {
            postForm.patchValue({
                [fieldName]: maxValue
            })
            return postForm;
        }
        return postForm;
    }

    public static checkInputNumberFormArray(postForm: FormGroup, index: number, groupName: string, fieldName: string, value: number, maxLength: number = 15, maxValue?: number) {
        const detailArray = postForm.get(groupName) as FormArray;
		const detailGroup = detailArray.at(index) as FormGroup;
        
        const stringValue: string = Math.floor(value).toString();
        const length: number = stringValue.length;
        if (length > maxLength) {
            const truncatedValue: number = +stringValue.substring(0, maxLength);
            detailGroup.patchValue({
                [fieldName]: truncatedValue
            });
            return postForm;
        } else if (maxValue && (value > maxValue)) {
            detailGroup.patchValue({
                [fieldName]: maxValue
            });
            return postForm;
        }
        return postForm;
    }

    public static formatCurrency(value: number, locales: string = 'vi-VN'): string {
        if (value === null || typeof value === 'undefined') {
            return '';
        }

        const cur = Number(value);
        const result = new Intl.NumberFormat(locales).format(cur);

        return result === 'NaN' ? '' : result;
    }

    public static formatNameDashboard(name: string, max: number) {
        const lines = name.split(' ');
        let result = '';
        for (let i = 0; i < lines.length; i++) {
            const word = lines[i];
            if (word.length > max) {
                result += '\n' + word + ' ';
            } else {
                if (result.split('\n').pop().length + word.length <= max) {
                    result += word + ' ';
                } else {
                    result += '\n' + word + ' ';
                }
            }
        }
        return result;
    }

    public static convertPriceDisplay(value) {
        if (value >= 1000000000) {
            return Utils.transformMoney(value / 1000000000) + 'T';
        } else if (value >= 1000000) {
            return Utils.transformMoney(value / 1000000) + 'Tr';
        } else if (value >= 1000) {
            return Utils.transformMoney(value / 1000) + 'N';
        } else {
            return value;
        }
    }

    public static changeUpperCase(postForm, controlName) {
        const inputElement = document.getElementById(controlName) as HTMLInputElement;
        if (inputElement) {
            // Lưu vị trí con trỏ hiện tại
            const start = inputElement.selectionStart;
            const end = inputElement.selectionEnd;
            postForm.patchValue({
                [controlName]: postForm?.value[controlName]?.toUpperCase()
            });
            // Khôi phục vị trí con trỏ
            inputElement.setSelectionRange(start, end);
        }
    }

    public static checkPermissionExist(params: IResponseItem<Record<string, boolean>>, checkPermissionTableContract: string) {
        return params?.data?.[checkPermissionTableContract]
    }

    public static setParamGetList(page?: Page, dataFilter: any = {}, excludeFields?: string[]): HttpParams {
        if(!dataFilter) dataFilter = {};
        let defaultExcludeFields = ["keywordType", "keyword", ...(excludeFields ?? [])];
        let params = new HttpParams();
		if(page) {
			params = params.set('pageSize', page.getPageSize())
			params = params.set('pageNumber', page.getPageNumber(dataFilter))
		}
		//
        if (dataFilter?.keyword) params = params.set(dataFilter.keywordType ?? "keyword", dataFilter.keyword);
        for (const [key, value] of Object.entries(dataFilter)) {
            if ((value || value === false || value === 0) && defaultExcludeFields.every((value) => key !== value)) {
				if(Array.isArray(value) && value.length) {
					value.forEach((item) => {
						if(['boolean', 'string', 'number'].includes(typeof(item))) {
							params = params.append(key, item);
						}
					})
				} else {
					params = params.set(key, dataFilter[key]);
				}
            } 
        }
        return params;
    }

	public static valueExist(value): boolean {
		if(Array.isArray(value)) {
			return !!value.length;
		} else {
			return !!(value || value === 0);
		}
		
	}
}

