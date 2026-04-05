export enum EErrorCode {
    Unauthorized = 401,
    BadRequest = 400,
    Forbidden = 403,
    NotFound = 404,
    MethodNotAllowed = 405,
    RequestTimeout = 408,
    UnsupportedMediaType = 415,
    InternalServerError = 500,
    BadGateway = 502,
    GatewayTimeout = 504,
}


export class ErrorCode {
    public static list: {[key: number] : string} = {
        400: 'BestRequest',
        401: 'Unauthorized',
        403: 'Forbidden',
        404: 'Not Found',
        405: 'Method Not Allowed',
        408: 'Request Time-out',
        415: 'Unsupported Media Type',
        500: 'Internal Server Error',
        502: 'Bad Gateway',
        504: 'Gateway Time-out',
    }
}

export class ErrorBankConst { 
    public static LOI_KET_NOI_MSB = 1505;
    public static SO_TK_KHONG_TON_TAI = 2036;
}
