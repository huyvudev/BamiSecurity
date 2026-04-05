export enum EStatusOderDetail {
    PENDING = 0,
    PROCESSING = 1,
    FAILED = 2,
    DONE = 3,
    CANCELLED = 4,
    APPROVED = 5,
}

export class StatusOrderDetail {
    public static status = [
        {
            name: "Chờ xử lý",
            code: 0
        },
        {
            name: "Đang xử lý",
            code: 1
        },
        {
            name: "Thất bại",
            code: 2
        },
        {
            name: "Thành công",
            code: 3
        },
        {
            name: "Hủy",
            code: 4
        },
        {
            name: "Đã duyệt",
            code: 5
        },
    ];
    public static ListStatusOrderItem = [{
        name: "Chờ xử lý",
        code: EStatusOderDetail.PENDING,
        class: 'tag-pending'
    },
    {
        name: "Đang xử lý",
        code: EStatusOderDetail.PROCESSING,
        class: 'tag-request'
    },

    {
        name: "Thất bại",
        code: EStatusOderDetail.FAILED,
        class: 'tag-request'
    },
    {
        name: "Thành công",
        code: EStatusOderDetail.DONE,
        class: 'tag-init'
    },
    {
        name: "Đã hủy",
        code: EStatusOderDetail.CANCELLED,
        class: 'tag-lock'
    },
    {
        name: "Đã duyệt",
        code: EStatusOderDetail.APPROVED,
        class: 'tag-wait-approve'
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