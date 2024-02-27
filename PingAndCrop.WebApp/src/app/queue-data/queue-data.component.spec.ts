import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueueDataComponent } from './queue-data.component';

describe('QueueDataComponent', () => {
  let component: QueueDataComponent;
  let fixture: ComponentFixture<QueueDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueueDataComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(QueueDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
