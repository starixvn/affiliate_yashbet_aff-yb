import { combineReducers } from 'redux';
import { SET_SELECTED_PAGE, SET_PARAM, SET_SHORT_CODE_LANGUAGE_LIST } from '../actions/types';
import createReducer from '../../../../modules/createReducer';

import { pagingOption } from '../../../../reducers/pagingReducer';

export const pageList = createReducer([], {});

export const selectedPage = createReducer(null, {
    [SET_SELECTED_PAGE](prevState, action) {
        return action.data ? Object.assign({}, action.data) : null;
    }
});

export const param = createReducer({}, {
    [SET_PARAM](prevState, action) {
        return Object.assign({}, prevState, action.data);
    }
});

export const shortCodeLanguageList = createReducer({}, {
    [SET_SHORT_CODE_LANGUAGE_LIST](prevState, action) {
        return { ...action.data };
    }
});

export default combineReducers({
    pagingOption,
    shortCodeLanguageList,
    pageList,
    selectedPage,
    param
});