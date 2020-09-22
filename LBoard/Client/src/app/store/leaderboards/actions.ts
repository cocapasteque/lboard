import { Action } from '@ngrx/store'

export const LOAD_ALL = '[Leaderboards] Load All'
export const LOAD_ALL_SUCCESSFUL = '[Leaderboards] Load All Successful'
export const LOAD_ALL_UNSUCCESSFUL = '[Leaderboards] Load All Unsuccessful'

export class LoadAll implements Action {
  readonly type = LOAD_ALL
  constructor() {}
}

export class LoadAllSuccessful implements Action {
  readonly type = LOAD_ALL_SUCCESSFUL
  constructor(public payload: any) {}
}

export class LoadAllUnsuccessful implements Action {
  readonly type = LOAD_ALL_UNSUCCESSFUL
  constructor() {}
}

export type Actions = LoadAll | LoadAllSuccessful | LoadAllUnsuccessful
