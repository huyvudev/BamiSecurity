export class Partner {
    id: number;
    name: string;
    namePartnerType: string;
    createDate: Date;
    numberBatch: number;
    partnerTypeId :number;
}

export class PartnerCreate {
    name: string;
    partnerTypeId: number
}

export class PartnerUpdate extends PartnerCreate {
    id: number
}

export class PartnerType {
    id: number;
    name: string;
}

export class PartnerTypeCreate {
    name: string;
}

export class PartnerTypeUpdate extends PartnerTypeCreate {
    id: number;
}