export class RequestModel{
    id: number | undefined;
    userApproveId: number | undefined;
    requestNote: string;
    summary: string;
    actionType?: number
}

export class ApproveModel{
    id: number | undefined;
    note: string;
    checkApprove: boolean = true;
}