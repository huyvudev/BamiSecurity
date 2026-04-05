import { SkuSize } from "./sku-size.model";

export class Sku {
    id: number;
    code: string;
    description?: string;
    isBigSize: boolean;
    isActive: boolean;
    needToReview: boolean;
    needManageMaterials: boolean;
    allowQcMultipleItems: boolean;
    skuBaseId?: number;
    materialId?: number;
    productMethodId?: number;
    sizes :SkuSize[]
}