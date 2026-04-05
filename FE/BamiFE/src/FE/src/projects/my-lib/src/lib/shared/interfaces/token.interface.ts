import { UserTypes } from "../consts/base.consts";

export interface ITokenResponse {
    access_token: string,
    expires_in: number,
    id_token: string,
    refresh_token: string,
    scope: string,
    token_type: string,
}

export interface ITokenDecode {
    "username": string,
    "sub": number,
    "iss": string,
    "name": string,
    "user_type": number,
    "user_id": number,
    "oi_prst": string,
    "oi_au_id": string,
    "client_id": string,
    "oi_tkn_id": string,
    "scope": string,
    "jti": string,
    "exp": number,
    "iat": number,
    "investor_id": number
}

export interface IUserTokenDecode {
	name: string,
	username: string,
	email: string,
	userId: number,
	userType: UserTypes,
}

