import { TestBed } from '@angular/core/testing';

import { EnqueueService } from './enqueue.service';

describe('EnqueueService', () => {
  let service: EnqueueService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EnqueueService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
