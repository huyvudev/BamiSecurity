export class BatchCreate {
    id: number;
    creatorName: string;
    priority: number;
    partnerId: number;
    items: Item[];
}
export class Item {
    orderId: number;
    itemIndex: number;
    constructor(orderId: number, itemIndex: number) {
        this.orderId = orderId;
        this.itemIndex = itemIndex;
    }
}