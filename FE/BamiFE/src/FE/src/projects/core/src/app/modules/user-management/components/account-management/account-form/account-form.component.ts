import { Component, OnInit } from '@angular/core';
import { FormDialog } from '@shared/core/component-bases.ts/form-dialog';
import { AccountService } from '../../../services/account.service';
import { finalize } from 'rxjs/operators';
import { emailValidator, phoneValidator, required, requiredArray, usernameValidator } from '@shared/validators/validator-common';
import { IEventSubmitForm } from '@mylib-shared/interfaces/base.interface';
import { Validators } from '@angular/forms';
import { SexConst } from '@mylib-shared/consts/base.consts';
import { RoleService } from '../../../services/role.service';
import { IRoleItemList } from '../../../models/role.model';

@Component({
	selector: 'app-account-form',
	templateUrl: './account-form.component.html',
	styleUrls: ['./account-form.component.scss']
})
export class AccountFormComponent extends FormDialog implements OnInit {

	constructor(
		private _selfService: AccountService,
		private _roleService: RoleService,
	) {
		super()
	}

	SexConst = SexConst;

	accountId: number;
	roles: IRoleItemList[] = [];

	ngOnInit(): void {
		//
		this.accountId = this.dialogConfig.data.id;
		this.getRoles();
		//
		this.form = this.fb.group({
			id: [this.accountId],
			username: [null, [required(), usernameValidator()]],
			userCode: 'userCode',
			password: [null, required()],
			isPasswordTemp: [true],
			fullName: [null, required()],
			email: [null, [required(), emailValidator()]],
			gender: [null, required()],
			phone: [null, [required(), phoneValidator()]],
			dateOfBirth: [null],
			roles: [[], requiredArray()],
		});
		//
		this.isEdit = true;
		if (!this.accountId) {
			this.form.removeControl('id');
		} else {
			this.form.removeControl('password');
			this.form.removeControl('isPasswordTemp');
			this.getAccount();
		}
		//
	}

	getAccount() {
		this._selfService.findById(this.accountId).subscribe((res) => {
			if(this.checkStatusResponse(res)) {
				console.log('res', res);
				this.form.patchValue(res?.data);
			}
		})
	}
	
	getRoles() {
		this._roleService.getAllNoPaging(this.destroyRef)
		.subscribe((res) => {
			if(this.checkStatusResponse(res)) {
				this.roles = res.data;
			}
		})
	}

	getRole() {
		this.isLoading = true;
		this._selfService.findById(this.accountId)
		.pipe(finalize(() => this.isLoading = false))
		.subscribe((res) => {
			if (this.checkStatusResponse(res)) {
				
			}
		})
	}

	save(event: IEventSubmitForm) {
		if(!this.inValidForm()) {
			if(event.hasChangeData || !this.accountId) {
				const body = this.formValue();
				if(!this.accountId) {
					this._selfService.create(body).subscribe((res) => {
						if(this.checkStatusResponse(res, 'Thêm thành công!')) {
							this.closeDialog(true)
						}
					})
				} else {
					this._selfService.update(body).subscribe((res) => {
						if(this.checkStatusResponse(res, 'Cập nhật thành công!')) {
							this.closeDialog(true)
						}
					})
				}
			} else {
				this.closeDialog();
			}
		}
	}
}
