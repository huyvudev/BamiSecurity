import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibAutoCompleteComponent } from './lib-auto-complete.component';

describe('LibAutoCompleteComponent', () => {
  let component: LibAutoCompleteComponent;
  let fixture: ComponentFixture<LibAutoCompleteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibAutoCompleteComponent]
    });
    fixture = TestBed.createComponent(LibAutoCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
