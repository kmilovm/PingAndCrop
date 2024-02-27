import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment'
import { Guid } from 'guid-typescript';
import { PacRequest } from '../models/pacRequest';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class EnqueueService {

  constructor(private http: HttpClient) { }

  fetchData(url:string){
    if (url === null || url === ''){
      console.error("url cannot be null or empty");
      return;
    }
    const controller : string = 'operations/enqueueRequest';
    const pacRequest: PacRequest = {
      Id: Guid.create().toString(),
      UserId:  localStorage.getItem('userId') ?? "",
      RequestedUrl: url
    };
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    this.http.post(environment.apiUrl + controller, pacRequest, { headers: headers }).subscribe({
      next: (res) => console.log('HTTP response', res),
      error: (err) => console.log('HTTP Error', err),
      complete: () => console.log('HTTP request completed.'),
    });
  }
}
