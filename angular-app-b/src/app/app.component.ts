import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signalR';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'angular-app-b';

  constructor(
    private signalRService: SignalRService
  ) { }

  async ngOnInit() {
    debugger;
    await this.signalRService.startConnection();
    this.signalRService.invokeServerMethod();
  }
}
