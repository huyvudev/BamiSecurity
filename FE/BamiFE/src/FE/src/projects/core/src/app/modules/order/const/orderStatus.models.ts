export enum EStatusOder {
    PENDING_IMPORTED = 0,
    PUSHED_ORDER = 1,
    PENDING_DESIGN = 2,
    READY_FOR_PRODUCTION = 3,
    IN_PRODUCTION = 4 ,
    PACKAGED = 5,
    CANCELED = 6,
}

export class StatusOrder {
    public static status = [
        {
            name: "Chờ xử lý",
            code: 0
        },
        {
            name: "Đã đẩy",
            code: 1
        },
        {
            name: "Đang chờ thiết kế",
            code: 2
        },
        {
            name: "Chuẩn bị cho sản xuất",
            code: 3
        },
        {
            name: "Đang sản xuất",
            code: 4
        },
        {
            name: "Đã đóng gói",
            code: 5
        },
        {
            name: "Đã hủy",
            code: 6
        },
    ];
    public static ListStatusOrderItem = [{
        name: "Chờ xử lý",
        code: EStatusOder.PENDING_IMPORTED,
        class: 'tag-pending'
    },
    {
        name: "Đã đẩy",
        code: EStatusOder.PUSHED_ORDER,
        class: 'tag-wait-approve'
    },
    {
        name: "Đang chờ thiết kế",
        code: EStatusOder.PENDING_DESIGN,
        class: 'tag-request'
    },
    {
        name: "Chuẩn bị cho sản xuất",
        code: EStatusOder.READY_FOR_PRODUCTION,
        class: 'tag-init'
    },
    {
        name: "Đang sản xuất",
        code: EStatusOder.IN_PRODUCTION,
        class: 'tag-incomplete'
    },
    {
        name: "Đã đóng gói",
        code: EStatusOder.PACKAGED,
        class: 'tag-lock'
    },
    {
        name: "Đã hủy",
        code: EStatusOder.CANCELED,
        class: 'tag-init'
    },
]
    public static getStatusInfo(code, atribution = null) {
        let status = this.ListStatusOrderItem.find(i => i.code == code);
        return status && atribution ? status[atribution] : status
    }

    public static getListStatusInfo(codes: number[], atribution: string | null = null) {
        return codes
            .map(code => {
                let status = this.ListStatusOrderItem.find(i => i.code === code);
                return status && atribution ? status[atribution] : status;
            })
            .filter(Boolean);
    }

}