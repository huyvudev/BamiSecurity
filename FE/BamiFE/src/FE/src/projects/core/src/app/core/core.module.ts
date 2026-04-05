import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoreRoutingModule } from './core-routing.module';
import { HomeComponent } from './home/home.component';
import { CalendarModule } from 'primeng/calendar';
import { MyLibModule } from 'projects/my-lib/src/public-api';
import { AppMainComponent } from './layout/main/app.main.component';
import { AppMenuComponent } from './layout/menu/app.menu.component';
import { AppTopBarComponent } from './layout/top-bar/app.topbar.component';
import { AppMenuitemComponent } from './layout/menu/app.menuitem.component';
import { ToastModule } from 'primeng/toast';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { NgxSpinnerModule } from 'ngx-spinner';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { RadioButtonModule } from 'primeng/radiobutton';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ButtonModule } from 'primeng/button';
import { NgxEchartsModule } from 'ngx-echarts';
import { DashboardService } from './services/dashboard.service';
import { DropdownModule } from 'primeng/dropdown';
import { PdfViewerComponent } from './components/pdf-viewer/pdf-viewer.component';
import { ImageModule } from 'primeng/image';
import { AvatarModule } from 'primeng/avatar';
import { OrderItemModule } from "../modules/order-item/order-item.module";

@NgModule({
    imports: [
    CommonModule,
    CoreRoutingModule,
    ToastModule,
    AvatarModule,
    MyLibModule,
    NgxSpinnerModule.forRoot(),
    OrderItemModule
    
],
    declarations: [
        HomeComponent,
        AppMainComponent,
        AppMenuComponent,
        AppTopBarComponent,
        AppMenuitemComponent,
        PdfViewerComponent,
    ],
    exports: [
        MyLibModule,
		FormsModule,
		ReactiveFormsModule
    ],
    providers: []
})
export class CoreModule { }
