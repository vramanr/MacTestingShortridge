import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalibrationEdit } from './calibration-edit';

describe('CalibrationEdit', () => {
  let component: CalibrationEdit;
  let fixture: ComponentFixture<CalibrationEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalibrationEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalibrationEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
