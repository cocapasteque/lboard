import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'
import { Injectable } from '@angular/core'
import store from 'store'
import { environment } from '../../../environments/environment'

@Injectable()
export class ApiService {
  constructor(private http: HttpClient) {
  }

  leaderboardsLoadAll(): Observable<any> {
    return this.http.get(environment.apiUrl + 'leaderboards', this.getHeader())
  }

  createCategory(category: any): Observable<any> {
    return this.http.post(environment.apiUrl + 'categories', category, this.getHeader())
  }

  getCategories(): Observable<any> {
    return this.http.get(environment.apiUrl + 'categories', this.getHeader())
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
