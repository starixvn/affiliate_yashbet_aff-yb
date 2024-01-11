import { LEFTPANEL_RECEIVE_AFFINFO, SET_AFF_COMMON_INFO } from './types';
import { post } from '../../../../modules/ajaxRequest';

import { setSelectedUserPanel } from '../../user-panel/actions';
import { userPanelMenuKey } from '../../common/modules/constants';
import { setIsValidUserName, toggleLoaderLeftPanel } from '../../left-panel/actions';

export const receiveLeftMenuInfo = (data) => {
    return {
        type: LEFTPANEL_RECEIVE_AFFINFO,
        data
    }
}

export const setAffCommonInfo = (data) => {
    return {
        type: SET_AFF_COMMON_INFO,
        data
    }
}

export const getAffiliateInfo = (affName, selectedUserPanel = userPanelMenuKey.statementHistory) => async (dispatch) => {
    dispatch(toggleLoaderLeftPanel(true));

    const url = 'Portal/CustomerLeftPanel/GetAffiliateInfo';
    const param = { affUsername: affName };
    const response = await post(url, param);
    const data = response && response.status === 200 && response.data ? response.data : null;

    const affCommonInfo = data && { affId: data.AffId, affName, status: data.Status, isDepositFeePercent: data.IsDepositFeePercent, isWithdrawalFeePercent: data.IsWithdrawalFeePercent };
    dispatch(setAffCommonInfo(affCommonInfo));
    dispatch(receiveLeftMenuInfo(data));
    data && dispatch(setSelectedUserPanel(selectedUserPanel));
    dispatch(setIsValidUserName(!!data));

    dispatch(toggleLoaderLeftPanel(false));
}