import { Store } from "./store.model";

export class Brand {
    id: number;
    name: string;
    stores? : Store[]
}