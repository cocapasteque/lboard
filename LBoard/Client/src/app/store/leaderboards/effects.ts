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

@Injectable()
export class LeaderboardEffects {
  constructor(
    private actions: Actions,
    private router: Router,
    private rxStore: Store<any>,
    private api: ApiService,
    private notification: NzNotificationService,
  ) {}

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
}
