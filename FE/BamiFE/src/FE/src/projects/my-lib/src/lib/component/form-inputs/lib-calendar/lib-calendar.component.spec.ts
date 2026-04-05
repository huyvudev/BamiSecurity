import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibCalendarComponent } from './lib-calendar.component';

describe('LibCalendarComponent', () => {
  let component: LibCalendarComponent;
  let fixture: ComponentFixture<LibCalendarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibCalendarComponent]
    });
    fixture = TestBed.createComponent(LibCalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
