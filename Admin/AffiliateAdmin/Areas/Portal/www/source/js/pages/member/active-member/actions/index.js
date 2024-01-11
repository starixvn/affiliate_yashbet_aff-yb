import { post } from '../../../../modules/ajaxRequest';
import { toggleLoader } from '../../../../actions/loaderAction';
import { SET_USER_NAMES, SET_SHOW_TEXT, SET_MESSAGE, SET_MEMBER_NOT_EXIST, SHOULD_RESET_SCROLL_POSITION, SET_ACTIVATED_GRID_DATA, SET_UNACTIVATED_GRID_DATA } from './types';

export const setUserNames = (data) => {
    return {
        type: SET_USER_NAMES,
        data
    }
}

export const setShowText = (data) => {
    return {
        type: SET_SHOW_TEXT,
        data
    }
}

export const setMessage = (data) => {
    return {
        type: SET_MESSAGE,
        data
    }
}

export const setMemberNotExist = (data) => {
    return {
        type: SET_MEMBER_NOT_EXIST,
        data
    }
}

export const setShouldResetScrollPosition = (data) => {
    return {
        type: SHOULD_RESET_SCROLL_POSITION,
        data
    }
}

export const setActivatedGridData = (data) => {
    return {
        type: SET_ACTIVATED_GRID_DATA,
        data
    }
}

export const setUnactivatedGridData = (data) => {
    return {
        type: SET_UNACTIVATED_GRID_DATA,
        data
    }
}

export const fetchData = (param) => async (dispatch) => {
    dispatch(toggleLoader(true));

    const url = 'Portal/ActiveMember/GetAllMembers';
    const response = await post(url, param);
    const data = response && response.status === 200 ? response.data : null;

    dispatch(setUnactivatedGridData(data ? data.InactiveMemberList : []));
    dispatch(setActivatedGridData(data ? data.ActivedMemeberList : []));
    dispatch(setMemberNotExist(data ? data.MemberNotExist : ''));
    dispatch(setShouldResetScrollPosition(true));
    dispatch(toggleLoader(false));
}

export const activeMember = (param) => async () => {
    // const url = 'Portal/ActiveMember/ActiveMember';
    // const response = await post(url, param);

    // return response && response.status === 200 ? response.data : null;
}