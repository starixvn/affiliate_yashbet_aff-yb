import { post } from '../../../../modules/ajaxRequest';
import { toggleLoader } from '../../../../actions/loaderAction';
import { setPagingOption } from '../../../../actions/pagingAction';
import { SET_PARAM, SET_CHECKED_TEXTS, SET_SELECTED_TEXT_ID, SHOULD_RESET_SCROLL_POSITION, SET_GRID_DATA, SET_DISABLE_CURRENCY, SET_CATEGORY_LIST } from './types';

export { toggleLoader, setPagingOption };

export const setParam = (data) => {
    return {
        type: SET_PARAM,
        data
    }
}

export const setSelectedTextId = (data) => {
    return {
        type: SET_SELECTED_TEXT_ID,
        data
    }
}

export const setCheckedTexts = (data) => {
    return {
        type: SET_CHECKED_TEXTS,
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

export const setDisableCurrency = (data) => {
    return {
        type: SET_DISABLE_CURRENCY,
        data
    }
}

export const setCategoryList = (data) => {
    return {
        type: SET_CATEGORY_LIST,
        data
    }
}

export const getCategoryList = (userType) => async (dispatch) => {
    const url = 'Portal/TextList/GetCategoryList';
    const response = await post(url, { userType });

    const data = response && response.status === 200 ? response.data : [];

    dispatch(setCategoryList(data));
}


export const fetchData = (param) => async (dispatch) => {
    dispatch(toggleLoader(true));

    const url = 'Portal/TextList/GetTextList';
    const response = await post(url, param);

    const data = response && response.status === 200 ? response.data : null;
    const checkedTexts = data ? _convertCheckedTexts(data.MainData) : {};

    dispatch(setGridData(data ? data.MainData : []));
    dispatch(setPagingOption({ TotalItem: data ? data.TotalItem : 0, PageIndex: param.PageIndex, PageSize: param.PageSize }));
    dispatch(setShouldResetScrollPosition(true));
    dispatch(setCheckedTexts(checkedTexts));
    dispatch(toggleLoader(false));
}

function _convertCheckedTexts(dataList) {
    const checkedTexts = {};
    for (let item of dataList) {
        const itemId = item.TextId;
        checkedTexts[itemId] = false;
    }

    return checkedTexts;
}

export const getTextDetail = (selectedTextId, userType) => async () => {
    const url = 'Portal/TextList/GetTextDetail';
    const response = await post(url, { selectedTextId, userType });

    return response && response.status === 200 ? response.data : null;
}

export const insertText = (param) => async () => {
    const url = 'Portal/TextList/insertText';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}

export const updateText = (param) => async () => {
    const url = 'Portal/TextList/UpdateText';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}

export const deleteTexts = (param) => async () => {
    const url = 'Portal/TextList/DeleteTexts';
    const response = await post(url, param);

    return response && response.status === 200 ? response.data : null;
}