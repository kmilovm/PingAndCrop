import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, timer } from 'rxjs';
import { switchMap, catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { PacResponse } from '../models/pacResponse';
import { PacRequest } from '../models/pacRequest';

@Injectable({
  providedIn: 'root'
})
export class QueueDataService {

  private apiUrl = environment.BaseApiUrl; // Replace with your API URL
  private queueInRoute = environment.QueueInRoute;
  private queueOutRoute = environment.QueueOutRoute;
  private intervalMinutes = 3; // Fetch data every 5 minutes

  constructor(private http: HttpClient) {}

  public fetchDataRequests(): Observable<PacRequest> {
    return this.fetchDataBase(this.apiUrl + this.queueInRoute);
  }

  public fetchDataResponses(): Observable<PacResponse> {
    return this.fetchDataBase(this.apiUrl + this.queueOutRoute)
  }

  private fetchDataBase<T>(url: string): Observable<any> {
    return timer(0, this.intervalMinutes * 60 * 1000).pipe(
      switchMap(() => this.http.get(url)),
      catchError((error) => {
        console.error('Error fetching data:', error);
        return [];
      })
    );
  }
}
