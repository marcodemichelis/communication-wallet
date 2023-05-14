import { allComponents, provideFASTDesignSystem } from "@microsoft/fast-components";
import { FASTElement, customElement, attr, html, css, observable, repeat } from "@microsoft/fast-element";
import { BroadcastMessage, CommunicationService, CommunicationServiceStatus } from "../../services/communication-service";
import moment from "moment";
import { Session } from "../../services/session";
import { AsyncActionItem, BotMessageItem, CommunicationApiProxy } from "../../services/communication-api-proxy";
import { appConfig } from "../../configuration";

provideFASTDesignSystem().register(allComponents);

const template = html<CommunicationWallet>`
    <div class="expander ${x => x.isOpen ? 'on' : ''}"></div>
    <div class="mainPanel">
        <div class="expandedArea" ?hidden="${x => !x.isOpen}">
            <div class="paper-tear"></div>
            <div class="leftPanel">
                    ${repeat(x => x.asyncActions, html<AsyncActionItem>`
                        <div class="messageItemBox ${i => i.itemType == 0 ? '' : 'hidden'}"><b>${a => moment(a.insertDate).format("DD/MM/YYYY HH:mm:ss")}</b> | <i><span :innerHTML="${(a, c) => c.parent.buildMessage(a.message)}"></span></i> | <div class="actionBadge status${x => x.status}"></div></div>
                        <div class="messageItemBox ${i => i.itemType != 0 ? 'type' + i.itemType : 'hidden'}"><b>${a => moment(a.insertDate).format("DD/MM/YYYY HH:mm:ss")}</b> | <i><span :innerHTML="${(a, c) => a.message}"></span></i></div>
                    `)}
            </div>
            <div class="rightPanel">
                <div class="interpretertxt">
                    <fast-text-field
                        appearance="outline"
                        placeholder="Chiedimi qualcosa, e proverò ad aiutarti"
                        @change="${(x, c) => x.interpreterRequestTextChanged(c.event)}" :value="${x => x.interpreterRequestText}"
                        @keyup="${(x, e) => x.onAskInputKeyUp(e.event)}"></fast-text-field>
                    <fast-button href="#" @click="${x => x.askToInterpreter()}" appearance="lightweight">
                        <img src="/assets/images/send.png" width="34" height="34" />
                    </fast-button>
                </div>
                <div class="messageBotContainer">
                ${repeat(x => x.botMessages, html<BotMessageItem>`
                    <div class="messageBot ${a => a.type}">
                        <div class="msg-bubble">
                            <b>${a => moment(a.when).format("HH:mm")}</b> <i><span :innerHTML="${a => a.message}"></span></i>
                        </div>
                    </div>
                `)}
                </div>
            </div>
            <div class="broadcastMessage ${x => x.broadcastMessage !== "" ? "on" : ""}" :innerHTML="${x => x.broadcastMessage}"></div>
        </div>
        <!--<fast-divider></fast-divider>-->
        <div class="collapsedArea">
            <div class="cornerPeel ${x => x.isOpen ? "off" : ""}">
                <div class="taskItem arrow ${x => x.isOpen ? 'down' : 'up'}" @click="${x => x.togglePanel()}"></div>
                <div class="taskItem alertMessageItem ${x => x.isAlertMessageActive ? 'on' : ''}" @click="${x => x.isAlertMessageActive ? x.togglePanel() : null}"></div>
                <div class="taskItem connectionItem status${x => x.connectionStatus}" title="${x => x.connectionStatusName}"></div>
            </div>
        </div>
    </div>
`;

