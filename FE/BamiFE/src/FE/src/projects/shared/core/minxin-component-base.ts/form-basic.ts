import { inject } from "@angular/core";
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ValidatorFn } from "@angular/forms";
import { ErrorMessage } from "@mylib-shared/consts/base.consts";
import { Utils } from "@mylib-shared/utils";
import { MessageService } from "primeng/api";
//
type Contructor<T = {}> = new (...args: any[]) => T;
//
export function MixinFormBasic<T extends Contructor>(Base: T) {
	return class extends Base {
		constructor(...args: any []) {
			super(...args)
		}

		protected fb: FormBuilder = inject(FormBuilder);
		private messageService: MessageService = inject(MessageService);
		
    	form: FormGroup;
		isEdit: boolean = false;
		isLoading: boolean = false;

		isGranted(permissionNames: any): boolean {
			return true;
		}
	
		protected addControl(name: string, value: any = null, validators?: ValidatorFn) {
			this.form.addControl(name, new FormControl(value, validators));
		}
	
		control(name: string) {
			return this.form?.get(name);
		}
	
		formValue(names?: string[]) {
			if(Array.isArray(names) && names?.length) {
				return this.form?.get(names).value;
			}
			return this.form?.value;
		}
	
		formRawValue() {
			return this.form?.getRawValue();
		}
	
		getControls(form = this.form) {
			return Object.keys(form?.controls);
		}
	
		inValidForm(formGroup: FormGroup = this.form, skipValidationForFields: string[] = []): boolean {
			// Duyệt qua tất cả các controls trong formGroup
			this.markControls(formGroup, skipValidationForFields);
	
			// Duyệt qua các controls trong FormArray
			Object.keys(formGroup.controls).forEach(key => {
				const control = formGroup.controls[key];
				if (control instanceof FormArray) {
					control.controls.forEach((controlItem: FormGroup | AbstractControl) => {
						// Gọi hàm markControls cho mỗi control trong FormArray
						if (controlItem instanceof FormGroup) {
							this.markControls(controlItem, skipValidationForFields);
						}
					});
				}
			});
	
			// Kiểm tra xem có bất kỳ control nào INVALID không
			let isInvalid = false;
			Object.keys(formGroup.controls).forEach(key => {
				if (formGroup.controls[key].status === "INVALID") {
					Utils.log(`invalid`, { controlName: key, controls: formGroup.controls[key] });
					isInvalid = true;
				}
			});

			if(isInvalid){
				this.messageService.add({ detail: ErrorMessage.ERR_INVALID_FORM, life: 3000, severity: 'error' });
			}
			return isInvalid;
		}

		protected markControls(formGroup: FormGroup, skipValidationForFields: string[] = []) {
			for (const i in formGroup.controls) {
				if (!skipValidationForFields.includes(i)) {
					formGroup.controls[i].markAsDirty();
				} else {
					formGroup.controls[i].clearValidators();
				}
				formGroup.controls[i].updateValueAndValidity({ emitEvent: false });
			}
		}

		activeEdit(fieldEnables: string[] = []) {
			if(this.form) {
				if(fieldEnables) {
					fieldEnables.forEach((fieldName) => {
						this.form.get(fieldName).enable()
					})
				} else {
					this.form.enable();
				}
			}
			//
			this.isEdit = true;
		}
	}
}