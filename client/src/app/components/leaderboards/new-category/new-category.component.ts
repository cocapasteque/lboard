import { select, Store } from '@ngrx/store'
import { Component, OnInit } from '@angular/core'

import * as Reducers from 'src/app/store/reducers'
import * as LeaderboardsAction from 'src/app/store/leaderboards/actions'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'

@Component({
  selector: 'lboard-new-category-component',
  templateUrl: './new-category.component.html',
})
export class NewCategoryComponent implements OnInit {
  categoryForm: FormGroup

  get categoryName() {
    return this.categoryForm.controls.name
  }

  constructor(private store: Store<any>, private fb: FormBuilder) {

  }

  ngOnInit(): void {
    this.categoryForm = this.fb.group({
      name: [null, [Validators.required]],
    })
  }

  validate(): void {
    for (const i in this.categoryForm.controls) {
      if (this.categoryForm.controls.hasOwnProperty(i)) {
        this.categoryForm.controls[i].markAsDirty()
        this.categoryForm.controls[i].updateValueAndValidity()
      }
    }
    if (this.categoryForm.valid) {
      this.store.dispatch(new LeaderboardsAction.CreateCategory({
        name: this.categoryName.value,
      }))
    }
  }
}
