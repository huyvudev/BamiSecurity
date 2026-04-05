export interface IRole {
	name: string;
	description: string;
	permissionKeys: string[];
	permissionKeysRemove: string[];
}

export interface IRoleUpdate extends IRole {
	id: number;
}

export interface IRoleItemList extends IRoleUpdate {
	//
	status: number;
	createdDate: Date;
	createdByUserName: string;
	permissionDetails: any;
}