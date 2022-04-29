/** @private */
export declare class AbortController implements AbortSignal {
    onabort: (() => void) | null;
    private _isAborted;

    get signal(): AbortSignal;

    get aborted(): boolean;

    abort(): void;
}

/** Represents a signal that can be monitored to determine if a request has been aborted. */
export interface AbortSignal {
    /** Indicates if the request has been aborted. */
    aborted: boolean;
    /** Set this to a handler that will be invoked when the request is aborted. */
    onabort: (() => void) | null;
}
