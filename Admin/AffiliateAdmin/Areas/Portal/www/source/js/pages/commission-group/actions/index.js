import { post } from '../../../modules/ajaxRequest';
import { toggleLoader } from '../../../actions/loaderAction';
import { setPagingOption } from '../../../actions/pagingAction';
import { getSelectedProductIds } from '../modules/service';

import { SET_GRIDDATA, SET_SELECTED_GROUP, SET_SEARCHED_GROUPNAME, SET_SELECTED_PRODUCT_IDS } from './types';

export { toggleLoader, setPagingOption };

export const fetchGroupSettingData = (param) => {
    const url = 'Portal/RevenueGroup/GetCommissionGroupList';

    return post(url, param);
}

export const setGridData = (data) => {
    return {
        type: SET_GRIDDATA,
        data
    }
}

export const getGroupSettingList = (param) => async (dispatch) => {
    dispatch(toggleLoader(true));

    if (!param.GroupName) {
        dispatch(setSearchedGroupName(''));
    }

    const response = await fetchGroupSettingData(param);
    if (response && response.status === 200) {
        const data = response.data;

        dispatch(setGridData(data ? data.CommissionSettingList : null));

        const { PageIndex, PageSize } = param;
        const pagingOption = {
            PageIndex,
            PageSize,
            TotalItem: data ? data.TotalRows : 0
        };
        dispatch(setPagingOption(pagingOption));
    }
    dispatch(toggleLoader(false));
}

export const setSelectedProductIds = (data) => {
    return {
        type: SET_SELECTED_PRODUCT_IDS,
        data
    }
}

export const setSelectedGroup = (data) => {
    return {
        type: SET_SELECTED_GROUP,
        data
    }
}

export const setSearchedGroupName = (data) => {
    return {
        type: SET_SEARCHED_GROUPNAME,
        data
    }
}

export const insertCommissionGroup = (param) => {
    const url = 'Portal/RevenueGroup/CreateCommissionGroup';

    return () => post(url, param);
}

export const updateCommissionGroup = (param) => {
    const url = 'Portal/RevenueGroup/UpdateCommissionGroup';

    return () => post(url, param);
}

export const getSelectedCommGroup = (param) => async (dispatch) => {
    const url = 'Portal/RevenueGroup/GetCommissionGroup';

    const response = await post(url, param);
    let data = response && response.status === 200 ? response.data : null;

    let selectedProductIds = data ? getSelectedProductIds(data.P2pDeductionList) : [];

    //if (data) {
    //    selectedProductIds = getSelectedProductIds(data.P2pDeductionList);
    //    const newPlatformFee = addKeyForAllPlatformFees(data.PlatformFeeList);
    //    data = Object.assign({}, data, { PlatformFeeList: newPlatformFee });
    //}

    dispatch(setSelectedProductIds(selectedProductIds));
    dispatch(setSelectedGroup(data));
}

//const addKeyForAllPlatformFees = (platformFee) => {
//    const now = Date.now();

//    return platformFee.map((item, index) => ({ ...item, Key: now - index }));
//}

export const deleteCommGroup = (param) => {
    const url = 'Portal/RevenueGroup/DeleteCommissionGroup';

    return () => post(url, param);
}