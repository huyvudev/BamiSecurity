import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibCascadeSelectComponent } from './lib-cascade-select.component';

describe('LibCascadeSelectComponent', () => {
  let component: LibCascadeSelectComponent;
  let fixture: ComponentFixture<LibCascadeSelectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibCascadeSelectComponent]
    });
    fixture = TestBed.createComponent(LibCascadeSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
