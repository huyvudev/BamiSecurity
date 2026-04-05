import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibInputSwitchComponent } from './lib-input-switch.component';

describe('LibInputSwitchComponent', () => {
  let component: LibInputSwitchComponent;
  let fixture: ComponentFixture<LibInputSwitchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibInputSwitchComponent]
    });
    fixture = TestBed.createComponent(LibInputSwitchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
