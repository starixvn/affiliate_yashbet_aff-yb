import { combineReducers } from 'redux';

import createReducer from '../../../../modules/createReducer';
import { isShowLoader } from '../../../../reducers/loaderReducer';
import { SET_SELECTED_MARKET, SET_SELECTED_DOMAIN, SHOULD_RESET_SCROLL_POSITION, SET_GRID_DATA, SET_AFF_DOMAIN_LIST, SET_REDIRECT_DOMAIN } from '../actions/types';

const marketList = createReducer(null, {})

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

export const selectedMarket = createReducer('', {
    [SET_SELECTED_MARKET](prevState, action) {
        return action.data;
    }
})

export const selectedDomain = createReducer(null, {
    [SET_SELECTED_DOMAIN](prevState, action) {
        return action.data;
    }
})

export const affDomainList = createReducer(null, {
    [SET_AFF_DOMAIN_LIST](prevState, action) {
        return action.data;
    }
})

export const redirectDomainList = createReducer(null, {
    [SET_REDIRECT_DOMAIN](prevState, action) {
        return action.data;
    }
})

export default combineReducers({
    gridData,
    isShowLoader,
    marketList,
    selectedMarket,
    selectedDomain,
    affDomainList,
    redirectDomainList,
    shouldResetScrollPosition
});