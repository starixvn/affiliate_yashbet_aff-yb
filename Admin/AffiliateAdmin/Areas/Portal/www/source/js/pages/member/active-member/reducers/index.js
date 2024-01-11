import { combineReducers } from 'redux';

import createReducer from '../../../../modules/createReducer';
import { isShowLoader } from '../../../../reducers/loaderReducer';
import { SET_USER_NAMES, SET_SHOW_TEXT, SET_MESSAGE, SET_MEMBER_NOT_EXIST, SHOULD_RESET_SCROLL_POSITION, SET_ACTIVATED_GRID_DATA, SET_UNACTIVATED_GRID_DATA } from '../actions/types';

export const userNames = createReducer('', {
    [SET_USER_NAMES](prevState, action) {
        return action.data;
    }
})

export const isShowText = createReducer(false, {
    [SET_SHOW_TEXT](prevState, action) {
        return action.data;
    }
})

export const message = createReducer('', {
    [SET_MESSAGE](prevState, action) {
        return action.data;
    }
})

export const memberNotExist = createReducer('', {
    [SET_MEMBER_NOT_EXIST](prevState, action) {
        return action.data;
    }
})

export const shouldResetScrollPosition = createReducer(true, {
    [SHOULD_RESET_SCROLL_POSITION](prevState, action) {
        return action.data;
    }
})

export const activatedGridData = createReducer('', {
    [SET_ACTIVATED_GRID_DATA](prevState, action) {
        return action.data;
    }
})

export const unactivatedGridData = createReducer('', {
    [SET_UNACTIVATED_GRID_DATA](prevState, action) {
        return action.data;
    }
})

export default combineReducers({
    isShowLoader,
    isShowText,
    userNames,
    message,
    memberNotExist,
    activatedGridData,
    unactivatedGridData,
    shouldResetScrollPosition
});