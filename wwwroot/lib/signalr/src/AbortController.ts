// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Rough polyfill of https://developer.mozilla.org/en-US/docs/Web/API/AbortController
// We don't actually ever use the API being polyfilled, we always use the polyfill because
// it's a very new API right now.

// Not exported from index.
/** @private */
export class AbortController implements AbortSignal {
    public onabort: (() => void) | null = null;
    private _isAborted = false;

    get signal(): AbortSignal {
        return this;
    }

    get aborted() {
        return this._isAborted;
    }

    public abort() {
        if (!this._isAborted) {
            this._isAborted = true;
            if (this.onabort) {
                this.onabort();
            }
        }
    }
}

/** Represents a signal that can be monitored to determine if a request has been aborted. */
export interface AbortSignal {
    /** Indicates if the request has been aborted. */
    aborted: boolean;
    /** Set this to a handler that will be invoked when the request is aborted. */
    onabort: (() => void) | null;
}
