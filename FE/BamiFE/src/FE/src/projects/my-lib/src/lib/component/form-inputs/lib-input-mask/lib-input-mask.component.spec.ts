import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibInputMaskComponent } from './lib-input-mask.component';

describe('LibInputMaskComponent', () => {
  let component: LibInputMaskComponent;
  let fixture: ComponentFixture<LibInputMaskComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibInputMaskComponent]
    });
    fixture = TestBed.createComponent(LibInputMaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
