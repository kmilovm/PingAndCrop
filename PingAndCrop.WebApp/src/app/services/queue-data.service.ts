import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, timer } from 'rxjs';
import { switchMap, catchError, map } from 'rxjs/operators';
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
  private intervalMinutes = 0.5; // Fetch data every X minutes

  constructor(private http: HttpClient) {}

  public fetchDataRequests(): Observable<Array<PacRequest>> {
    return this.fetchDataBase<PacRequest>(this.apiUrl + this.queueInRoute);
  }

  public fetchDataResponses(): Observable<Array<PacResponse>> {
    return this.fetchDataBase<PacResponse>(this.apiUrl + this.queueOutRoute)
  }

  private fetchDataBase<T>(url: string): Observable<Array<T>> {
    const uniqueParam = new Date().getTime();
    return timer(0, this.intervalMinutes * 60 * 1000).pipe(
      switchMap(() => this.http.get<Array<T>>(url+ `?cacheBuster=${uniqueParam}`)),
      catchError((error) => {
        console.error('Error fetching data:', error);
        return [];
      })
    );
  }
}
