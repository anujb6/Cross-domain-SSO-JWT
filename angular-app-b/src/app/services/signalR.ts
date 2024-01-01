import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: HubConnection;
  private latestTokenSubject = new Subject<string>();

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7225/websocket')
      .build();

    this.registerReceiveTokenHandler();
  }

  async startConnection() {
    await this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.log('Error while starting SignalR connection: ', err));
  }

  stopConnection() {
    this.hubConnection.stop()
      .then(() => console.log('SignalR connection stopped'))
      .catch(err => console.log('Error while stopping SignalR connection: ', err));
  }

   registerReceiveTokenHandler() {
    this.hubConnection.on('ReceiveToken', (token: string) => {
      console.log('Received token:', token);
      localStorage.setItem('AuthToken', token);
    });
  }

   invokeServerMethod() {
    this.hubConnection.invoke('SendTokenToClient').catch(err => console.error(err));
  }

  onReceiveToken(): Observable<string> {
    return this.latestTokenSubject.asObservable();
  }
}
