import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibInputTextComponent } from './lib-input-text.component';

describe('LibInputTextComponent', () => {
  let component: LibInputTextComponent;
  let fixture: ComponentFixture<LibInputTextComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibInputTextComponent]
    });
    fixture = TestBed.createComponent(LibInputTextComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
