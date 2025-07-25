import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderSearch } from './order-search';

describe('OrderSearch', () => {
  let component: OrderSearch;
  let fixture: ComponentFixture<OrderSearch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderSearch]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderSearch);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
