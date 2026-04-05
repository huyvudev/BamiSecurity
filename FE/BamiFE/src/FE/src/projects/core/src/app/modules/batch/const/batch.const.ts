export enum EBtachStatus {
    WAITING = 0,
    PRINTED = 1,
    CUT = 2,
    ENGRAVED = 3,
    IMPROVED = 4,
    QCHT = 5,
    COMPLETED = 6,
    UNFINISHED = 7,
}

export enum EPriorityBatch {
    NORMAL = 0,
    HIGH = 1,
    HIGHEST = 2,
}

export class StatusBatch {
    public static status = [
        {
            name: "Đang chờ",
            code: 0
        },
        {
            name: "Đã in",
            code: 1
        },
        {
            name: "Đã cắt",
            code: 2
        },
        {
            name: "Đã khắc",
            code: 3
        },
        {
            name: "Đã hoàn thiện",
            code: 4
        },
        {
            name: "QCHT",
            code: 5
        },
        {
            name: "Hoàn thành",
            code: 6
        },
        {
            name: "Chưa hoàn thành",
            code: 7
        },
    ];

    public static ListStatusBatch = [
        {
            name: "Đang chờ",
            code: EBtachStatus.WAITING,
            class: 'tag-pending'
        },
        {
            name: "Đã in",
            code: EBtachStatus.PRINTED,
            class: 'tag-request'
        },
        {
            name: "Đã cắt",
            code: EBtachStatus.CUT,
            class: 'tag-init'
        },
        {
            name: "Đã khắc",
            code: EBtachStatus.ENGRAVED,
            class: 'tag-incomplete'
        },
        {
            name: "Đã hoàn thiện",
            code: EBtachStatus.IMPROVED,
            class: 'tag-deactive'
        },
        {
            name: "QCHT",
            code: EBtachStatus.QCHT,
            class: 'tag-wait-approve'
        },
        {
            name: "Hoàn thành",
            code: EBtachStatus.COMPLETED,
            class: 'tag-lock'
        },
        {
            name: "Chưa hoàn thành",
            code: EBtachStatus.UNFINISHED,
            class: 'tag-cancel'
        },
    ]

    public static getStatusInfo(code, atribution = null) {
        let status = this.ListStatusBatch.find(i => i.code == code);
        return status && atribution ? status[atribution] : status
    }

    public static Prioritize = [
        {
            name: 'Bình thường',
            code: 0
        },
        {
            name: 'Cao',
            code: 1
        },
        {
            name: 'Cao nhất',
            code: 2
        },
    ]

    public static PrioritizeStatus = [
        {
            name: 'Bình thường',
            code: EPriorityBatch.NORMAL,
            class: 'tag-pending'
        },
        {
            name: 'Cao',
            code: EPriorityBatch.HIGH,
            class: 'tag-wait-approve'
        },
        {
            name: 'Cao nhất',
            code: EPriorityBatch.HIGHEST,
            class: 'tag-request'
        },
    ]

    public static TypeBatch = [
        {
            name: 'Tạo Lô theo số lượng',
            code: 0
        },
        {
            name: 'Tạo lô theo danh sách',
            code: 1
        },
    ]

    public static getPrioritizeStatusInfo(codes: number[], atribution: string | null = null) {
        return codes
            .map(code => {
                let status = this.PrioritizeStatus.find(i => i.code === code);
                return status && atribution ? status[atribution] : status;
            })
            .filter(Boolean);
    }



    public static TimePrint = [
        {
            name: 'Quá hạn',
            code: 0
        },
        {
            name: 'Quá hạn (đã sản xuất)',
            code: 1
        },
        {
            name: 'Quá hạn (chưa & đang sản xuất)',
            code: 2
        },
        {
            name: 'Không quá hạn',
            code: 3
        },
        {
            name: 'Khoảng giờ',
            code: 3
        },
    ]


    public static TimeCut = [
        {
            name: 'Quá hạn',
            code: 0
        },
        {
            name: 'Không quá hạn',
            code: 1
        },
        {
            name: 'Khoảng giờ',
            code: 2
        },
    ];

    public static StatusMerge = [
        {
            name: 'Created',
            code: 0
        },
        {
            name: 'Pending',
            code: 1
        },
        {
            name: 'Processing',
            code: 2
        },
        {
            name: 'Done',
            code: 3
        },
        {
            name: 'Error',
            code: 4
        },
        {
            name: 'Retry',
            code: 5
        },
    ]

    public static ShipStatus = [
        {
            name: 'Yes',
            code: 0
        },
        {
            name: 'No',
            code: 1
        },
    ]
}
