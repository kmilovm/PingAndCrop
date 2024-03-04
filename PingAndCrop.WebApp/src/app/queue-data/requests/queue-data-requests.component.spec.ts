import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueDataRequestsComponent } from './queue-data-requests.component';

describe('QueueDataRequestsComponent', () => {
  let component: QueueDataRequestsComponent;
  let fixture: ComponentFixture<QueueDataRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueDataRequestsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueueDataRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
