import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { getMenuData } from './config'
import { map } from 'rxjs/operators'

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  constructor() {
  }

  getMenuData(categories: any[] = []): Observable<any[]> {
    const menu = getMenuData
    categories.forEach(x => {
      menu.push({
        title: x.name,
        key: x.name,
        icon: 'fe fe-grid',
        url: '/leaderboards/' + x.id,
      })
    })
    return of(getMenuData)
  }
}
