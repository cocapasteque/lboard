import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'
import { Injectable } from '@angular/core'
import store from 'store'

@Injectable()
export class ApiService {
  constructor(private http: HttpClient) {}

  leaderboardsLoadAll(): Observable<any> {
    return this.http.get('/api/leaderboards', this.getHeader())
  }

  getHeader() {
    const accessToken = store.get('accessToken')
    return accessToken
      ? {
          headers: {
            Authorization: `Bearer ${accessToken}`,
            AccessToken: accessToken,
          },
        }
      : {}
  }
}
