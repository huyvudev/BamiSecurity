export class OrderItem {
    id: number;
    itemIndex: any;
    template?: Template;
    status?: number;
    note?: string;
    orderId?: any;
    orderStatus?: number;
    orderDetailStatus?: number;
    orderNumber?: string;
    namespace?: string;
    size: any;
    code?: string;
    width?: number;
    length?: number;
    errorMessage?: number;
    mockUpFront: string;
    mockUpBack?: string;
    designFront?: string;
    designBack?: string;
    designSleeves?: string;
    designHood?: string;
    createdDate?: string;
    design?: Design;
    sizeOrderItem?:SizeOrderItem;
    idSku?:any;
}

export class Template {
    code?: string;
    orderNumber?: string;
    namespace?: string;
    listStatus?: any
}


export class Design {
    saleDesignFront?: string;
    saleDesignBack?: string;
    saleDesignSleeves?: string;
    saleDesignHood?: string;
}
export class SizeOrderItem {
    size?: any;
    width?: number;
    length?: number;
}
