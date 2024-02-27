import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';

export class SignalRConnectionInfo {
  url: string = environment.signalREndpoint;
  accessKey: string = environment.signalRAccessKey;
}

@Injectable()
export class SignalrService {
  public initializeSignalRConnection(): void {
    let connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.signalREndpoint)
      .configureLogging(signalR.LogLevel.Debug)
      .build();

    connection.start()
      .then(() => {
        console.log("connection.start");
      })
      .catch((error) => {
        console.log("connection start error", error);
      });

    connection.on("send", data => {
      console.log(data);
    });
  }
}
