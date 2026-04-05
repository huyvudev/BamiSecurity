export enum ETableColumnType {
    REORDER = "REORDER",    // KÉO THẢ
    IMAGE = "IMAGE",    // IMAGE
    TEXT = 'TEXT',  // TEXT
    //
    CHECKBOX_ACTION = 'CHECKBOX_ACTION',    // CHECKBOX CÓ THỂ TICK
    CHECKBOX_CHANGE = 'CHECKBOX_CHANGE',    // CHECKBOX XỬ LÝ GỌI HÀM
    CHECKBOX_SHOW = 'CHECKBOX_SHOW',    // CHECKBOX CHỈ ĐỂ SHOW
    //
    CURRENCY = 'CURRENCY',  // KIỂU TIỀN TỆ (pipe)
    //
    ACTION_DROPDOWN = 'ACTION_DROPDOWN',    // (FUNCTIONS) LIST ACTION
    ACTION_BUTTON = 'ACTION_BUTTON',    // (FUNCTION) GIỐNG ACTION_ICON CHỈ KHÁC VỀ MẶT HIỂN THỊ
    ACTION_ICON = 'ACTION_ICON',    // (FUNCTION)
    //        
    STATUS = 'STATUS', // HIỂN THỊ KIỂU THẺ P-TAG
    HISTORY = 'HISTORY', // HIỂN THỊ lịch sử thay đổi

    LINK = 'LINK', // TEXT LINK
    QUARANTINE_CONTRACT = 'QUARANTINE_CONTRACT', // Hợp đồng phong tỏa

    IS_MOCKUP_FRONT = 'IS_MOCKUP_FRONT',
    IS_MOCKUP_BACK = 'IS_MOCKUP_BACK',
    DESIGN = 'DESIGN',
    SIZE_TEXT = 'SIZE_TEXT',
    IMAGE_FILE_PRINT = 'IMAGE_FILE_PRINT',
    IMAGE_SET = 'IMAGE_SET',
    TEMPLATE = 'TEMPLATE',
    LIST_STATUS = 'LIST_STATUS',
    LABEL = 'LABEL',
    TRACKING_NUMBER = 'TRACKING_NUMBER',
    NOTE_BATCH = 'NOTE_BATCH',
    NOTE_ITEM = 'NOTE_ITEM'
}

export enum ETableFrozen {
    LEFT = 'left',  // FIXED COLUMN LEFT
    RIGHT = 'right',    // FIXED COLUMN RIGHT
}

export enum ValueType {
    FIELD = 'FIELD',    // LẤY GIÁ TRỊ THEO FIELD (ROW.FIELD)
    ROW = 'ROW',    // LẤY GIÁ TRỊ LÀ ROW
}

export enum ETableMessage {
    empty = 'Không có dữ liệu', // MESSAGE HIỂN THỊ KHI TABLE KHÔNG CÓ DATA
}

export enum OrderSort {
    ASC = 1,
    DESC = -1,
}


export enum EImageTable {
    EMPTY = '/shared/assets/layout/images/table/empty-2.png',
}