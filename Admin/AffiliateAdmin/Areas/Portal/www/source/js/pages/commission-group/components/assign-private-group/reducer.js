import { combineReducers } from "redux";
import { selectedGroup, productList, p2pProductList, deductionList, defaultCurrency, selectedProductIds } from '../../reducers';
import createReducer from '../../../../modules/createReducer';

export const affInfo = createReducer({}, {});

export default combineReducers({
    productList,
    p2pProductList,
    deductionList,
    selectedGroup,
    affInfo,
    defaultCurrency,
    selectedProductIds
});