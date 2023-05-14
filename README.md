Communication Wallet
======================

This project uses technologies as websocket signalR to demonstrate the efficiency and efficacy of the asynchronous way in an application.
----------------------

Used Technologies in the project:

- Framework backend (webapi, ...)	-> **Asp.net 7.0**, backend part (web and background processes)  
- Libreria per websocket -> **Microsoft SignalR**  
- Websocket service -> **Azure SignalR service as backplane** (in the code it uses the appservice host as signalR service provider)  
- Queue system -> **Azure Service Bus** (in the code it uses in-memory queues)  
- Storage files e tabelle -> **Azure Storage account** (in the code so far it uses in-memory collections and static files)  
- Hosting applicazione -> **Azure App Service**  
- Tecnologia client -> **WebComponents**  
- Framework client -> **Webpack, Typescript e Libreria Microsoft Fast**  
