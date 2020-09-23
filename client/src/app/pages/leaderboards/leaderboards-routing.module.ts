import { LeaderboardsPage } from './index/leaderboards.component'
import { RouterModule, Routes } from '@angular/router'
import { NgModule } from '@angular/core'
import { LayoutsModule } from '../../layouts/layouts.module'
import { NewCategoryPage } from './new-category/new-category.component'

const routes: Routes = [
  {
    path: 'all',
    component: LeaderboardsPage,
    data: { title: 'Leaderboards' },
  },
  {
    path: 'new-category',
    component: NewCategoryPage,
    data: { title: 'New Category' },
  },
]

@NgModule({
  imports: [LayoutsModule, RouterModule.forChild(routes)],
  providers: [],
  exports: [RouterModule],
})
export class LeaderboardsRouterModule {
}
