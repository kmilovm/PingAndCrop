import { TestBed } from '@angular/core/testing';

import { QueueDataService } from './queue-data.service';

describe('QueueDataService', () => {
  let service: QueueDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QueueDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
