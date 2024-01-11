import { post } from '../../../../modules/ajaxRequest';
import { toggleLoader } from '../../../../actions/loaderAction';
import { SHOULD_RESET_SCROLL_POSITION, SET_SELECTED_DOMAIN, SET_GRID_DATA } from './types';

export { toggleLoader };

export const setShouldResetScrollPosition = (data) => {
    return {
        type: SHOULD_RESET_SCROLL_POSITION,
        data
    }
}

export const setGridData = (data) => {
    return {
        type: SET_GRID_DATA,
        data
    }
}

export const setSelectedDomain = (data) => {
    return {
        type: SET_SELECTED_DOMAIN,
        data
    }
}

export const getDomainList = async (isAffDomain) => {
    const url = 'Portal/Domain/GetDomainList';
    const response = await post(url, { isAffDomain });

    return response && response.status === 200 ? response.data : [];
}

export const fetchData = (isAffDomain) => async (dispatch) => {
    dispatch(toggleLoader(true));

    const data = await getDomainList(isAffDomain);

    dispatch(setGridData(data));
    dispatch(setShouldResetScrollPosition(true));
    dispatch(toggleLoader(false));
}

export const insertDomain = (param) => async () => {
    const url = 'Portal/Domain/InsertDomain';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}

export const updateDomain = (param) => async () => {
    const url = 'Portal/Domain/UpdateDomain';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}

export const deleteDomain = (param) => async () => {
    const url = 'Portal/Domain/DeleteDomain';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}