import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalibrationSearch } from './calibration-search';

describe('CalibrationSearch', () => {
  let component: CalibrationSearch;
  let fixture: ComponentFixture<CalibrationSearch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalibrationSearch]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalibrationSearch);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
