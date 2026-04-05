import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibCheckboxComponent } from './lib-checkbox.component';

describe('LibCheckboxComponent', () => {
  let component: LibCheckboxComponent;
  let fixture: ComponentFixture<LibCheckboxComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibCheckboxComponent]
    });
    fixture = TestBed.createComponent(LibCheckboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
