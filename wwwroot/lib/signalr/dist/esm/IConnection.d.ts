import {TransferFormat} from "./ITransport";

/** @private */
export interface IConnection {
    readonly features: any;
    readonly connectionId?: string;
    baseUrl: string;
    onreceive: ((data: string | ArrayBuffer) => void) | null;
    onclose: ((error?: Error) => void) | null;

    start(transferFormat: TransferFormat): Promise<void>;

    send(data: string | ArrayBuffer): Promise<void>;

    stop(error?: Error): Promise<void>;
}
