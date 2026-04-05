import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ETypeHandleLinkYoutube } from '../../shared/consts/base.consts';
import { Utils } from '../../shared/utils';

@Component({
  selector: 'lib-video',
  styleUrls: ['./video.component.scss'],
  template: `
    <div>
      <ng-container>
        <video
          *ngIf="link && !linkYoutube"
          [style.height.px]="height"
          [style.width]="width ? width + 'px' : '100%'"
          [src]="link"
          controls
        ></video>
      </ng-container>
      <ng-container
        *ngIf="link | handleLinkYoutube : ETypeHandleLinkYoutube.CHECK_LINK"
      >
        <iframe
          [width]="width || '100%'"
          [height]="height"
          [src]="
            link | handleLinkYoutube : ETypeHandleLinkYoutube.GET_EMBED_LINK
          "
          frameborder="0"
          allowfullscreen
        >
        </iframe>
      </ng-container>
    </div>
  `,
})
export class VideoComponent implements OnInit {
  @Input() height: number = 250;
  @Input() width: number;
  @Input() src: string | File;

  link: string | SafeUrl;
  linkYoutube: boolean;
  //
  ETypeHandleLinkYoutube = ETypeHandleLinkYoutube;

  constructor(private sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    this.init();
  }

  init() {
    this.link = this.src;
    if (typeof this.src === 'string' && !this.checkLinkYoutube(this.src)) {
      this.link = this.src;
      // this.link = !this.src.includes("https") ? (AppConsts.remoteServiceBaseUrl + '/' + this.src) : this.src;
    }
    //
    if (this.src instanceof File) {
      this.link = this.sanitizer.bypassSecurityTrustUrl(
        URL.createObjectURL(this.src)
      );
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    this.init();
  }

  checkLinkYoutube(url): boolean {
    const check = Utils.checkLinkYoutube(url);
    this.linkYoutube = check;
    return check;
  }
}
