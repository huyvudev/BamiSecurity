import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { MainBatchComponent } from './components/main-batch.component';
import { BatchDetailComponent } from './components/batch-detail/batch-detail.component';
import { UpdateStatusBatchComponent } from './components/update-status-batch/update-status-batch.component';
const routes: Routes = [
    {
        path: 'batch',
        component: MainBatchComponent
    },
    {
        path: 'batch/detail/:id',
        component: BatchDetailComponent
    },
    {
        path: 'qrCreate',
        component: UpdateStatusBatchComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class BatchRoutingModule { }
