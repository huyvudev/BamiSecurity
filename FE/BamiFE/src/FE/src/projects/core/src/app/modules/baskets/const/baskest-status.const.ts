export enum EStatusBaskets {
    WAITING = 0,
    COMPLETED = 1,
    FREE = 2,
}

export class BasketsConst {
    public static BasketsStatus = [
        {
            name: 'Đang chờ',
            code: 0
        },
        {
            name: 'Đã hoàn thành',
            code: 1
        },
        {
            name: 'Tự do',
            code: 2
        },
    ]

    public static Address = [
        {
            name: 'Thường (US)',
            code: 0
        },
        {
            name: 'Quốc tế',
            code: 1
        },
    ]
    public static ListStatusBaskets = [
        {
            name: "Đang chờ",
            code: EStatusBaskets.WAITING,
            class: 'tag-pending'
        },
        {
            name: "Đã hoàn thành",
            code: EStatusBaskets.COMPLETED,
            class: 'tag-incomplete'
        },
        {
            name: "Tự do",
            code: EStatusBaskets.FREE,
            class: 'tag-init'
        },
    ]
    public static getStatusInfo(code, atribution = null) {
        let status = this.ListStatusBaskets.find(i => i.code == code);
        return status && atribution ? status[atribution] : status
    }

    public static typeOrder =[
        {
            code : 0,
            name :'Chưa chọn',
        },
        {
            code : 1,
            name :'Đơn 2',
        },
        {
            code : 2,
            name :'Đơn 3',
        },
        {
            code : 3,
            name :'Đơn 4',
        },
        {
            code : 4,
            name :'Đơn 5',
        },
    ]
}