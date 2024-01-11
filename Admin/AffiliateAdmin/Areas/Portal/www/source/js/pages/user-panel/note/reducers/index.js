import { combineReducers } from 'redux';
import { SET_NOTE_LIST, SET_SHOW_POPUP, SET_NOTE_INFO } from '../actions/types';
import createReducer from '../../../../modules/createReducer';

import { pagingOption } from '../../../../reducers/pagingReducer';

export const noteList = createReducer([], {
    [SET_NOTE_LIST](prevState, action) {
        return [...action.data];
    }
});

export const isShowPopup = createReducer(false, {
    [SET_SHOW_POPUP](prevState, action) {
        return action.data;
    }
});

const defaultNote = { noteId: 0, noteContent: '' };
export const noteInfo = createReducer({ ...defaultNote }, {
    [SET_NOTE_INFO](prevState, action) {
        return { ...action.data };
    }
});

export default combineReducers({
    pagingOption,
    isShowPopup,
    noteList,
    noteInfo
});