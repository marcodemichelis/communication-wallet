import { RouterConfiguration, Route } from '@microsoft/fast-router';
import { pageLayout } from './layouts/page-layout';
import { HomeScreen } from './components/home/home-screen';

type RouteSettings = {
  public?: boolean
};

export class AppRouterConfiguration extends RouterConfiguration<RouteSettings> {
  public configure() {
    this.title = 'Communication Wallet';
    this.defaultLayout = pageLayout;
    this.routes.map( 
      { path: '', redirect: 'homeScreen' },
      { path: 'home-screen', title: 'Home', element: HomeScreen, name: 'homeScreen' },
      //{ path: 'not-found', title: 'Not Found', element: NotFound }
    );

    this.routes.fallback(
      () => ({ redirect: 'homeScreen' })
    );

    this.contributors.push({
      navigate(phase) {
        const settings = phase.route.settings;
  
        if (settings && settings.public) {
          return;
        }
      }
    });
  }
}