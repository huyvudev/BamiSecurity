import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibToggleButtonComponent } from './lib-toggle-button.component';

describe('LibToggleButtonComponent', () => {
  let component: LibToggleButtonComponent;
  let fixture: ComponentFixture<LibToggleButtonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibToggleButtonComponent]
    });
    fixture = TestBed.createComponent(LibToggleButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
