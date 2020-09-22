import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { select, Store } from '@ngrx/store'
import * as Reducers from 'src/app/store/reducers'
import * as UserActions from 'src/app/store/user/actions'
import * as SettingsActions from 'src/app/store/settings/actions'

@Component({
  selector: 'cui-system-login',
  templateUrl: './login.component.html',
  styleUrls: ['../style.component.scss'],
})
export class LoginComponent {
  form: FormGroup
  logo: String
  authProvider: string = 'jwt'
  loading: boolean = false

  constructor(private fb: FormBuilder, private store: Store<any>) {
    this.form = fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
    })
    this.store.pipe(select(Reducers.getSettings)).subscribe(state => {
      this.logo = state.logo
      this.authProvider = state.authProvider
    })
    this.store.pipe(select(Reducers.getUser)).subscribe(state => {
      this.loading = state.loading
    })
  }

  get username() {
    return this.form.controls.username
  }
  get password() {
    return this.form.controls.password
  }

  submitForm(): void {
    this.username.markAsDirty()
    this.username.updateValueAndValidity()
    this.password.markAsDirty()
    this.password.updateValueAndValidity()
    if (this.username.invalid || this.password.invalid) {
      return
    }
    const payload = {
      username: this.username.value,
      password: this.password.value,
    }
    this.store.dispatch(new UserActions.Login(payload))
  }
}
