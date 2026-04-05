import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibSelectComponent } from './lib-select.component';

describe('LibSelectComponent', () => {
  let component: LibSelectComponent;
  let fixture: ComponentFixture<LibSelectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibSelectComponent]
    });
    fixture = TestBed.createComponent(LibSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
