import { StatusResonse } from "../consts/base.consts";

export interface IResponseList<Item> {
    code: number;
    data: {
        items: Item[],
        totalItems: number,
    };
    message: string;
    status: StatusResonse;
}

export interface IResponseListNodeJS<Item> {
    limit: number;
    page: number;
    results: Item[];
    totalPages: number;
    totalResults: number;
}

export interface IResponseItem<Item> {
    code: number;
    data: Item;
    message: string;
    status: StatusResonse;
    successFE?: string;
}

export interface IResponseItemList<Item> {
    code: number;
    data: Item[];
    message: string;
    status: StatusResonse;
    successFE?: string;
}

export interface IResponseDialog<Data> {
    data: Data,
    accept: boolean,
}
