import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainBatchComponent } from './components/main-batch.component';
import { BatchRoutingModule } from './batch-routing.module';
import { CoreModule } from '../../core/core.module';
import { CreateBatchComponent } from './components/create-batch/create-batch.component';
import { PartnerService } from '../partner/services/partner.service';
import { BatchDetailComponent } from './components/batch-detail/batch-detail.component';
import { UpdateStatusBatchComponent } from './components/update-status-batch/update-status-batch.component';
import { QRCodeModule } from 'angularx-qrcode';


@NgModule({
  declarations: [
    MainBatchComponent,
    CreateBatchComponent,
    BatchDetailComponent,
    UpdateStatusBatchComponent
  ],
  imports: [
    CommonModule,
    BatchRoutingModule,
    CoreModule,
    QRCodeModule
  ],
  providers: [
    PartnerService
  ]
})
export class BatchModule { }
