import { Component } from '@angular/core';
import { DefaultImage } from '../../shared/consts/base.consts';

@Component({
  selector: 'lib-page-not-found',
  templateUrl: './page-not-found.component.html',
  styleUrls: ['./page-not-found.component.scss']
})
export class PageNotFoundComponent {
  defaultImage = DefaultImage;
  
  redirectToHome(): void {
    const currentOrigin = window.location.origin;
    window.location.href = `${currentOrigin}`;
  }
}
