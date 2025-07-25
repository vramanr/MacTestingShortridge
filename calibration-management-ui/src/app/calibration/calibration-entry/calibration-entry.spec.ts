import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalibrationEntry } from './calibration-entry';

describe('CalibrationEntry', () => {
  let component: CalibrationEntry;
  let fixture: ComponentFixture<CalibrationEntry>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalibrationEntry]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalibrationEntry);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
