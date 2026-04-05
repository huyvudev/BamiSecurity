export interface IEvent {
    originalEvent: PointerEvent;
}

export interface IEventChecked extends IEvent {
    checked: boolean,
}

export interface IEventTabView extends IEvent {
    index: number;
}

export interface IEventData<Data> extends IEvent {
    value: Data;
}

export interface ITabView {
    [key: string] : boolean;
} 

export interface ISelectedFileUpload extends IEvent {
    currentFiles: File[],
    files: number;
    originEvent: PointerEvent,
}