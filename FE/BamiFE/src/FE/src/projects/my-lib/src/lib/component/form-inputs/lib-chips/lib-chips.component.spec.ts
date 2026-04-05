import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibChipsComponent } from './lib-chips.component';

describe('LibChipsComponent', () => {
  let component: LibChipsComponent;
  let fixture: ComponentFixture<LibChipsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibChipsComponent]
    });
    fixture = TestBed.createComponent(LibChipsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
