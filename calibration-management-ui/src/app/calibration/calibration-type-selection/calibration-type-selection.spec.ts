import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalibrationTypeSelection } from './calibration-type-selection';

describe('CalibrationTypeSelection', () => {
  let component: CalibrationTypeSelection;
  let fixture: ComponentFixture<CalibrationTypeSelection>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalibrationTypeSelection]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalibrationTypeSelection);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
