import { ErrorHandler, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

// Application Components
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Application services
import { HanldeHttpInterceptor } from '../../../shared/providers/hanlde.interceptor';
import { MenuService } from './core/layout/menu/app.menu.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { MarkdownModule } from 'ngx-markdown';
import { MyLibModule } from 'projects/my-lib/src/public-api';
import { NgxEchartsModule } from 'ngx-echarts';
import { GlobalErrorHandler } from 'projects/shared/providers/global-error-hanlder';
import { environment } from '@shared/environments/environment';
import { OrderItemModule } from './modules/order-item/order-item.module';
import { MainOrderItemComponent } from './modules/order-item/components/main-order-item.component';


@NgModule({
    imports: [
        CommonModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        HttpClientModule,
        MarkdownModule.forRoot(),
        MyLibModule.forRoot(environment),
        NgxEchartsModule.forRoot({
            echarts: () => import('echarts')
        }),
    ],
    declarations: [
        AppComponent,

    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: HanldeHttpInterceptor, multi: true },
        { provide: ErrorHandler, useClass: GlobalErrorHandler },
        MenuService,
        MessageService,
        DialogService
    ],
    bootstrap: [AppComponent],
})
export class AppModule {
}
