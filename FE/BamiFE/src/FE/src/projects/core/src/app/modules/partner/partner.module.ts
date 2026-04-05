import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainPartnerComponent } from './components/main-partner.component';
import { PartnerRoutingModule } from './partner-routing.module';
import { CoreModule } from '../../core/core.module';
import { CreatePartnerComponent } from './components/create-partner/create-partner.component';
import { MainPartnerTypeComponent } from './components/main-partner-type/main-partner-type.component';
import { CreatePartnerTypeComponent } from './components/main-partner-type/create-partner-type/create-partner-type.component';
import { PartnerService } from './services/partner.service';
import { PartnerTypeService } from './services/partner-type.service';
import { DialogService } from 'primeng/dynamicdialog';




@NgModule({
  declarations: [
    MainPartnerComponent,
    CreatePartnerComponent,
    MainPartnerTypeComponent,
    CreatePartnerTypeComponent
  ],
  imports: [
    CommonModule,
    PartnerRoutingModule,
    CoreModule
  ],
  providers :[
    PartnerService,
    PartnerTypeService,
    DialogService
  ]
})
export class PartnerModule { }
