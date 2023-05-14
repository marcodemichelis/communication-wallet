import { FASTElement, customElement, html, css } from '@microsoft/fast-element';
import { FASTRouter } from '@microsoft/fast-router';
import { AppRouterConfiguration } from './routes';

FASTRouter;

const template = html<MainApp>`
    <fast-router :config=${x => x.routerConfiguration}></fast-router>
`;

const styles = css`
  :host {
    /*contain: content;*/
  }

  :host, fast-router {  
    display: block;
    width: 100%;
    height: 100%;
  }
`;

@customElement({
  name: 'main-app',
  template,
  styles
})
export class MainApp extends FASTElement {
  routerConfiguration = new AppRouterConfiguration();
}