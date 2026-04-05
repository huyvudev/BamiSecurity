import { InjectionToken } from "@angular/core";
import { ApiConfig } from "./interface-tokens";

export const API_CONFIG = new InjectionToken<ApiConfig>('api-config')
export const PERMISSION_KEYS = new InjectionToken<any>('permission-keys')