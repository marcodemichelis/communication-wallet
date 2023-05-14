import { css, html }  from "@microsoft/fast-element";
import { FASTElementLayout } from '@microsoft/fast-router';
import { CommunicationWallet } from '../components/communication/communication-wallet';

CommunicationWallet;

export const pageLayout = new FASTElementLayout(
  html`
    <div class="grid-container">
      <div class="item1">
        <img class="logo" src="/assets/images/asyncGuys.png">
        <h1>CODE GAMES 2023 - Communication Wallet DEMO</h1>
      </div>
      <!-- <div class="item2">Menu</div> -->
      <div class="item3"><slot></slot></div>
      <div class="item4 communicationSection">
        <communication-wallet></communication-wallet>
      </div>
    </div>
  `,
  css`
    :host {
      
    }
    .logo {
      position: absolute;
      top: 0px;
      left: 0px;
      height: 70px;
    }
    .communicationSection {
      position: relative;
    }

    .item1 { grid-area: header; }
/*    .item2 { grid-area: menu; }*/
    .item3 { grid-area: main; }
    .item4 { grid-area: footer; }

    .grid-container {
      display: grid;
      height: 100%;
      grid-template-areas:
        'header header header header header header'
        'main main main main main main'
        'footer footer footer footer footer footer';
      //gap: 10px;
      background-color: #2196F3;
      //padding: 10px;
    }

    .grid-container > div {
      background-color: rgba(255, 255, 255, 0.8);
      text-align: center;
      padding: 0px;
    }
    .item3 {
      overflow-y: auto;
    }
  `
);