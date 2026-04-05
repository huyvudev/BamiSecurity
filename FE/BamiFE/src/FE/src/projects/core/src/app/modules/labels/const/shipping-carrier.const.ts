export class LabelConstant {
    public static ShippingCarrierStatus = [
        {
            name: 'Royal Mail',
            code: 1
        },
        {
            name: 'Singapore Post',
            code: 2
        },
        {
            name: 'Shoppe',
            code: 3
        },
        {
            name: 'TimUS',
            code: 4
        },
        {
            name: 'UPS',
            code: 5
        },
        {
            name: 'USPS MN',
            code: 6
        },
        {
            name: 'Daily Design',
            code: 7
        },
        {
            name: 'LLC_manual',
            code: 8
        },
        {
            name: 'DPD UK',
            code: 9
        },
        {
            name: 'SB CA',
            code: 10
        },
        {
            name: 'SB AU',
            code: 11
        },
        {
            name: 'SB YUN',
            code: 12
        },
        {
            name: 'HW_USPS',
            code: 13
        },
        {
            name: 'JT_USPS',
            code: 14
        }

    ]

    public static statusLabels = [{
        name: 'Printable',
        code: 1
    },
    {
        name: 'UnPrintable',
        code: 2
    },
    ]

    public static getNameByCode(code: number): string | undefined {
        const carrier = this.ShippingCarrierStatus.find(item => item.code === code);
        return carrier ? carrier.name : undefined;
    }
}