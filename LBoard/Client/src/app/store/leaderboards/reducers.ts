import * as LeaderboardActions from './actions'

export const initialState: object = {
  loading: false,
  all: [],
}

export function reducer(state = initialState, action: LeaderboardActions.Actions): object {
  switch (action.type) {
    case LeaderboardActions.LOAD_ALL:
      return {
        ...state,
        loading: true,
      }
    case LeaderboardActions.LOAD_ALL_SUCCESSFUL:
      return {
        ...state,
        all: action.payload,
        loading: false,
      }
    default:
      return state
  }
}

export const getLeaderboards = (state: any) => state
