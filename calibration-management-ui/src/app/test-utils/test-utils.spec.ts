import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestUtils } from './test-utils';

describe('TestUtils', () => {
  let component: TestUtils;
  let fixture: ComponentFixture<TestUtils>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestUtils]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestUtils);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
