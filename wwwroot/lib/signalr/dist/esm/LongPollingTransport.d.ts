import {HttpClient} from "./HttpClient";
import {ILogger} from "./ILogger";
import {ITransport, TransferFormat} from "./ITransport";
import {IHttpConnectionOptions} from "./IHttpConnectionOptions";

/** @private */
export declare class LongPollingTransport implements ITransport {
    onreceive: ((data: string | ArrayBuffer) => void) | null;
    onclose: ((error?: Error) => void) | null;
    private readonly _httpClient;
    private readonly _accessTokenFactory;
    private readonly _logger;
    private readonly _options;
    private readonly _pollAbort;
    private _url?;
    private _running;
    private _receiving?;
    private _closeError?;
    private _getAccessToken;
    private _updateHeaderToken;
    private _poll;
    private _raiseOnClose;

    constructor(httpClient: HttpClient, accessTokenFactory: (() => string | Promise<string>) | undefined, logger: ILogger, options: IHttpConnectionOptions);

    get pollAborted(): boolean;

    connect(url: string, transferFormat: TransferFormat): Promise<void>;

    send(data: any): Promise<void>;

    stop(): Promise<void>;
}
