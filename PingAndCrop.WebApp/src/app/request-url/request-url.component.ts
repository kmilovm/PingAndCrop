import { Component } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { EnqueueService } from '../services/enqueue.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-request-url',
  standalone: true,
  imports: [MatInputModule, MatButton, MatFormFieldModule, FormsModule],
  templateUrl: './request-url.component.html',
  styleUrl: './request-url.component.css',
})
export class RequestUrlComponent {
  constructor(private enqService: EnqueueService) {}

  url:string = '';

  submit() {
    console.log("url", this.url);
    this.enqService.fetchData(this.url);
  }
}
