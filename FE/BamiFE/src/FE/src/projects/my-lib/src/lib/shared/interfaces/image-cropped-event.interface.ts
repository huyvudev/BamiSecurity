export interface IImageCroppedEvent {
    base64?: string | null;
    blob?: Blob | null;
    width: number;
    height: number;
    cropperPosition: ICropperPosition;
    imagePosition: ICropperPosition;
    offsetImagePosition?: ICropperPosition;
    objectUrl: string;
}

export interface ICropperPosition {
    x1: number;
    y1: number;
    x2: number;
    y2: number;
}
