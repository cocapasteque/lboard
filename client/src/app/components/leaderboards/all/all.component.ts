import { select, Store } from '@ngrx/store'
import { Component, OnInit } from '@angular/core'

import * as Reducers from 'src/app/store/reducers'
import * as LeaderboardsAction from 'src/app/store/leaderboards/actions'

@Component({
  selector: 'lboard-all-leaderboards',
  templateUrl: './all.component.html',
})
export class AllComponent implements OnInit {
  loading = true
  leaderboards = []

  constructor(private store: Store<any>) {
    this.store.pipe(select(Reducers.getAllLeaderboards)).subscribe(state => {
      this.leaderboards = state.leaderboards
      this.loading = state.loading
    })
  }

  ngOnInit(): void {
    this.store.dispatch(new LeaderboardsAction.LoadAll())
  }
}
