import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupButtonEditComponent } from './group-button-submit.component';

describe('GroupButtonEditComponent', () => {
  let component: GroupButtonEditComponent;
  let fixture: ComponentFixture<GroupButtonEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupButtonEditComponent]
    });
    fixture = TestBed.createComponent(GroupButtonEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