const styles = css`
    :host {
        z-index: 1000;
        text-align: left;
    }
    .mainPanel {
        position: absolute;
        bottom: 0px;
        left: 0px;
        right: 0px;
    }
    .leftPanel {
        color: black;
        height: 250px;
        overflow-y: auto;
        font-family: aktive-grotesk, "Segoe UI";
        padding-top: 20px;
        padding-bottom: 10px;
        padding-left: 10px;
        padding-right: 10px;
        width: 50%;
        float: left;
    }
    .rightPanel{
        color: black;
        height: 250px;
        overflow-y: auto;
        font-family: aktive-grotesk, "Segoe UI";
        padding-top: 15px;
        padding-bottom: 10px;
        padding-right: 10px;
        padding-left: 10px;
        width: auto;
        background-image: url("/assets/images/chat-small.png");
        background-repeat: no-repeat;
        background-position: center 55px;
    }
    .interpretertxt {
        display: flex;
    }
    .interpretertxt > fast-text-field {
        margin-top:20px;
        width: 100%;
    }
    .interpretertxt > fast-button {
        margin-top: 20px;
    }
    .taskItem {
        float: right;
        height: 23px;
        width: 26px;
        padding-left: 5px;
        padding-right: 5px;
        padding-top: 3px;
        border-right: solid 1px grey;
        background-repeat: no-repeat;
        background-position: center;
        margin-top: 120px;
    }
    .taskItem.arrow {
        cursor: pointer;
    }
    .taskItem.arrow.up {
        background-image: url("/assets/images/arrow-up.png");
    }
    .taskItem.arrow.down {
        background-image: url("/assets/images/arrow-down.png");
    }
    .connectionItem.status0
    {
        background-image: url("/assets/images/red-circle.png");
    }
    .connectionItem.status1,
    .connectionItem.status3
    {
        background-image: url("/assets/images/yellow-circle.png");
    }
    .connectionItem.status2
    {
        background-image: url("/assets/images/green-circle.png");
    }
    .alertMessageItem {
        background-image: url("/assets/images/bell-transparent-grey.gif");
    }
    .alertMessageItem.on {
        cursor: pointer;
        background-image: url("/assets/images/bell-transparent.gif");
    }
    .collapsedArea {
        text-align: right;
    }
    .cornerPeel {
        position: absolute;
        bottom: 0px;
        right: 0px;
        width: 200px;
        height: 150px;
        background-image: url("/assets/images/corner-peel-with-bg.png");
        background-repeat: no-repeat;
        background-position: right;
    }
    .cornerPeel.off {
        background-image: none;
        height: 26px;
    }
    .cornerPeel.off .taskItem {
        margin-top: -4px;
    }
    .expander {
        height: 300px;
        display: none;
    }
    .expander.on {
        display: block;
    }
    .expandedArea {
        height: 300px;
        background-color: #EEEEEE;
    }
    .broadcastMessage {
        position: absolute;
        left: 0px;
        right: 120px;
        bottom: 0px;
        height: 26px;
        color: red;
        font-weight: bold;
        border-top: 1px solid gray;
        text-align: center;
    }
    .broadcastMessage.on {
        animation: blinking 1s infinite;
    }
    @keyframes blinking {
        0% {
            background-color: #f8f398;
        }
        100% {
            background-color: #d9dad7;
        }
    }
    .paper-tear {
        position: absolute;
        top: -30px;
        left: 0px;
        right: 0px;
        height: 80px;
        background-image: url("/assets/images/paper-tear-02.png");
        background-repeat: no-repeat;
        background-position: top left;
    }
    .collapsedArea a {
        cursor: pointer;
    }
    .disconnected {
        background-image: url("/assets/images/red-circle.png");
        background-repeat: no-repeat;
        height: 26px;
    }
    .messageItemBox {
        position: relative;
        padding: 5px;
        background-color: white;
        border-radius: 8px;
        box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        margin-top: 15px;
        padding-right: 125px;
    }
    .messageItemBox.type2 {
        background-color: #87bdd8;
        border-radius: 0px;
    }
    .messageItemBox.type3 {
        background-color: #ffef96;
        border-radius: 0px;
    }
    .messageItemBox.hidden {
        display: none;
    }
    .actionBadge {
        position: absolute;
        right: 5px;
        top: 5px;
        //float: right;
        border-radius: 10px;
        width: 120px;
        height: 20px;
        text-align: center;
        font-weight: bold;
    }
    .actionBadge.status0::before {
        content: "enqueued";
    }
    .actionBadge.status0 {
        background-color: black;
        color: white;
    }
    .actionBadge.status1::before {
        content: "progress";
        margin-right: 8px;
    }
    .actionBadge.status1 {
        background-color: white;
        background-image: url(/assets/images/3-dots-small-transparent.gif);
        background-repeat: no-repeat;
        background-position: center right;
        color: black;
        border: 1px solid grey;
    }
    .actionBadge.status2::before {
        content: "finished";
    }
    .actionBadge.status2 {
        background-color: green;
        color: white;
    }
    .actionBadge.status3::before {
        content: "error";
    }
    .actionBadge.status3 {
        background-color: red;
        color: white;
    }
    .messageBotContainer{
        flex: 1;
        overflow-y: auto;
        padding: 10px;
    }
    .messageBot {
        display: flex;
        align-items: flex-end;
        margin-bottom: 10px;
    }
    .messageBot.sent {
    }
    .messageBot.sent {
        flex-direction: row-reverse;
    }
    .msg-bubble {
        max-width: 450px;
        padding: 15px;
        border-radius: 15px;
        background-color: #87bdd8;
    }
    .sent .msg-bubble {
        background-color: #d9ecd0;
        border-bottom-right-radius: 0;
    }
`;

