import { Component, OnInit } from '@angular/core';
import { QueueDataService } from '../services/queue-data.service';

@Component({
  selector: 'app-queue-data',
  standalone: true,
  imports: [],
  templateUrl: './queue-data.component.html',
  styleUrl: './queue-data.component.css'
})
export class QueueDataComponent implements OnInit {
  dataRequests: any;
  dataResponses: any;

  constructor(private dataService: QueueDataService) {}

  ngOnInit() {
    this.dataService.fetchDataRequests().subscribe((data) => {
      this.dataRequests = data;
      console.log('Fetched dataRequests:', this.dataRequests);
    });

    this.dataService.fetchDataRequests().subscribe((data) => {
      this.dataResponses = data;
      console.log('Fetched dataResponses:', this.dataResponses);
    });
  }
}
