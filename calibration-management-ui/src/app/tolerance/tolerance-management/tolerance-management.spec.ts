import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToleranceManagement } from './tolerance-management';

describe('ToleranceManagement', () => {
  let component: ToleranceManagement;
  let fixture: ComponentFixture<ToleranceManagement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ToleranceManagement]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ToleranceManagement);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
