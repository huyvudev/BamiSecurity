import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { IImageCroppedEvent } from '@mylib-shared/interfaces/image-cropped-event.interface';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'crop-image',
  templateUrl: './crop-image.component.html',
  styleUrls: ['./crop-image.component.scss']
})
export class CropImageComponent {

  constructor(
      private dialogConfig: DynamicDialogConfig,
      public dialogRef: DynamicDialogRef, 
      // public _fileService: FileService, 
      public sanitizer: DomSanitizer, 
  ) { }

  transform: any = {};
  croppedImage: any = '';
  showCropper: boolean = false;
  fileImage: File;
  fileUrl: string;
  fileCrop: any;

  ngOnInit(): void {
      this.fileImage = this.dialogConfig.data.fileImage;
  }

  imageLoaded() {
      this.showCropper = true;
  }

  blobImage: Blob;
  imageCropped(event: IImageCroppedEvent) {
      this.fileCrop = new File([event.blob], this.fileImage.name, { type: event.blob.type});
      this.fileCrop.objectUrl = event.objectUrl;
      //
      const divCrop: any = document.getElementsByClassName("ngx-ic-cropper");
      const elementCrop: HTMLElement = divCrop[0];
      this.imageInfo.width = elementCrop.offsetWidth;
      this.imageInfo.height = elementCrop.offsetHeight;
  }

  imageInfo: {width: number, height: number};
  cropperReady(sourceImageDimensions: {width: number, height: number}) {
      this.imageInfo = sourceImageDimensions;
  }

  zoomValue: number = 0;
  zoom(value) {
      const scale = 1 + (value/100);
      this.transform = {
          ...this.transform,
          scale: scale
      };
  }

  loadImageFailed() {
  }

  hideDialog() {
      this.dialogRef.close();
  }

  close() {
      this.hideDialog();
  }

  save() {
      this.dialogRef.close(this.fileCrop);
  }
}

