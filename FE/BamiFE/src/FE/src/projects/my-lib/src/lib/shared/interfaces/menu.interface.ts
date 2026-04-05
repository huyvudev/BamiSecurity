export interface IMenu {
	label: string;
	icon?: string;
	customIcon?: string;
	customIconActive?: string;
	routerLink: [string];
	isShow?: boolean;
	items?: IMenu[];
}

