export class Order {
    id: number;
    name: string;
    address: string;
    city: string;
    state: string;
    postalCode: string;
    country: string;
    status: number;
    orderNumber: string;
    address2: string;
    phone: string;
    email: string;
    tax: string;
    namespace?: string;
    brandId?: number;
    details?: OrderDetail[];
}

export class OrderDetail {

    orderId?: number;
    id: number;
    type?: string; //Loại sản phẩm (ví dụ: T-shirt, Hoodie, Mug)
    title?: string; //Tiêu đề (ví dụ: Women Black T-shirt)
    size?: string; //  Kích cỡ của sản phẩm dạng chuỗi ví dụ 10in, 20in, 10x20in dùng cho lúc nhập dữ liệu
    sellerSku?: string;
    color?: string;
    quantity: number;
    status: number;

    errorMessage?: string;
   

    width: number;
    
    length: number;

    codeSku?: string;

   
    skuId?: number;

    mockUpFront?: string;
    mockUpBack?: string;

    saleDesignFront?: string;
    saleDesignBack?: string;
    saleDesignSleeves?: string;
    saleDesignHood?: string;

    designFront?: string;
    designBack?: string;
    designSleeves?: string;
    designHood?: string;

    designSale?: DesignSale;
    design?: Design;
}

export class DesignSale {
    saleDesignFront?: string;
    saleDesignBack?: string;
    saleDesignSleeves?: string;
    saleDesignHood?: string;
}


export class Design {
    designFront?: string;
    designBack?: string;
    designSleeves?: string;
    designHood?: string;
}

