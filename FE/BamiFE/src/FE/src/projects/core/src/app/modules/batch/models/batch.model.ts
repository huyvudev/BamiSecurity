import { OrderItem } from "../../order-item/models/order-item.model";

export class Batch {
    id: number;
    creatorName: string;
    priority: number;
    partnerId: number;
    nameBatch?: NameBatch;
    partnerName: string;
    status: number;
    printDate: string;
    cutDate: string;
    engravedDate: string;
    finishDate: string;
    createdDate: string;
    sku?: string;
}
export class NameBatch {
    code?: string;
    listStatus?: any
}


export class ItemOrder {
    id: number;
    itemIndex: number;
    orderId: number;
}

export class BatchDetail extends Batch {

    orderItems?: OrderItem[]

}