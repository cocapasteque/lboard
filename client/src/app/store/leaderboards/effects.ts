import { Injectable } from '@angular/core'
import { Actions, Effect, ofType, OnInitEffects } from '@ngrx/effects'
import { Router } from '@angular/router'
import { NzNotificationService } from 'ng-zorro-antd'
import { select, Store } from '@ngrx/store'

import * as LeaderboardActions from './actions'
import * as Reducers from 'src/app/store/reducers'
import { from, Observable, of } from 'rxjs'
import { catchError, concatMap, map, switchMap, withLatestFrom } from 'rxjs/operators'
import { ApiService } from '../../services/api'
import { LoadAllCategories } from './actions'

@Injectable()
export class LeaderboardEffects {
  constructor(
    private actions: Actions,
    private router: Router,
    private rxStore: Store<any>,
    private api: ApiService,
    private notification: NzNotificationService,
  ) {
  }

  @Effect()
  loadAll: Observable<any> = this.actions.pipe(
    ofType(LeaderboardActions.LOAD_ALL),
    map((action: LeaderboardActions.LoadAll) => true),
    concatMap(action =>
      of(action).pipe(withLatestFrom(this.rxStore.pipe(select(Reducers.getSettings)))),
    ),
    switchMap(() => {
      return this.api.leaderboardsLoadAll().pipe(
        map(response => {
          if (response) {
            return new LeaderboardActions.LoadAllSuccessful(response)
          }
          return new LeaderboardActions.LoadAllUnsuccessful()
        }),
      )
    }),
    catchError(error => {
      this.notification.error('Cannot get leaderboards', error.message)
      return from([{ type: LeaderboardActions.LOAD_ALL_UNSUCCESSFUL }])
    }),
  )

  @Effect()
  loadAllCategories: Observable<any> = this.actions.pipe(
    ofType(LeaderboardActions.LOAD_ALL_CATEGORIES),
    map((action: LeaderboardActions.LoadAllCategories) => true),
    concatMap(action =>
      of(action).pipe(withLatestFrom(this.rxStore.pipe(select(Reducers.getSettings)))),
    ),
    switchMap(() => {
      return this.api.getCategories().pipe(
        map(response => {
          if (response) {
            return new LeaderboardActions.LoadAllCategoriesSuccessful(response)
          }
          return new LeaderboardActions.LoadAllCategoriesUnsuccessful()
        }),
      )
    }),
    catchError(error => {
      this.notification.error('Cannot get categories', error.message)
      return from([{ type: LeaderboardActions.LOAD_ALL_CATEGORIES_UNSUCCESSFUL }])
    }),
  )

  @Effect()
  createCategory: Observable<any> = this.actions.pipe(
    ofType(LeaderboardActions.CREATE_CATEGORY),
    map((action: LeaderboardActions.CreateCategory) => action.payload),
    concatMap(action =>
      of(action).pipe(withLatestFrom(this.rxStore.pipe(select(Reducers.getSettings)))),
    ),
    switchMap(([payload, settings]) => {
      return this.api.createCategory(payload).pipe(
        map(response => {
          console.log(response)
          if (response && response.id) {
            this.notification.success('Success', 'Category ' + response.name + ' Created')
            return new LeaderboardActions.CreateCategorySuccessful()
          }
          this.notification.error('CATEGORY ERROR', response)
          return new LeaderboardActions.CreateCategoryUnsuccessful()
        }),
      )
    }),
  )
}
