import { EFormatDateDisplay } from "../consts/base.consts";
import { ETableColumnType, ETableFrozen, OrderSort, ValueType } from "../consts/lib-table.consts";

export interface IColumn {
    field?: string;
    header?: string;
    title?:string,
    width?: number;
    minWidth?: number;
    style?: Object;
    left?: number; // fix frozen left
    right?: number; // fix frozen right
    isShow?: boolean,
    hasUpdateIsShow?: boolean,
    isSort?: boolean,
    type?: ETableColumnType | EFormatDateDisplay;
    otherType?: ETableColumnType | EFormatDateDisplay;
    isPin?: boolean; // Khóa cột
    isFrozen?: boolean; // Fixed cột
    alignFrozen?: ETableFrozen;   // Vị trí fixed cột left|right
    isResize?: boolean; // width auto
    class?: string;
    icon?: string;
    classButton?: string;
    isCutText?: boolean;
    position?: number;
    displaySettingColumn?: boolean;
    unit?: string;
    fieldSort?: string;
    getTagInfo?: Function;
    getDisabled?: Function;
    action?: Function;
    customValue?: Function;
    valueType?: ValueType;
    sliceString?: number;
    isPermission?: boolean;
    heightImage?: number;   
    widthImage?: number;
    isCheckboxIcon?:boolean;
    isHideOptionCheckbox?: boolean;
    disableButtonAction?: Function;
}

export interface IColumnConfig {
    field?: string;
    header?: string;
    width?: number;
    action?: Function;
    isFrozen?: boolean;
    isPin?: boolean;
    alignFrozen?: ETableFrozen;
    otherType?: ETableColumnType | EFormatDateDisplay;
    type?: ETableColumnType | EFormatDateDisplay;
    customValue?: Function;
    displaySettingColumn?: boolean;
    class?: string;
    getTagInfo?: Function;
    isLink?: boolean
}
export class IAction {
    data?: any;
    label: string;
    icon?: string;
    command: ($event: any) => void;
}

export interface ISort {
    field: string;
    order: OrderSort;
}

export interface IDropdown {
    name?: string;
    labelName?: string;
    code?: number | string | boolean;
    value?: number | string | boolean;
    severity?: string;
    rawData?: any;
  }