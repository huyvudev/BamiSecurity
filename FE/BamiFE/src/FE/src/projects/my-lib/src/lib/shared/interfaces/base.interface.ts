import { EAcceptFile, EIconConfirm, EUserType } from "../consts/base.consts";

export interface IEConfirm {
    message: string,
    icon?: EIconConfirm,
    labelButton?: ILabelButton,
}

export interface IParamHandleDTO {
    idName?: string;
    isCheckNull?: boolean;
}

export interface IType {
    name?: string;
    code?: string | number;
}

export interface ITag extends IType {
    severity: string,
}

export interface IErrorCode {
    [key: number] : string,
}

export interface IDialogUploadFileConfig {
    folderUpload?: string,
    header?: string,
    width?:string,
    uploadServer?: boolean,
    multiple?: boolean,
    accept?: EAcceptFile | string,
    quantity?: number, // Số phần tử upload lấy từ phần tử đầu tiên
    previewBeforeUpload?: boolean,
    callback?: Function,
    chooseLabel?: string,
    domain?: string,
    //
    titleInput?: string,
    inputValue?: string,
    inputRequired?: boolean,
    isChooseNow?: boolean,
    isMove?: boolean,
	service?: any,
}

export interface IResponseDialogUpload<T> {
    inputData: string,
    fileUrls: T[], // string[] | File[]
}

export interface ILabelButton {
    accept: string;
    cancel: string;
}

export interface IDropdown{
    labelName: string,
    value: number | string
}

export interface IDialogConfirmConfig {
    header?: string,
    message?: string,
    width?: string,
    icon?: EIconConfirm,
    labelButton?: ILabelButton,
	labelAccept?: string,
	labelCancel?: string,
    isNoteConfirm?: boolean,
    dropdownData?: any[];
	//
	styleClass?: string,
	maskStyleClass?: string,
    createBy?: string,
    createDate?: string
}

export interface IConfirmNavigation{
    active?: boolean,
    message?: string
}

export interface IUserDecodeToken {
    client_id: string;
    trading_provider_ids: string;
    trading_provider_id: string;
    partner_id: string;
    username: string;
    display_name: string;
    user_type: EUserType;
    exp: number;
    iat: number;
    ip_address_login: string;
    iss: string;
    jti: string;
    oi_au_id: string;
    oi_prst: string;
    oi_tkn_id: string;
    scope: string;
    sub: string;
    userId: number;
}

export interface IRadioButtonValue {
    label: string,
    value: any,
}

export interface IEventSubmitForm {
    hasChangeData: boolean,
	originalData: any,
	actionType: 'create' | 'update' | 'confirm'
}
