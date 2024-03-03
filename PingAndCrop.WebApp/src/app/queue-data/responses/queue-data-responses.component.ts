import { Component, ViewChild, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { QueueDataService } from '../../services/queue-data.service';
import { MatTableDataSource,  MatTableModule } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { PacResponse } from '../../models/pacResponse';
import { DetailsDialogComponent } from '../../details-dialog/details-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { Observable, timer } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-queue-data-responses',
  standalone: true,
  imports: [MatExpansionModule, MatTableModule, MatPaginator, CommonModule, MatButtonModule],
  templateUrl: './queue-data-responses.component.html',
  styleUrl: './queue-data-responses.component.css'
})

export class QueueDataResponsesComponent implements OnInit {
  dataSource:MatTableDataSource<PacResponse> = new MatTableDataSource<PacResponse>([]);
  displayedColumns: string[] = ['Id', 'Url', 'CroppedResponse'];
  panelOpenState = true;
  countdown$: Observable<number> | undefined;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private dataService: QueueDataService, private dialog: MatDialog) {}

  ngOnInit() {
    this.dataService.fetchDataResponses().subscribe({
      next: (response:Array<PacResponse>) => {
        this.dataSource = new MatTableDataSource<PacResponse>(response);
        this.dataSource.paginator = this.paginator;
        this.startCountdown();
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

  startCountdown() {
    const countdownLength = environment.QueryIntervalInMinutes * 60;
    this.countdown$ = timer(0, 1000).pipe(
      map(i => countdownLength - i),
      take(countdownLength + 1)
    );
  }

  openDialog(request:any): void {
    this.dialog.open(DetailsDialogComponent, {
      width: '600px',
      data: {
        url: request.Url,
        details: request.CroppedResponse
      },
    });
  }
}
