import { post } from '../../../../modules/ajaxRequest';
import { toggleLoader } from '../../../../actions/loaderAction';
import { SET_SELECTED_MARKET, SET_SELECTED_DOMAIN, SHOULD_RESET_SCROLL_POSITION, SET_GRID_DATA, SET_AFF_DOMAIN_LIST, SET_REDIRECT_DOMAIN } from './types';
import { getDomainList } from '../../domain/actions/index';

export { toggleLoader };

export const setSelectedDomain = (data) => {
    return {
        type: SET_SELECTED_DOMAIN,
        data
    }
}

export const setSelectedMarket = (data) => {
    return {
        type: SET_SELECTED_MARKET,
        data
    }
}

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

export const setAffDomainList = (data) => {
    return {
        type: SET_AFF_DOMAIN_LIST,
        data
    }
}

export const setRedirectDomainList = (data) => {
    return {
        type: SET_REDIRECT_DOMAIN,
        data
    }
}

export const fetchData = (market) => async (dispatch) => {
    dispatch(toggleLoader(true));

    const url = 'Portal/DomainControl/GetDomainControlList';
    const response = await post(url, { market });

    const data = response && response.status === 200 ? response.data : [];

    dispatch(setGridData(data));
    dispatch(setShouldResetScrollPosition(true));
    dispatch(toggleLoader(false));
}

export const fetchDomain = () => async (dispatch) => {
    dispatch(setAffDomainList(await getDomainList(true)));
    dispatch(setRedirectDomainList(await getDomainList(false)));
}

export const insertDomain = (param) => async () => {
    const url = 'Portal/DomainControl/InsertDomainControl';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}

export const updateDomain = (param) => async () => {
    const url = 'Portal/DomainControl/UpdateDomainControl';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}

export const deleteDomain = (param) => async () => {
    const url = 'Portal/DomainControl/DeleteDomainControl';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}
