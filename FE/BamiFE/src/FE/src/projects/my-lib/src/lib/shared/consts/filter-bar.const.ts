import { MenuItem } from "primeng/api/menuitem";
export enum ETableTopBar {
    INPUT_SELECT = 'INPUT_SELECT',
    INPUT_TEXT = 'INPUT_TEXT',
    ADD_MULTIPLE = 'ADD_MULTIPLE',
    ADD_SINGLE = 'ADD_SINGLE',
    ACTION_BUTTON = 'ACTION_BUTTON',
    SELECT = 'SELECT',
    MULTIPLE_SELECT = 'MULTIPLE_SELECT',
    DATE = 'DATE',
    DATE_SELECT = 'DATE_SELECT',
    DATE_TO_DATE = 'DATE_TO_DATE',
}

export interface IItemFilter {
    type: ETableTopBar;
    label?: string;
    icon?: string;
	styleClass?: string;
    items?: MenuItem[];
    data?: any;
    command?: Function;
    isShow?: boolean;
	variableReference?: string,
	placeholder?: string,
	widthClass?: string,
	defaultValue?: any,
    optionConfig?: {
        showClear?: boolean,
        data?: any,
		label?: string;
        value?: string,
        filter?: boolean,
        filterBy?: string,
        genLabelCode?: string[]
    };
}

export const SEED_DATA_FILTER_BAR = [ 
]
