import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibInputNumberComponent } from './lib-input-number.component';

describe('LibInputNumberComponent', () => {
  let component: LibInputNumberComponent;
  let fixture: ComponentFixture<LibInputNumberComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibInputNumberComponent]
    });
    fixture = TestBed.createComponent(LibInputNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
