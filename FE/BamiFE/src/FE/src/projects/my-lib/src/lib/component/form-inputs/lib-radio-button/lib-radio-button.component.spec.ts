import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibRadioButtonComponent } from './lib-radio-button.component';

describe('LibRadioButtonComponent', () => {
  let component: LibRadioButtonComponent;
  let fixture: ComponentFixture<LibRadioButtonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibRadioButtonComponent]
    });
    fixture = TestBed.createComponent(LibRadioButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
