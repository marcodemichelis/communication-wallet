import { allComponents, provideFASTDesignSystem, fastCombobox } from "@microsoft/fast-components";
import { FASTElement, customElement, attr, html, css, observable, Observable } from "@microsoft/fast-element";
import { CommunicationService } from "../../services/communication-service";
import { Http } from "../../cross-cutting/http";
import { Session } from '../../services/session';
import { CommunicationApiProxy } from "../../services/communication-api-proxy";

provideFASTDesignSystem().register(allComponents);

const template = html<HomeScreen>`
        <div class="operationResultMessage">${x => x.operationResultMessage}</div>
        <fast-card style="width: 300px;">
            <img src="/assets/images/use-case-report.png" />
            <div style="padding: 0 10px 10px; color: var(--neutral-foreground-rest);">
                <h3>Long time Report</h3>
                <p>Use case: frequentemente capita di dover generare report che impiegano un tempo molto lungo, anche alcuni minuti, bloccando il lavoro e obbligando l'utente ad aspettare la fine della processazione</p>
                Cliente:
                <fast-combobox autocomplete="both" @change="${(x, ec) => x.longReportCustomerChanged(ec.event)}" current-value="${x => x.longReportCustomer}">
                    <fast-option>Christopher Eccleston</fast-option>
                    <fast-option>David Tenant</fast-option>
                    <fast-option>Matt Smith</fast-option>
                    <fast-option>Peter Capaldi</fast-option>
                    <fast-option>Jodie Whittaker</fast-option>
                </fast-combobox>
                <fast-button id="longReportInfo" @click="${x => x.requestLongReport()}">Genera Report</fast-button>
            </div>
        </fast-card>
        <fast-card style="width: 300px;">
            <img src="/assets/images/use-case-agenziaentrate.jpg" />
            <div style="padding: 0 10px 10px; color: var(--neutral-foreground-rest);">
                <h3>Long time third parties operation</h3>
                <p>Use case: frequentemente capita di dover spedire documentazione a terze parti, attività che richiede un tempo molto lungo, anche alcuni minuti, bloccando il lavoro e obbligando l'utente ad aspettare la fine della processazione</p>
                Cliente:
                <fast-combobox autocomplete="both" @change="${(x, ec) => x.longTaskCustomerChanged(ec.event)}" current-value="${x => x.longTaskCustomer}">
                    <fast-option>Christopher Eccleston</fast-option>
                    <fast-option>David Tenant</fast-option>
                    <fast-option>Matt Smith</fast-option>
                    <fast-option>Peter Capaldi</fast-option>
                    <fast-option>Jodie Whittaker</fast-option>
                </fast-combobox>
                <fast-button id="longTaskInfo" @click="${x => x.requestLongTask()}">Invia documento</fast-button>
            </div>
        </fast-card>
        <fast-card style="width: 300px;">
            <img src="/assets/images/use-case-broadcasting.png" />
            <div style="padding: 0 10px 10px; color: var(--neutral-foreground-rest);">
                <h3>Broadcast messages</h3>
                <p>Use case: capita di dover spedire un messaggio a tutti gli user connessi, relativo a manutenzioni straordinarie o ad avvisi</p>
                <fast-text-area appearance="filled" cols="45" placeholder="message text" @change="${(x, c) => x.broadcastMessageChanged(c.event)}" :value="${x => x.broadcastMessage}">Messaggio Html</fast-text-area>
                <fast-checkbox @change="${(x, c) => x.broadcastMessageImportantChanged(c.event)}">Importante</fast-checkbox>
                <fast-button id="broadcastMessage" @click="${x => x.sendBroadcastMessage()}">Invia messaggio a tutti</fast-button>
            </div>
        </fast-card>
        <fast-card style="width: 300px;">
            <img src="/assets/images/use-case-marketing.jpg" />
            <div style="padding: 0 10px 10px; color: var(--neutral-foreground-rest);">
                <h3>Contextual Info Push (tutorial or marketing)</h3>
                <p>Use case: durante un'operazione che dura minuti, si potrebbe pensare di "pushare" contenuti relativi all'operazione che si sta eseguendo, come nuovi articoli normativa, leggi, video... 
                oppure anche fare marketing su nuovi pacchetti da acquistare
                <br>
                <fast-button id="articlePush" @click="${x => x.enableArticlesPush()}">Attiva push</fast-button>
                <fast-button id="articlePush" @click="${x => x.disableArticlesPush()}">Disattiva push</fast-button>
            </div>
        </fast-card>
        <fast-card style="width: 300px;">
            <img src="/assets/images/use-case-bot.png" />
            <div style="padding: 0 10px 10px; color: var(--neutral-foreground-rest);">
                <h3>Operations by Bot</h3>
                <p>Use case: data un'opportuna configurazione di backoffice, è possibile da un testo libero far eseguire operazioni ricorrenti su clienti e anni specifici (stampa bilancio 2023 Mario Rossi)
            </div>
        </fast-card>

        <fast-tooltip anchor="longReportInfo">Invia la richiesta asincrona di generazione di un report (durata generazione 30 secondi)</fast-tooltip>
        <fast-tooltip anchor="longTaskInfo">Invia la richiesta asincrona di esecuzione di un long task (durata task 60 secondi)</fast-tooltip>
        <fast-tooltip anchor="broadcastMessage">Invia a tutti gli utenti loggati un messaggio che verrà visualizzato nel wallet,<br>se si seleziona l'opzione 'importante' il wallet verrà aperto in automatico nel browser di tutti gli utenti connessi</fast-tooltip>
        <fast-tooltip anchor="articlePush">Abilita l'invio di articoli e offerte marketing a tutti gli utenti connessi</fast-tooltip>
`;

