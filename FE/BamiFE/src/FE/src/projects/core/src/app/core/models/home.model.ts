export class DashboardFilterModel {
    firstDate?: any;
    endDate?: any;
    projectId?: number;

    constructor() {
        this.endDate = new Date();
        // 15 ngày trước
        const firstDate = new Date();
        firstDate.setDate(firstDate.getDate() - 15);
        this.firstDate = firstDate;
    }
}