import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueDataResponsesComponent } from './queue-data-requests.component';

describe('QueueDataRequestsComponent', () => {
  let component: QueueDataResponsesComponent;
  let fixture: ComponentFixture<QueueDataResponsesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueDataResponsesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueueDataResponsesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
