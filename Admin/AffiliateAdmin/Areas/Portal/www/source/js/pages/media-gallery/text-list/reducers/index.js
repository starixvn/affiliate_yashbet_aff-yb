import { combineReducers } from 'redux';

import createReducer from '../../../../modules/createReducer';
import { pagingOption } from '../../../../reducers/pagingReducer';
import { isShowLoader } from '../../../../reducers/loaderReducer';
import { SET_PARAM, SET_CHECKED_TEXTS, SET_SELECTED_TEXT_ID, SHOULD_RESET_SCROLL_POSITION, SET_GRID_DATA, SET_DISABLE_CURRENCY, SET_CATEGORY_LIST } from '../actions/types';

export const categoryList = createReducer(null, {
    [SET_CATEGORY_LIST](prevState, action) {
        return action.data;
    }
});
export const languageList = createReducer(null, {});
export const currencyList = createReducer(null, {});
export const defaultLanguage = createReducer(null, {});

export const param = createReducer(null, {
    [SET_PARAM](prevState, action) {
        return Object.assign({}, prevState, action.data);
    }
})

export const selectedTextId = createReducer(null, {
    [SET_SELECTED_TEXT_ID](prevState, action) {
        return action.data;
    }
})

export const checkedTexts = createReducer({}, {
    [SET_CHECKED_TEXTS](prevState, action) {
        return action.data;
    }
})

export const shouldResetScrollPosition = createReducer(true, {
    [SHOULD_RESET_SCROLL_POSITION](prevState, action) {
        return action.data;
    }
})

export const gridData = createReducer(null, {
    [SET_GRID_DATA](prevState, action) {
        return action.data;
    }
})

export const isDisableCurrency = createReducer(false, {
    [SET_DISABLE_CURRENCY](prevState, action) {
        return action.data;
    }
})

export default combineReducers({
    categoryList,
    languageList,
    currencyList,
    defaultLanguage,
    param,
    gridData,
    pagingOption,
    isShowLoader,
    checkedTexts,
    selectedTextId,
    shouldResetScrollPosition,
    isDisableCurrency
});