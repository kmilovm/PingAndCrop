import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RequestUrlComponent } from './request-url/request-url.component';
import { HttpClientModule } from '@angular/common/http';
import { SignalrService } from './services/signalr.service';
import { Guid } from 'guid-typescript';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet, RequestUrlComponent, HttpClientModule],
    providers: [SignalrService]
})
export class AppComponent implements OnInit {
  constructor(private signalrService: SignalrService) {
    localStorage.setItem('userId', Guid.create().toString());
  }
  title = 'PingAndCrop';

  ngOnInit() {
    this.signalrService.initializeSignalRConnection();
  }
}


