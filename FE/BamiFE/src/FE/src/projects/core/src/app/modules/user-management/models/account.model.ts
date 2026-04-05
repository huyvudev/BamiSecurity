import { ESex } from "@mylib-shared/consts/base.consts";

export interface IAccount {
	username: string;
	password: string;
	isPasswordTemp: boolean;
	fullName: string;
	avatarS3key: string;
	gender: ESex;
	email: string;
	phoneNumber: string;
	dateOfBirth: Date;
	roleIds: number[];
}

export interface IAccountUpdate extends IAccount {
	id: number;
}

export interface IAccountItemList extends IAccountUpdate {
	//
	status: number;
	createdDate: Date;
	createdBy: string;
	avatarImageUri: string;
	s3Key: string;

}