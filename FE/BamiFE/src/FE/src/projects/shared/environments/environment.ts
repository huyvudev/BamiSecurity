// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { IEnvironment } from "@mylib-shared/interfaces/environment.interface";

export const environment: IEnvironment = {
    production: false,
    api: 'http://pf-test-api.stecom.vn:10104',
    apiFile: 'http://pf-test-api.stecom.vn:10104',
};
