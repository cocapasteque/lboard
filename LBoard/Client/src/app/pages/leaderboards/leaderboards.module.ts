import { LeaderboardsPage } from './index/leaderboards.component'
import { SharedModule } from '../../shared.module'
import { FormsModule } from '@angular/forms'
import { NgModule } from '@angular/core'
import { LeaderboardsRouterModule } from './leaderboards-routing.module'
import { LeaderboardsComponentsModule } from '../../components/leaderboards/leaderboards-components.module'

const COMPONENTS = [LeaderboardsPage]

@NgModule({
  imports: [SharedModule, FormsModule, LeaderboardsRouterModule, LeaderboardsComponentsModule],
  declarations: [...COMPONENTS],
})
export class LeaderboardsModule {}
