import { LeaderboardsPage } from './index/leaderboards.component'
import { RouterModule, Routes } from '@angular/router'
import { NgModule } from '@angular/core'
import { LayoutsModule } from '../../layouts/layouts.module'

const routes: Routes = [
  {
    path: 'all',
    component: LeaderboardsPage,
    data: { title: 'Leaderboards' },
  },
]

@NgModule({
  imports: [LayoutsModule, RouterModule.forChild(routes)],
  providers: [],
  exports: [RouterModule],
})
export class LeaderboardsRouterModule {}
