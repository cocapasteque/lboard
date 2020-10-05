import * as LeaderboardActions from './actions'

export const initialState: object = {
  loading: false,
  leaderboards: [],
  categories: []
}

export function reducer(state = initialState, action: LeaderboardActions.Actions): object {
  switch (action.type) {
    case LeaderboardActions.LOAD_ALL:
    case LeaderboardActions.CREATE_CATEGORY:
    case LeaderboardActions.LOAD_ALL_CATEGORIES:
      return {
        ...state,
        loading: true,
      }
    case LeaderboardActions.LOAD_ALL_SUCCESSFUL:
      return {
        ...state,
        leaderboards: action.payload,
        loading: false,
      }
    case LeaderboardActions.LOAD_ALL_CATEGORIES_SUCCESSFUL:
      return {
        ...state,
        categories: action.payload,
        loading: false,
      }

    case LeaderboardActions.LOAD_ALL_CATEGORIES_UNSUCCESSFUL:
    case LeaderboardActions.CREATE_CATEGORY_SUCCESSFUL:
    case LeaderboardActions.CREATE_CATEGORY_UNSUCCESSFUL:
      return {
        ...state,
        loading: false,
      }
    default:
      return state
  }
}

export const getLeaderboards = (state: any) => state
export const getCategories = (state: any) => state
