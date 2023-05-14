import { Http } from "../cross-cutting/http";
import { appConfig } from '../configuration';
import { DI } from '@microsoft/fast-foundation';

export interface CommunicationApiProxy {
    requestLongReport(input: LongReportRequest): Promise<string>;
    requestLongTask(input: LongTaskRequest): Promise<string>;
    getAsyncActionItems(username: string): Promise<AsyncActionItem[]>;
    enableArticlesPush(): Promise<string>;
    disableArticlesPush(): Promise<string>;
}

export class CommunicationApiProxyImpl implements CommunicationApiProxy {
    @Http httpClient: Http;

    requestLongReport(input: LongReportRequest): Promise<string> {
        return this.httpClient
            .post<string>(`${appConfig.beBaseUrl}/api/communication/long-report`, input);
    }
    requestLongTask(input: LongTaskRequest): Promise<string> {
        return this.httpClient
            .post<string>(`${appConfig.beBaseUrl}/api/communication/long-task`, input);
    }
    
    getAsyncActionItems(username: string): Promise<AsyncActionItem[]> {
        return this.httpClient
            .get<AsyncActionItem[]>(`${appConfig.beBaseUrl}/api/communication/get-items-by-user?username=${username}`);
    }

    public enableArticlesPush(): Promise<string> {
        return this.httpClient
            .post<string>(`${appConfig.beBaseUrl}/api/communication/enable-contextual-info-push`, null);
    }
    public disableArticlesPush(): Promise<string> {
        return this.httpClient
            .post<string>(`${appConfig.beBaseUrl}/api/communication/disable-contextual-info-push`, null);
    }
}

export interface LongReportRequest {
    connectionId: string,
    userName: string,
    customerName: string,
}
export interface LongTaskRequest {
    connectionId: string,
    userName: string,
    customerName: string,
}

export interface AsyncActionItem {
    connectionId: string,
    userName: string, 
    operationId: string,
    itemType: number,
    message: string,
    status: number,
    insertDate: Date,
}

export interface BotMessageItem {
    type: "sent" | "received",
    when: Date,
    message: string
}

export const CommunicationApiProxy = DI.createInterface<CommunicationApiProxy>(
    x => x.singleton(CommunicationApiProxyImpl)
);