@customElement({
    name: 'communication-wallet',
    template,
    styles
})
export class CommunicationWallet extends FASTElement {
    @CommunicationService communicationService!: CommunicationService;
    @Session session: Session;
    @CommunicationApiProxy communicationApiProxy: CommunicationApiProxy;

    @observable isOpen: boolean = false;
    @observable isAlertMessageActive: boolean = false;
    @observable connectionStatus: number = CommunicationServiceStatus.Disconnected;
    @observable connectionStatusName: string = "Disconnesso";
    @observable asyncActions: AsyncActionItem[];
    @observable botMessages: BotMessageItem[]= [];
    @observable broadcastMessage: string = "";
    @observable interpreterRequestText: string = "";


    connectedCallback() {
        super.connectedCallback();
        console.log('communication-wallet is now connected to the DOM');
        this.communicationService
            .getBotResponses()
            .subscribe((botItem :BotMessageItem) => {
                this.botMessages = [botItem, ...this.botMessages];
            });
        this.communicationService
            .getConnectionStatuses()
            .subscribe((status: number) => {
                this.connectionStatus = status;
                this.connectionStatusName = (status === CommunicationServiceStatus.Disconnected ? "Wallet Disconnesso" : 
                    status === CommunicationServiceStatus.Connected ? "Wallet Connesso" : "Wallet in Connessione...")
            });
        this.communicationService
            .getNewChanges()
            .subscribe((_: number) => {
                this.communicationApiProxy
                    .getAsyncActionItems(this.session.username)
                    .then(r => {
                        this.asyncActions = r;
                        if (!this.isOpen)
                            this.isAlertMessageActive = true;
                    });
            });
        this.communicationService
            .getBroadcastMessages()
            .subscribe((item: BroadcastMessage) => {
                this.broadcastMessage = item.message;
                if (item.important) {
                    this.isOpen = true;
                }
                else if (!this.isOpen)
                    this.isAlertMessageActive = true;
            });
        this.communicationService.connect();

        this.communicationApiProxy
            .getAsyncActionItems(this.session.username)
            .then(r => this.asyncActions = r);
    }

    togglePanel(){
        this.isOpen ? this.closePanel() : this.openPanel();
    }
    openPanel(){
        this.isOpen = true;
        this.isAlertMessageActive = false;
    }
    closePanel(){
        this.isOpen = false;
    }
    sendMessage(msg: string){
        this.communicationService.echoMessage(msg);
    }
    buildMessage(message: string) :string {
        return message.replace("[beBaseUrl]", appConfig.beBaseUrl);
    }

    interpreterRequestTextChanged(event: Event): void {
        if (!event || !event.target) return;
        this.interpreterRequestText = (<any>event.target).value;
    }

    askToInterpreter(){
        if (this.interpreterRequestText)
        {
            const botItem: BotMessageItem = {
                when: new Date(),
                type: "sent",
                message: this.interpreterRequestText
            }
            this.botMessages = [botItem, ...this.botMessages]
            this.communicationService.askToInterpreter(this.interpreterRequestText);
            this.interpreterRequestText = "";
        }
    }

    onAskInputKeyUp(event: Event) {
        if ((<KeyboardEvent>event).key === 'Enter') {
            this.askToInterpreter();
        }
    }
}
