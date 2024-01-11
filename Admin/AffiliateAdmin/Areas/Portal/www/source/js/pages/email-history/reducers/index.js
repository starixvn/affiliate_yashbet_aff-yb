import { combineReducers } from 'redux';

import createReducer from '../../../modules/createReducer';
import { pagingOption } from '../../../reducers/pagingReducer';
import { isShowLoader } from '../../../reducers/loaderReducer';
import { SET_PARAM, SET_CHECKED_EMAILS, SET_SELECTED_EMAIL_ID, SHOULD_RESET_SCROLL_POSITION, SET_GRID_DATA } from '../actions/types';

export const today = createReducer(null, {});

export const param = createReducer(null, {
    [SET_PARAM](prevState, action) {
        return Object.assign({}, prevState, action.data);
    }
})

export const selectedEmailId = createReducer(null, {
    [SET_SELECTED_EMAIL_ID](prevState, action) {
        return action.data;
    }
})

export const checkedEmails = createReducer({}, {
    [SET_CHECKED_EMAILS](prevState, action) {
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

export default combineReducers({
    pagingOption,
    isShowLoader,
    selectedEmailId,
    checkedEmails,
    shouldResetScrollPosition,
    gridData,
    today,
    param
});