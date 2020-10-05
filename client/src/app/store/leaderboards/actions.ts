import { Action } from '@ngrx/store'

export const LOAD_ALL = '[Leaderboards] Load All'
export const LOAD_ALL_SUCCESSFUL = '[Leaderboards] Load All Successful'
export const LOAD_ALL_UNSUCCESSFUL = '[Leaderboards] Load All Unsuccessful'
export const CREATE_CATEGORY = '[Category] Create New Category'
export const CREATE_CATEGORY_SUCCESSFUL = '[Category] Created Category'
export const CREATE_CATEGORY_UNSUCCESSFUL = '[Category] Created Category Fail'
export const LOAD_ALL_CATEGORIES = '[Category] Load All'
export const LOAD_ALL_CATEGORIES_SUCCESSFUL = '[Category] Load All Successful'
export const LOAD_ALL_CATEGORIES_UNSUCCESSFUL = '[Category] Load All Unsuccessful'

export class LoadAll implements Action {
  readonly type = LOAD_ALL

  constructor() {
  }
}

export class LoadAllSuccessful implements Action {
  readonly type = LOAD_ALL_SUCCESSFUL

  constructor(public payload: any) {
  }
}

export class LoadAllUnsuccessful implements Action {
  readonly type = LOAD_ALL_UNSUCCESSFUL

  constructor() {
  }
}

export class CreateCategory implements Action {
  readonly type = CREATE_CATEGORY

  constructor(public payload: any) {
  }
}

export class CreateCategorySuccessful implements Action {
  readonly type = CREATE_CATEGORY_SUCCESSFUL

  constructor() {
  }
}

export class CreateCategoryUnsuccessful implements Action {
  readonly type = CREATE_CATEGORY_UNSUCCESSFUL

  constructor() {
  }
}

export class LoadAllCategories implements Action {
  readonly type = LOAD_ALL_CATEGORIES

  constructor() {
  }
}

export class LoadAllCategoriesSuccessful implements Action {
  readonly type = LOAD_ALL_CATEGORIES_SUCCESSFUL

  constructor(public payload: any) {
  }
}

export class LoadAllCategoriesUnsuccessful implements Action {
  readonly type = LOAD_ALL_CATEGORIES_UNSUCCESSFUL

  constructor() {
  }
}


export type Actions =
  LoadAll
  | LoadAllSuccessful
  | LoadAllUnsuccessful
  | CreateCategory
  | CreateCategorySuccessful
  | CreateCategoryUnsuccessful
  | LoadAllCategories
  | LoadAllCategoriesSuccessful
  | LoadAllCategoriesUnsuccessful
