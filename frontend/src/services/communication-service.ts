import { Observable, Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { DI } from '@microsoft/fast-foundation';
import { appConfig } from '../configuration';
import { Session } from './session';
import { BotMessageItem } from './communication-api-proxy';

export interface CommunicationService {
    connect(): void;
    disconnect(): void;
    getConnectionStatuses(): Observable<number>;
    getMessages(): Observable<number>;
    getBotResponses(): Observable<BotMessageItem>;
    getNewChanges(): Observable<number>;
    getBroadcastMessages(): Observable<BroadcastMessage>;
    askToInterpreter(msg: string): void;
    echoMessage(msg: string): void;
    sendBroadcastMessage(broadcastMessage: BroadcastMessage): void;
    currentConnectionId() : string;
}

export class CommunicationServiceImpl implements CommunicationService {
    private messages$: Subject<number>;
    private newBroadcastMessages$: Subject<BroadcastMessage>;
    private newBotResponse$: Subject<BotMessageItem>;
    private newChanges$: Subject<number>;
    private statuses$: Subject<number>;
    private connection: signalR.HubConnection;
    private hubName = 'communication-hub';
    private hubUrl: string = `${appConfig.beBaseUrl}/${this.hubName}`;
    private connectionId: string;

    private newRequestResultReceivedSubscription = 'newRequestResultReceived';
    private newMessageResultReceivedSubscription = 'newMessageResultReceived';
    private newChangesForUserReceivedSubscription = 'newChangesForUserReceived';
    private newBroadcastMessagesReceivedSubscription = 'newBroadcastMessagesReceived';
    private newInterpreterResponseSubscription = 'newInterpreterResponseReceived';
    

    private echoSubscription = 'echo';
    @Session session: Session;

    constructor() {
        this.messages$ = new Subject<number>();
        this.newBotResponse$ = new Subject<BotMessageItem>();
        this.newBroadcastMessages$ = new Subject<BroadcastMessage>();
        this.newChanges$ = new Subject<number>();
        this.statuses$ = new Subject<number>();
        this.connection = new signalR.HubConnectionBuilder()
            .configureLogging(signalR.LogLevel.Information)
            .withUrl(`${this.hubUrl}`)  // 'https://localhost:44353'
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: retryContext => {
                    return 10000;
                }
            })
            .build();
        this.connection.onreconnecting((error) => {
            this.statuses$.next(CommunicationServiceStatus.Reconnecting);
            console.warn(`Communication Wallet Connection lost due to error "${error}". Reconnecting...`);
        });
        this.connection.onreconnected((connectionId) => {
            this.statuses$.next(CommunicationServiceStatus.Connected);
            console.info(`Communication Wallet Connection reestablished. Connected with connectionId "${connectionId}".`);
            this.registerSignalRCallbacks();
            this.getConnectionId();
        })
        ;
        this.connection.onclose((err) => {
            this.statuses$.next(CommunicationServiceStatus.Disconnected);
            if (err !== undefined)
                console.error(`Communication Wallet Connection closed due to error "${err}". Try refreshing this page to restart the connection.`);
            else
                console.info(`Communication Wallet Connection closed`);
        });
    }
    public connect() {
        this.connection.baseUrl += `?username=${this.session.username}`;
        this.statuses$.next(CommunicationServiceStatus.Connecting);
        this.connection.start()
            .then(() => {
                this.statuses$.next(CommunicationServiceStatus.Connected);
                console.log('Communication Wallet SignalR Connected!');
                this.registerSignalRCallbacks();
                console.log('Communication Wallet SignalR callbacks registration Done!');
            })
            .then(() => this.getConnectionId())
            .catch((err) => {
                this.statuses$.next(CommunicationServiceStatus.Disconnected);
                console.error(err.toString());
            });
    }
    public getMessages(): Observable<number> {
        return this.messages$.asObservable();
    }
    public getBroadcastMessages(): Observable<BroadcastMessage> {
        return this.newBroadcastMessages$.asObservable();
    }
    public getBotResponses(): Observable<BotMessageItem> {
        return this.newBotResponse$.asObservable();
    }
    public getNewChanges(): Observable<number> {
        return this.newChanges$.asObservable();
    }
    public getConnectionStatuses(): Observable<number> {
        return this.statuses$.asObservable();
    }
    public sendBroadcastMessage(broadcastMessage: BroadcastMessage): void {
        this.connection
            .invoke("BroadcastMessage", broadcastMessage.important, broadcastMessage.message)
            .catch((err) => console.error(err.toString()));
    }
    public askToInterpreter(msg: string): void{
        this.connection
            .invoke("AskToInterpreter", msg)
            .catch((err) => console.error(err.toString()));
    }

    public echoMessage(msg: string): void {
        this.connection
            .invoke("Echo", this.session.username, msg)
            .catch((err) => console.error(err.toString()));
    }
    public disconnect() {
        this.connection.stop();
        console.log('Communication Wallet SignalR Disconnected!');
    }
    public currentConnectionId() : string {
        return this.connectionId;
    }
    // Private Methods
    private registerSignalRCallbacks = () => {
        this.connection.off(this.newRequestResultReceivedSubscription);
        this.connection.off(this.newMessageResultReceivedSubscription);
        this.connection.off(this.echoSubscription);
        this.connection.off(this.newChangesForUserReceivedSubscription);
        this.connection.off(this.newInterpreterResponseSubscription);
        this.connection.off(this.newBroadcastMessagesReceivedSubscription);

        this.connection.on(this.newRequestResultReceivedSubscription, (msg) => {
            this.messages$.next(msg);
            console.log(`New request result received: ${JSON.stringify(msg)}`);
        });
        this.connection.on(this.newMessageResultReceivedSubscription, (msg) => {
            this.messages$.next(msg);
            console.log(`New message result received: ${msg}`);
        });
        this.connection.on(this.echoSubscription, (name, msg) => {
            this.messages$.next(msg);
            console.log(`From server: ${name} -> ${msg}`);
        });
        this.connection.on(this.newChangesForUserReceivedSubscription, () => {
            this.newChanges$.next(1);
            console.log(`New changes to load`);
        });
        this.connection.on(this.newInterpreterResponseSubscription, (res) => {
            this.newBotResponse$.next({
                when: new Date(),
                type: "received",
                message: res
            });
        });
        this.connection.on(this.newBroadcastMessagesReceivedSubscription, (important, msg) => {
            this.newBroadcastMessages$.next({
                important,
                message: msg,
            });
            console.log(`New broadcast message`);
        });
    };

    private getConnectionId = () => {
        this.connection
            .invoke('getconnectionid')
            .then((data) => {
                console.log(`connectionId: ${data}`);
                this.connectionId = data;
            });
      }
}

export const CommunicationServiceStatus = {
    Disconnected: 0,
    Connecting: 1,
    Connected: 2,
    Reconnecting: 3,
}
export interface BroadcastMessage {
    important: boolean,
    message: string,
}

export const CommunicationService = DI.createInterface<CommunicationService>(
    x => x.singleton(CommunicationServiceImpl)
);
