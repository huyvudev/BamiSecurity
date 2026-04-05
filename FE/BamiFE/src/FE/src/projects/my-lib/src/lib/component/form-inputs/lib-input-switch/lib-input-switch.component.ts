import { ChangeDetectionStrategy, Component, EventEmitter, forwardRef, Input, Output, ViewChild } from '@angular/core';
import { LIB_NG_VALUE_ACCESSOR, LibBaseControl } from '../lib-base';
import { InputSwitch } from 'primeng/inputswitch';

@Component({
    selector: 'lib-inputSwitch',
    styleUrls: ['./lib-input-switch.component.scss'],
    template: `
        <ng-container>
            <div>
                <label class="mb-2" *ngIf="label">
                    {{ label }}
                    <!-- <span class="required-field" *ngIf="showIconRequired"> * </span> -->
					<icon-required *ngIf="isRequired || required" />
                </label>
                <!--  -->
                <p-inputSwitch
                    #inputSwitch
                    [(ngModel)]="value"
                    [inputId]="inputId"
                    [name]="name"
                    [disabled]="isDisabled || disabled"
                    [trueValue]="trueValue"
                    [falseValue]="falseValue"
                    (onChange)="_onChange($event.checked); onChange.emit($event)"
                >
                </p-inputSwitch>
            </div>
        </ng-container>
    `,
    providers: [
        {
            provide: LIB_NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => LibInputSwitchComponent),
            multi: true
        }
    ],
    changeDetection: ChangeDetectionStrategy.OnPush
})

export class LibInputSwitchComponent extends LibBaseControl {

    @Input() showIconRequired: boolean = false;
    @Input() style: string;
    @Input() styleClass: string;
    @Input() trueValue: any = true;
    @Input() falseValue: any = false;

    @Output() onChange = new EventEmitter<any>(false);

    @ViewChild('inputSwitch') inputSwitch: InputSwitch;

}
