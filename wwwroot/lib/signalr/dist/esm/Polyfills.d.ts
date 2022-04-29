/** @private */
export declare type EventSourceConstructor = new (url: string, eventSourceInitDict?: EventSourceInit) => EventSource;

/** @private */
export interface WebSocketConstructor {
    readonly CLOSED: number;
    readonly CLOSING: number;
    readonly CONNECTING: number;
    readonly OPEN: number;

    new(url: string, protocols?: string | string[], options?: any): WebSocket;
}
