import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { HttpClient } from '@angular/common/http'
import store from 'store'
import { environment } from '../../../environments/environment'

@Injectable()
export class jwtAuthService {
  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post(environment.apiUrl + 'auth/login', { username, password })
  }

  register(email: string, password: string, username: string): Observable<any> {
    return this.http.post(environment.apiUrl + 'auth/register', { email, password, username })
  }

  currentAccount(): Observable<any> {
    const accessToken = store.get('accessToken')
    const params = accessToken
      ? {
          headers: {
            Authorization: `Bearer ${accessToken}`,
            AccessToken: accessToken,
          },
        }
      : {}

    return this.http.get(environment.apiUrl + 'auth/account', params)
  }

  logout(): Observable<any> {
    return this.http.get(environment.apiUrl + 'auth/logout')
  }
}
