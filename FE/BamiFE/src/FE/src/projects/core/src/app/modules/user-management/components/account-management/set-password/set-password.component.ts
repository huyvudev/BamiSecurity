import { Component, OnInit } from '@angular/core';
import { FormDialog } from '@shared/core/component-bases.ts/form-dialog';
import { AccountService } from '../../../services/account.service';
import { required } from '@shared/validators/validator-common';

@Component({
    selector: 'app-set-password',
    templateUrl: './set-password.component.html',
    styleUrls: ['./set-password.component.scss']
})
export class SetPasswordComponent extends FormDialog implements OnInit {

    constructor(
        private _accountService: AccountService,
    ) {
        super()
    }

    ngOnInit(): void {
        this.form = this.fb.group({
            id: this.dialogConfig.data.id,
            password: [null, required()]
        })
    }

    save() {
        if(!this.inValidForm()) {
            this._accountService.setPassword(this.formRawValue())
            .subscribe((res) => {
                if(this.checkStatusResponse(res, 'Cập nhật thành công!')) {
                    this.closeDialog();
                } 
            })
        }
    }
}
