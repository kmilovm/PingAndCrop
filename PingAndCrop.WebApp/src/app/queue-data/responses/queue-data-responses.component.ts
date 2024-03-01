import { Component, ViewChild, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { QueueDataService } from '../../services/queue-data.service';
import { MatTableDataSource,  MatTableModule } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { PacResponse } from '../../models/pacResponse';

@Component({
  selector: 'app-queue-data-responses',
  standalone: true,
  imports: [MatExpansionModule, MatTableModule, MatPaginator, CommonModule],
  templateUrl: './queue-data-responses.component.html',
  styleUrl: './queue-data-responses.component.css'
})

export class QueueDataResponsesComponent implements OnInit {
  dataSource:MatTableDataSource<PacResponse> = new MatTableDataSource<PacResponse>([]);
  displayedColumns: string[] = ['MessageId', 'CroppedResponse'];
  panelOpenState = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private dataService: QueueDataService) {}

  ngOnInit() {
    this.dataService.fetchDataResponses().subscribe({
      next: (response:Array<PacResponse>) => {
        this.dataSource = new MatTableDataSource<PacResponse>(response);
        this.dataSource.paginator = this.paginator;
        console.log('Fetched dataResponses:', response, "Date:",Date.now());
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
