import { DestroyRef, Directive, inject } from "@angular/core";
import { MixinBase } from "../minxin-component-base.ts/base";
import { IColumn, IColumnConfig } from "@mylib-shared/interfaces/lib-table.interface";
import { ETableColumnType, ETableFrozen } from "@mylib-shared/consts/lib-table.consts";
import { Page } from "@mylib-shared/models/page";

@Directive()
export abstract class PageBase extends MixinBase(class{}) { 

	getColumnId(config: IColumnConfig = {}): IColumn {
        const defaultConfig: IColumn = {
            field: 'id',
            header: '#ID',
            width: 5,
            action: undefined,
            isFrozen: true,
            isPin: true,
            alignFrozen: ETableFrozen.LEFT,
            otherType: config.isLink !== true ? ETableColumnType.TEXT : ETableColumnType.LINK,
            customValue: (value) => {
                return value = (value?.id ?? value ?? '-').toString().slice(0, 4);
            }
        };
    
        const finalConfig = { ...defaultConfig, ...config };
    
        return finalConfig
    }
    
    getColumnStatus(config: IColumnConfig = {}): IColumn {
        const defaultConfig: IColumn = {
            field: 'status',
            header: 'Trạng thái',
            width: 7.5,
            isFrozen: true,
            alignFrozen: ETableFrozen.RIGHT,
            type: ETableColumnType.STATUS,
            getTagInfo: undefined,
        };
    
        const finalConfig = { ...defaultConfig, ...config };
    
        return finalConfig
    }

    getColumnAction(config: IColumn = {}): IColumn {
        const defaultConfig: IColumn = {
            field: '',
            header: '',
            width: 3,
            displaySettingColumn: false,
            isFrozen: true,
            alignFrozen: ETableFrozen.RIGHT,
            type: ETableColumnType.ACTION_DROPDOWN,
        };
    
        const finalConfig = { ...defaultConfig, ...config };
    
        return finalConfig
    }
}