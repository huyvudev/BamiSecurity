import { ModuleWithProviders, NgModule } from '@angular/core';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { ImageModule } from 'primeng/image';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FileUploadModule } from 'primeng/fileupload';
import { MultiSelectModule } from 'primeng/multiselect';
import { HandleLinkYoutubePipe } from './pipes/handle-link-youtube.pipe';

import { DateTimePipe } from './pipes/date-time.pipe';
import { AcceptFilePipe } from './pipes/accept-file.pipe';
import { CurrencyPipe } from './pipes/currency.pipe';
import { FunctionPipe } from './pipes/function.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';

import { TableComponent } from './component/table/table.component';
import { CheckboxComponent } from './component/checkbox/checkbox.component';
import { LoadingComponent } from './component/loading/loading.component';
import { ViewComponent } from './component/view/view.component';
import { UploadComponent } from './component/upload/upload.component';
import { MarkdownHtmlComponent } from './component/markdown-html/markdown-html.component';

import { SettingDisplayColumnTableComponent } from './component/table/setting-display-column-table/setting-display-column-table.component';
import { CropImageComponent } from './component/upload/crop-image/crop-image.component';
import { PaginatorModule } from 'primeng/paginator';
import { InputSearchComponent } from './component/input-search/input-search.component';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { MenuModule } from 'primeng/menu';
import { ResizeColumnTableDirective } from './directives/resize-column-table.directive';
import { MarkdownModule } from "ngx-markdown";
import { AngularEditorModule } from "@kolkov/angular-editor";
import { InputTextareaModule } from 'primeng/inputtextarea';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LibHelperService } from './shared/services/lib-helper.service';
import { SkeletonModule } from 'primeng/skeleton';
import { VideoComponent } from './component/video/video.component';
import { ClickOutsideDirective } from './directives/click-outside.directive';
import { PageNotFoundComponent } from './component/page-not-found/page-not-found.component';
import { BreadcrumbComponent } from './component/layout/breadcrumb/breadcrumb.component';
import { LabelDropdownPipe } from './pipes/label-dropdown.pipe';
import { TextareaAutoresizeDirective } from './directives/textarea-autoresize.directive';
import { ImageCropperModule } from 'ngx-image-cropper';
import { SliderModule } from 'primeng/slider';
import { KeyFilterModule } from 'primeng/keyfilter';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ClickDropdownDirective } from './directives/click-dropdown.directive';
import { DateMaskDirective } from './directives/date-mask.directive';
import { ThreeDotLoadingComponent } from './component/three-dot-loading/three-dot-loading.component';
import { ImageComponent } from './component/image/image.component';
import { TableTopbarComponent } from './component/table-topbar/table-topbar.component';
import { LibInputTextComponent } from './component/form-inputs/lib-input-text/lib-input-text.component';
import { LibSelectComponent } from './component/form-inputs/lib-select/lib-select.component';
import { InputDropdownSearchComponent } from './component/input-dropdown-search/input-dropdown-search.component';
import { InputMaskDirective } from './directives/input-mask.directive';
import { ButtonModule } from 'primeng/button';
import { ConfirmComponent } from './component/confirm/confirm.component';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputSwitchModule } from 'primeng/inputswitch';
import { PasswordModule } from 'primeng/password';
import { DropdownModule } from 'primeng/dropdown';
import { LibCalendarComponent } from './component/form-inputs/lib-calendar/lib-calendar.component';
import { LibInputNumberComponent } from './component/form-inputs/lib-input-number/lib-input-number.component';
import { LibInputSwitchComponent } from './component/form-inputs/lib-input-switch/lib-input-switch.component';
import { LibMultiSelectComponent } from './component/form-inputs/lib-multi-select/lib-multi-select.component';
import { LibPasswordComponent } from './component/form-inputs/lib-password/lib-password.component';
import { LibRadioButtonComponent } from './component/form-inputs/lib-radio-button/lib-radio-button.component';
import { LibButtonComponent } from './component/buttons/lib-button/lib-button.component';
import { GroupButtonSubmitComponent } from './component/buttons/group-button-submit/group-button-submit.component';
import { IconRequiredComponent } from './component/icon/icon-required/icon-required.component';
import { TooltipModule } from 'primeng/tooltip';
import { FormRequestComponent } from './component/form-shared/form-request/form-request.component';
import { FormApproveComponent } from './component/form-shared/form-approve/form-approve.component';
import { WpTabviewComponent } from './component/wp-tabview/wp-tabview.component';
import { InputOtp, InputOtpModule } from './component/primeng-module/inputotp/public_api';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { DateDropdownSearchComponent } from './component/date-dropdown-search/date-dropdown-search.component';
import { IEnvironment } from '@mylib-shared/interfaces/environment.interface';
import { CardModule } from 'primeng/card';
import { ReplaceSizePipe } from './pipes/replace-size.pipe';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        TableModule,
        CalendarModule,
        CheckboxModule,
        ImageModule,
        SelectButtonModule,
        FileUploadModule,
        PaginatorModule,
        InputTextModule,
        TagModule,
        MenuModule,
        InputTextareaModule,
        MarkdownModule,
        AngularEditorModule,
        NgxSpinnerModule,
        SkeletonModule,
        ImageCropperModule,
        SliderModule,
        KeyFilterModule,
		ButtonModule,
        InputNumberModule,
        InputSwitchModule,
        MultiSelectModule,
        PasswordModule,
        RadioButtonModule,
        DropdownModule,
		TooltipModule,
        InputOtpModule,
		OverlayPanelModule,
        CardModule 
    ],
    declarations: [
        ResizeColumnTableDirective,
        ConfirmComponent,
        TableComponent,
        ViewComponent,
        CheckboxComponent,
        LoadingComponent,
        ViewComponent,
        UploadComponent,
        MarkdownHtmlComponent,
        SettingDisplayColumnTableComponent,
        CropImageComponent,
        HandleLinkYoutubePipe,
        DateTimePipe,
        AcceptFilePipe,
        CurrencyPipe,
        FunctionPipe,
        TruncatePipe,
        InputSearchComponent,
        VideoComponent,
        ClickOutsideDirective,
        TextareaAutoresizeDirective,
        PageNotFoundComponent,
        BreadcrumbComponent,
        LabelDropdownPipe,
        ClickDropdownDirective,
        DateMaskDirective,
        ThreeDotLoadingComponent,
        ImageComponent,
        TableTopbarComponent,
        LibSelectComponent,
        InputDropdownSearchComponent,
        InputMaskDirective,
        LibInputTextComponent,
        LibCalendarComponent,
        LibInputNumberComponent,
        LibInputSwitchComponent,
        LibMultiSelectComponent,
        LibPasswordComponent,
        LibRadioButtonComponent,
		LibButtonComponent,
		GroupButtonSubmitComponent,
		FormRequestComponent,
		FormApproveComponent,
        IconRequiredComponent,
        WpTabviewComponent,
        DateDropdownSearchComponent,
        ReplaceSizePipe,
    ],
    exports: [
        ResizeColumnTableDirective,
        TableComponent,
        ViewComponent,
        CheckboxComponent,
        LoadingComponent,
        ViewComponent,
        UploadComponent,
        MarkdownHtmlComponent,
        SettingDisplayColumnTableComponent,
        CropImageComponent,
        ConfirmComponent,
        PageNotFoundComponent,
        BreadcrumbComponent,
        InputSearchComponent,
        VideoComponent,
        ThreeDotLoadingComponent,
        ImageComponent,
        TableTopbarComponent,
        InputDropdownSearchComponent,
        LibInputTextComponent,
        LibSelectComponent,
        LibCalendarComponent,
        LibInputNumberComponent,
        LibInputSwitchComponent,
        LibMultiSelectComponent,
        LibPasswordComponent,
        LibRadioButtonComponent,
		LibButtonComponent,
		GroupButtonSubmitComponent,
		FormRequestComponent,
		FormApproveComponent,
        WpTabviewComponent,

		// Icon
		IconRequiredComponent,
        // Pipes
        HandleLinkYoutubePipe,
        DateTimePipe,
        AcceptFilePipe,
        CurrencyPipe,
        FunctionPipe,
        TruncatePipe,
        LabelDropdownPipe,
        // Directives
        ClickOutsideDirective,
        TextareaAutoresizeDirective,
        ClickDropdownDirective,
        DateMaskDirective,
        InputMaskDirective,
        InputOtpModule
    ]
})

export class MyLibModule {
    public static forRoot(environment: IEnvironment): ModuleWithProviders<MyLibModule> {
        return {
            ngModule: MyLibModule,
            providers: [
                LibHelperService,
                {
                    provide: 'env',
                    useValue: environment
                },
            ]
        };
    }
}
