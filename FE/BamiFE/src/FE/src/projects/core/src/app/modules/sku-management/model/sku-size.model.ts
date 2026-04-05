export class SkuSize {
    id: number;
    name: string;
    width: number;
    height: number;
    length: number;
    weight: number;
    additionalWeight: number;
    isDefault: boolean;
    baseCost: number;
    costInMeters: number;
    weightByVolume: number;
    packageDescription: string;
    skuId: number;
    mockUpsList : MockupSku[]
}



export class MockupSku {
    id: number;
    mockupUrl: string;
    skuSizeId: number
}
