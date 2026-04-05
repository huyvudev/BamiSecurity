import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibSelectButtonComponent } from './lib-select-button.component';

describe('LibSelectButtonComponent', () => {
  let component: LibSelectButtonComponent;
  let fixture: ComponentFixture<LibSelectButtonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibSelectButtonComponent]
    });
    fixture = TestBed.createComponent(LibSelectButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
