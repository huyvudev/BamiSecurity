import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibInputGroupComponent } from './lib-input-group.component';

describe('LibInputGroupComponent', () => {
  let component: LibInputGroupComponent;
  let fixture: ComponentFixture<LibInputGroupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibInputGroupComponent]
    });
    fixture = TestBed.createComponent(LibInputGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
