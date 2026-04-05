export enum EStatusOderItem {
    UNPRINTED = 0,
    PUSHED = 1,
    PRINTED = 2,
    CUT = 3,
    ENGRAVED = 4,
    IMPROVED = 5,
    COMPLETED = 6,
    SHIPPED = 7,
    CANCEL_SHIP = 8,
    TIKTOK = 9,

}

export class StatusOrderItem {

    public static statusItems = [
        {
            name: "Chưa in",
            code: 0
        },
        {
            name: "Đã đẩy",
            code: 1
        },
        {
            name: "Đã in",
            code: 2
        },
        {
            name: "Đã cắt",
            code: 3
        },
        {
            name: "Đã khắc",
            code: 4
        },
        {
            name: "Đã hoàn thiện",
            code: 5
        },
        {
            name: "Hoàn thành",
            code: 6
        },
        {
            name: "Đã ship",
            code: 7
        },
        {
            name: "Hủy ship",
            code: 8
        }
    ];

    public static ListStatusOrderItem = [{
        name: "Chưa in",
        code: EStatusOderItem.UNPRINTED,
        class: 'tag-pending'
    },
    {
        name: "Đã đẩy",
        code: EStatusOderItem.PUSHED,
        class: 'tag-wait-approve'
    },
    {
        name: "Đã in",
        code: EStatusOderItem.PRINTED,
        class: 'tag-request'
    },
    {
        name: "Đã cắt",
        code: EStatusOderItem.CUT,
        class: 'tag-init'
    },
    {
        name: "Đã khắc",
        code: EStatusOderItem.ENGRAVED,
        class: 'tag-incomplete'
    },
    {
        name: "Đã hoàn thành",
        code: EStatusOderItem.COMPLETED,
        class: 'tag-lock'
    },
    {
        name: "Đã hoàn thiện",
        code: EStatusOderItem.IMPROVED,
        class: 'tag-init'
    },
    {
        name: "Đã ship",
        code: EStatusOderItem.SHIPPED,
        class: 'tag-deactive'
    },
    {
        name: "Hủy ship",
        code: EStatusOderItem.CANCEL_SHIP,
        class: 'tag-cancel'
    },
    {
        name: "Tik Tok",
        code: EStatusOderItem.TIKTOK,
        class: 'tag-cancel'
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


    public static warningShip =[
        {
            name: "All",
            code: 0
        },
        {
            name: "Đã hoàn thành",
            code: 1
        },
        {
            name: "Đã đóng gói",
            code: 2
        },
    ]

    public static autoApproved = [
        {
            name: "All",
            code: 0
        },
        {
            name: "Yes",
            code: 1
        },
        {
            name: "No", code: 2
        }
    ];
    public static requestUpdate = [
        {
            name: "Waiting update",
            code: 0
        },
        {
            name: "Updated",
            code: 1
        },
        {
            name: "No request", code: 2
        }
    ];

}