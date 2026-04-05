import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibRatingComponent } from './lib-rating.component';

describe('LibRatingComponent', () => {
  let component: LibRatingComponent;
  let fixture: ComponentFixture<LibRatingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LibRatingComponent]
    });
    fixture = TestBed.createComponent(LibRatingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
