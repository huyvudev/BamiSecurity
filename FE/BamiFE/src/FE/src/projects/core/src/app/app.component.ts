import {Component, OnInit} from '@angular/core';
import {PrimeNGConfig} from 'primeng/api';

@Component({
    selector: 'app-root',
    template: `
        <router-outlet></router-outlet>
    `
})
export class AppComponent implements OnInit{

    horizontalMenu: boolean;

    darkMode = false;

    menuColorMode = 'light';

    menuColor = 'layout-menu-light';

    themeColor = 'blue';

    layoutColor = 'blue';

    ripple = true;

    inputStyle = 'outlined';

    constructor(
        private primengConfig: PrimeNGConfig,
    ) {}

    ngOnInit() {
        this.primengConfig.ripple = true;
    }
}
