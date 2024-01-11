import { combineReducers } from 'redux';

import createReducer from '../../../../modules/createReducer';
import { isShowLoader } from '../../../../reducers/loaderReducer';
import { SET_CHECKED_LANDING_PAGES, SET_SELECTED_LANDING_PAGE_ID, SHOULD_RESET_SCROLL_POSITION, SET_GRID_DATA } from '../actions/types';

export const selectedLandingPageId = createReducer(null, {
    [SET_SELECTED_LANDING_PAGE_ID](prevState, action) {
        return action.data;
    }
})

export const checkedLandingPages = createReducer({}, {
    [SET_CHECKED_LANDING_PAGES](prevState, action) {
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
    gridData,
    isShowLoader,
    checkedLandingPages,
    selectedLandingPageId,
    shouldResetScrollPosition
});