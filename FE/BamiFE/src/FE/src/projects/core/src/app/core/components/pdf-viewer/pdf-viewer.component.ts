import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-pdf-viewer',
  templateUrl: './pdf-viewer.component.html',
  styleUrls: ['./pdf-viewer.component.scss']
})
export class PdfViewerComponent implements OnInit {

  pdfUrl: SafeResourceUrl

  constructor(
		public ref: DynamicDialogRef,
		public configDialog: DynamicDialogConfig,
    private sanitizer: DomSanitizer
 
	) { 
    
  }
  ngOnInit(): void {
     this.pdfUrl = this.getSafeUrl(this.configDialog.data)
  }

  getSafeUrl(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
}
