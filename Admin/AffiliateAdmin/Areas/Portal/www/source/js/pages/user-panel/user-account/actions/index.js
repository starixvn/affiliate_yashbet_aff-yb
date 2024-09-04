import { post } from '../../../../modules/ajaxRequest';
import { toggleLoaderUserPanel } from '../../../customer/user-panel/actions';
import { getAffiliateInfo } from '../../../customer/mainpage/actions/loadingDataAction';
import { SET_USER_ACCOUNT_INFO, SET_MORE_SITE_URLS_LIST } from './types';

export { getAffiliateInfo }

export const setUserAccountInfo = (data) => {
    return {
        type: SET_USER_ACCOUNT_INFO,
        data
    }
}

export const setMoreSiteUrlsList = (data) => {
    return {
        type: SET_MORE_SITE_URLS_LIST,
        data
    }
}

export const getUserAccountInfo = (affId) => async (dispatch) => {
    dispatch(toggleLoaderUserPanel(true));
    const url = 'Portal/UserPanel/UserAccount/GetUserAccountInfo';

    const response = await post(url, { affId });

    const data = response && response.status === 200 ? response.data : null;

    const moreSiteUrls = data && data.MoreSiteUrls;
    const moreSiteUrlsList = moreSiteUrls && moreSiteUrls != '' && moreSiteUrls.split(';');

    dispatch(setMoreSiteUrlsList(moreSiteUrlsList));
    dispatch(setUserAccountInfo(data));
    dispatch(toggleLoaderUserPanel(false));
}

export const updateUserAccount = (param) => async () => {
    const url = 'Portal/UserPanel/UserAccount/updateUserAccount';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}

export const getPaymentMethod = () => async () => {
    const url = 'Portal/UserPanel/UserAccount/GetPaymentMethod';
    const response = await post(url);

    return response && response.status === 200 ? response.data : null;
}