const styles = css`
    :host {
        display: flex;
    }
    :not(:defined) {
        visibility: hidden;
    }
  
    fast-card {
        display: flex;
        margin-right: 10px;
        padding: 16px;
        flex-direction: column;
    }
    
    h2 {
        font-size: 30px;
        line-height: var(--type-ramp-plus-5-line-height);
    }
    
    #button {
        align-self: flex-end;
    }
    .operationResultMessage {
        position: absolute;
        top: 0px;
        right: 0px;
        text-align: right;
        color: green;
        font-weight: bold;
        background-color: #D3D3D3;
        padding: 5px;
        border: 1px solid gray;
    }
`;

@customElement({
    name: 'home-screen',
    template,
    styles
})
export class HomeScreen extends FASTElement {
    @CommunicationService communicationService!: CommunicationService;
    @Http httpClient: Http;
    @Session session: Session;
    @CommunicationApiProxy communicationApiProxy: CommunicationApiProxy;

    @observable operationResultMessage: string = "";
    @observable broadcastMessage: string = "Si avvisa che tra un'ora circa, alle 11, avverrà un'operazione di manutenzione straordinaria, potrebbero esserci dei rallentamenti, ci scusiamo per il disagio";

    @observable longReportCustomer: string = "David Tenant";
    @observable longTaskCustomer: string = "Christopher Eccleston";
    broadcastMessageImportant:boolean = false;

    connectedCallback() {
        super.connectedCallback();
        console.log('Home screen  is now connected to the DOM');
        this.session.setCurrentUsername(prompt("Username:"));
    }

    showOperationResultMessage(msg: string): void {
        this.operationResultMessage = msg;
        setTimeout(() => this.operationResultMessage = "", 3000);
    }
    longReportCustomerChanged(event: Event): void {
        if (!event || !event.target) return;
        this.longReportCustomer = (<any>event.target).value;
    }
    longTaskCustomerChanged(event: Event): void {
        if (!event || !event.target) return;
        this.longTaskCustomer = (<any>event.target).value;
    }
    broadcastMessageChanged(event: Event): void {
        if (!event || !event.target) return;
        this.broadcastMessage = (<any>event.target).value;
    }
    broadcastMessageImportantChanged(event: Event): void {
        this.broadcastMessageImportant = (<any>event.target).checked;
    }

    requestLongReport(): void {
        this.communicationApiProxy
            .requestLongReport({ 
                connectionId: this.communicationService.currentConnectionId(),
                userName: this.session.username,
                customerName: this.longReportCustomer,
            })
            .then(r => this.showOperationResultMessage("Richiesta REPORT inserita con successo!"));
    }
    requestLongTask(): void {
        this.communicationApiProxy
            .requestLongTask({ 
                connectionId: this.communicationService.currentConnectionId(),
                userName: this.session.username,
                customerName: this.longTaskCustomer,
            })
            .then(r => this.showOperationResultMessage("Richiesta TASK inserita con successo!"));
    }
    sendBroadcastMessage(): void {
        this.communicationService.sendBroadcastMessage({
            important: this.broadcastMessageImportant,
            message: this.broadcastMessage,
        });
    }
    enableArticlesPush(): void {
        this.communicationApiProxy.enableArticlesPush();
    }
    disableArticlesPush(): void {
        this.communicationApiProxy.disableArticlesPush();
    }

    askToInterpreter(): void {
        this.communicationService.askToInterpreter("Vorrei il report delle vendite del cliente Giampetruzzi Flavio dal 1/1/2023 al 30/06/2023");
    }
}