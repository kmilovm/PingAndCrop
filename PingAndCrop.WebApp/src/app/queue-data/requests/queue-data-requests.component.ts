import { Component, ViewChild, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { QueueDataService } from '../../services/queue-data.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { PacRequest } from '../../models/pacRequest';

@Component({
  selector: 'app-queue-data-requests',
  standalone: true,
  imports: [MatExpansionModule, MatTableModule, MatPaginator, CommonModule],
  templateUrl: './queue-data-requests.component.html',
  styleUrl: './queue-data-requests.component.css',
})
export class QueueDataRequestsComponent implements OnInit {
  dataSource: MatTableDataSource<PacRequest> = new MatTableDataSource<PacRequest>([]);
  displayedColumns: string[] = ['Id', 'RequestedUrl'];
  panelOpenState = true;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  constructor(private dataService: QueueDataService) {}

  ngOnInit() {
    this.dataService.fetchDataRequests().subscribe({
      next: (response: Array<PacRequest>) => {
        this.dataSource = new MatTableDataSource<PacRequest>(response);
        this.dataSource.paginator = this.paginator;
        console.log('Fetched dataRequests:', response, "Date:",Date.now());
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }
}
