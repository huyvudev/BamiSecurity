
export enum EStautsFilePrint {
    PENDING = 0,
    PROCESSING = 1,
    REVIEW = 2,
    DONE = 3,
    FULLY_DONE = 4,
    ON_HOLD = 5,
    RETRY = 6,
    ERROR = 7,
    CANCELLED = 8,
    TIK_TOK = 9,
}

export class StautsFilePrint {

    static numberItems = [
        {
            code: 0
        },
        {
            code: 1
        },
        {
            code: 2
        },
        {
            code: 3
        },
        {
            code: 4
        },
        {
            code: 5
        },
        {
            code: 6
        },
        {
            code: 7
        },
        {
            code: 8
        }
    ];

   
    public static autoApproved = [
        {
            name: "YES",
            code: 1
        },
        {
            name: "NO", code: 0
        }
    ];

    public static ListFilePrintStatus = [{
        name: "Chờ xử lý",
        code: EStautsFilePrint.PENDING,
        class:'tag-pending'
    },
    {
        name: "Đang xử lý",
        code: EStautsFilePrint.PROCESSING,
          class:'tag-wait-approve'
    },
    {
        name: "Đợi duyệt",
        code: EStautsFilePrint.REVIEW,
          class:'tag-request'
    },
    {
        name: "Đã xong một phần",
        code: EStautsFilePrint.DONE,
          class:'tag-init'
    },
    {
        name: "Đã xong",
        code: EStautsFilePrint.FULLY_DONE,
          class:'tag-incomplete'
    },
    {
        name: "Tạm dừng",
        code: EStautsFilePrint.ON_HOLD,
          class:'tag-lock'
    },
    {
        name: "Tạo lại",
        code: EStautsFilePrint.RETRY,
          class:'tag-init'
    },
    {
        name: "Lỗi",
        code: EStautsFilePrint.ERROR,
          class:'tag-deactive'
    },
    {
        name: "Đã hủy",
        code: EStautsFilePrint.CANCELLED,
          class:'tag-cancel'
    },
    {
        name: "Tik Tok",
        code: EStautsFilePrint.TIK_TOK,
          class:'tag-cancel'
    }]

    public static getStatusInfo(code, atribution = null) {
        let status = this.ListFilePrintStatus.find(i => i.code == code);
        return status && atribution ? status[atribution] : status
    }

    public static getListStatusInfo(codes: number[], atribution: string | null = null) {
        return codes
            .map(code => {
                let status = this.ListFilePrintStatus.find(i => i.code === code);
                return status && atribution ? status[atribution] : status;
            })
            .filter(Boolean);
    }

    statusFIlePrint: any = [{
            name: "Chờ xử lý",
            code: 0
        },
        {
            name: "Đang xử lý",
            code: 1
        },
        {
            name: "Đợi duyệt",
            code: 2
        },
        {
            name: "Đã xong một phần",
            code: 3
        },
        {
            name: "Đã xong",
            code: 4
        },
        {
            name: "Tạm dừng",
            code: 5
        },
        {
            name: "Tạo lại",
            code: 6
        },
        {
            name: "Lỗi",
            code: 7
        },
        {
            name: "Đã hủy",
            code: 8
        }]

}


