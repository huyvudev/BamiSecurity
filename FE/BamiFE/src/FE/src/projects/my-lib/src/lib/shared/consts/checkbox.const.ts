import { EYesNo } from "./base.consts";

export class CheckboxConsts {
    public static CHECKED = 'checked';
    public static UNCHECKED = 'unChecked';
    public static TYPE_BOOLEAN = 'Boolean';
    public static TYPE_YESNO = 'YesNo';

    public static values = [
        {
            type: this.TYPE_BOOLEAN,
            [this.CHECKED]: true,
            [this.UNCHECKED]: false,
        },
        {
            type: this.TYPE_YESNO,
            [this.CHECKED]: EYesNo.YES,
            [this.UNCHECKED]: EYesNo.NO,
        },
    ];

    public static getCheckedValues() {
        const checkedValues = this.values.map(c => c[this.CHECKED]);
        return checkedValues;
    }
}
