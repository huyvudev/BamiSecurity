import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { StatusUpdateComponent } from './components/status-update/status-update.component';
import { EStatusOderItem } from '../order-item/const/status-item.const';



const routes: Routes = [
    {
        path: 'complete',
        component: StatusUpdateComponent,
        data: { status: EStatusOderItem.COMPLETED }
    },
    {
        path: 'pushed',
        component: StatusUpdateComponent,
        data: { status: EStatusOderItem.PUSHED }
    },
    {
        path: 'printed',
        component: StatusUpdateComponent,
        data: { status: EStatusOderItem.PRINTED }
    },
    {
        path: 'cuted',
        component: StatusUpdateComponent,
        data: { status: EStatusOderItem.CUT }
    },
    {
        path: 'engraved',
        component: StatusUpdateComponent,
        data: { status: EStatusOderItem.ENGRAVED }
    },
    {
        path: 'improved',
        component: StatusUpdateComponent,
        data: { status: EStatusOderItem.IMPROVED }
    },
    {
        path: 'shipped',
        component: StatusUpdateComponent,
        data: { status: EStatusOderItem.SHIPPED }
    },
    {
        path: 'cancel-ship',
        component: StatusUpdateComponent,
        data: { status: EStatusOderItem.CANCEL_SHIP }
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class UpdateStatusRoutingModule { }
