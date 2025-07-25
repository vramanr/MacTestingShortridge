import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TechnicianManagement } from './technician-management';

describe('TechnicianManagement', () => {
  let component: TechnicianManagement;
  let fixture: ComponentFixture<TechnicianManagement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TechnicianManagement]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TechnicianManagement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
