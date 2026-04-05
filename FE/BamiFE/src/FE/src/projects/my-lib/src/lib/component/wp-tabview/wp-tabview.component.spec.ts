import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WpTabviewComponent } from './wp-tabview.component';

describe('WpTabviewComponent', () => {
  let component: WpTabviewComponent;
  let fixture: ComponentFixture<WpTabviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WpTabviewComponent]
    });
    fixture = TestBed.createComponent(WpTabviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
