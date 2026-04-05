export class RequestModel {
    requestNote: string;
}

export class ApproveModel {
    approveNote: string;
    cancelNote: string;
    approve: boolean = true;
}

export interface IPermissionConfig {
	key: string,
	parentKey: string,
	label: string,
	icon: string
}