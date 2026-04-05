import { ChangeDetectorRef, Component, Inject, Input } from '@angular/core';
import { LibHelperService } from '../../shared/services/lib-helper.service';
import { ContentTypeEView, DefaultImage } from '../../shared/consts/base.consts';
import { IEnvironment } from '../../shared/interfaces/environment.interface';

@Component({
	selector: 'lib-image',
	template: `<img 
		(click)="showImage(getImage(src))" 
		[src]="src ? getImage(src) : imageDefault" 
		[class]="class + ' ' + (src ? 'cursor-pointer' : 'disable-click')"
		[ngStyle]="{'width': 'auto', 'max-height': height + 'px'}"
	/>
	`,
})
export class ImageComponent {
	@Input() src: string;
	@Input() class: string;
	@Input() isShowImage: boolean;
	@Input() imageDefault;
	@Input() height: number;

	environment: IEnvironment;
	
	constructor(
		private _libHelperService: LibHelperService,
		@Inject('env') environment
	) {
		this.environment = environment
	}

	showImage(src) {
		if (this.isShowImage && src) {
			this._libHelperService.dialogViewerRef(src, ContentTypeEView.IMAGE);
		}
	}
	
	getImage(image) {
		let apiView = this.environment?.api;
		console.log(apiView)
		return `${apiView}/media/${image?.url || image}`;
	}
}
