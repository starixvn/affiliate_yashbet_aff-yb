import { combineReducers } from "redux";

import { SET_GRIDDATA, SET_SELECTED_GROUP, SET_SEARCHED_GROUPNAME, SET_SELECTED_PRODUCT_IDS } from '../actions/types';
import createReducer from '../../../modules/createReducer';
import { isShowLoader } from '../../../reducers/loaderReducer';
import { pagingOption } from '../../../reducers/pagingReducer';

export const gridData = createReducer(null, {
    [SET_GRIDDATA](prevState, action) {
        return action.data;
    }
})

export const searchedGroupName = createReducer('', {
    [SET_SEARCHED_GROUPNAME](prevState, action) {
        return action.data;
    }
})

export const selectedGroup = createReducer(null, {
    [SET_SELECTED_GROUP](prevState, action) {
        return !action.data ? null : Object.assign({}, prevState, action.data);
    }
})

export const productList = createReducer([], {});

export const p2pProductList = createReducer([], {});

export const selectedProductIds = createReducer([], {
    [SET_SELECTED_PRODUCT_IDS](prevState, action){
        return action.data;
    }
});
export const deductionList = createReducer([], {});

export const defaultCurrency = createReducer(null, {});

export default combineReducers({
    gridData,
    pagingOption,
    productList,
    p2pProductList,
    deductionList,
    selectedGroup,
    selectedProductIds,
    searchedGroupName,
    isShowLoader,
    defaultCurrency
});