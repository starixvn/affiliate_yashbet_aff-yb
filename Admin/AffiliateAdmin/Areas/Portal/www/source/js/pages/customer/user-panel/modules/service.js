import { post } from '../../../../modules/ajaxRequest';

export const isActiveMenu = (selectedMenu, menuItem) => {
    return selectedMenu && (selectedMenu.key === menuItem.key || selectedMenu.parent === menuItem.key);
}

export const getPageList = async () => {
    const response = await post('Portal/CommonSetting/GetPageList');
    return response && response.status === 200 ? response.data : [];
}

export const getTrackingCodeList = async (affId) => {
    const response = await post('Portal/CommonSetting/GetTrackingCodeList', { affId });
    return response && response.status === 200 ? response.data : [];
}
