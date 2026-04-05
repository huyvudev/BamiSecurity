import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibPasswordComponent } from './lib-password.component';

describe('LibPasswordComponent', () => {
  let component: LibPasswordComponent;
  let fixture: ComponentFixture<LibPasswordComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibPasswordComponent]
    });
    fixture = TestBed.createComponent(LibPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
