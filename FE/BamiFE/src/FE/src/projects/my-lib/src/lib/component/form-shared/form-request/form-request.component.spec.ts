import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRequestComponent } from './form-request.component';

describe('FormRequestComponent', () => {
  let component: FormRequestComponent;
  let fixture: ComponentFixture<FormRequestComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FormRequestComponent]
    });
    fixture = TestBed.createComponent(FormRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
