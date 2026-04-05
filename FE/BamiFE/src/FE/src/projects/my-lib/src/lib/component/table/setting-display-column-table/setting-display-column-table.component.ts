import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { IColumn } from '../../../shared/interfaces/lib-table.interface';
import { IEventChecked } from '../../../shared/interfaces/event.interface';

@Component({
    selector: 'setting-display-column-table',
    templateUrl: './setting-display-column-table.component.html',
})

export class SettingDisplayColumnTableComponent {
    constructor(
        public ref: DynamicDialogRef,
        public configDialog: DynamicDialogConfig
    ) { }

    title: string;
    submitted = false;
    acceptStatus: boolean = true;

    selectColumns: IColumn[] = [];
    isCheckedAll:  boolean = true;
    isPinAll: boolean = true;

    ngOnInit(): void {
        this.selectColumns = JSON.parse(JSON.stringify(this.configDialog.data.cols));
        this.checkIsCheckedAll();
        this.selectColumns.forEach((col) => {
            if(col.displaySettingColumn){
                if(!col.isPin){
                    this.isPinAll = false;
                }
            }
        })
    }

	countColumnShow() {
		const columnDisplay = this.selectColumns.filter(s => s?.displaySettingColumn);
		return `(${columnDisplay?.length} cột)`;
	}

    checkIsCheckedAll() {
        let countSeletedDisplay = this.selectColumns.filter(s => s.displaySettingColumn)?.length;
        let countSeletedShow = this.selectColumns.filter(s => s.isShow && s.displaySettingColumn)?.length;
        this.isCheckedAll = (countSeletedDisplay === countSeletedShow);
    }

    changeSelectedAll(event: IEventChecked) {
        this.selectColumns.forEach(item => {
            if(item.displaySettingColumn && !item?.isPin) {
                item.isShow = event.checked;
            }
        });
    }

    accept() {
        this.acceptStatus = true;
        this.onAccept();
    }

    cancel() {
        this.acceptStatus = false;
        this.onAccept();
    }

    onAccept() {
        this.selectColumns = this.selectColumns.sort( (a, b) => a.position - b.position);
        this.ref.close({ data: this.selectColumns, accept: this.acceptStatus });
    }
}
