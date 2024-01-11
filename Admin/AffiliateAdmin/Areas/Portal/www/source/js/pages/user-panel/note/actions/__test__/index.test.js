import * as actions from '../index';
import * as types from '../types';
import * as ajaxRequest from '../../../../../modules/ajaxRequest';

import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

const middlewares = [thunk]
const mockStore = configureMockStore(middlewares)

const runTestAction = ({ name, type, data }) => {
    it(`should create action to ${name}`, () => {
        const expectedAction = { type, data };

        expect(actions[name](data)).toEqual(expectedAction);
    });
}

const actionList = [
    { name: 'setNoteList', type: types.SET_NOTE_LIST, data: [{ NoteId: 123456 }] },
    { name: 'setShowPopup', type: types.SET_SHOW_POPUP, data: true },
    { name: 'setNoteInfo', type: types.SET_NOTE_INFO, data: { NoteId: 123456 } }
];

describe('note actions', () => {
    actionList.forEach(item => {
        runTestAction(item);
    });

    it('should create action to getNoteList with correct parameter', () => {
        const expectedResponse = {
            data: {
                MainData: [
                    {
                        CreatedBy: "AAHubert",
                        LastModifyText: "06/20/2019 22:21:05",
                        NoteContent: "Test Test",
                        NoteId: 615
                    },
                    {
                        CreatedBy: "AAwalter",
                        LastModifyText: "06/19/2019 00:02:05",
                        NoteContent: "Testing5555 update",
                        NoteId: 982
                    }
                ],
                TotalItem: 2
            },
            status: 200
        };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const expectedAction = [
            { type: 'TOGGLE_LOADER_USER_PANEL', data: true },
            { type: types.SET_NOTE_LIST, data: expectedResponse.data.MainData },
            { type: 'SET_PAGING_OPTION', data: { PageIndex: 1, PageSize: 20, TotalItem: 2 } },
            { type: 'TOGGLE_LOADER_USER_PANEL', data: false },
        ];
        const param = {
            affId: 1271808,
            pageIndex: 1,
            pageSize: 20
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getNoteList'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/UserPanel/Note/GetNoteList', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })

    it('should create action to getNoteList with incorrect parameter', () => {
        ajaxRequest.post = jest.fn();

        const expectedAction = [
            { type: 'TOGGLE_LOADER_USER_PANEL', data: true },
            { type: 'TOGGLE_LOADER_USER_PANEL', data: false },
        ];
        const param = {
            affId: 123456,
            pageIndex: 1,
            pageSize: 20
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getNoteList'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/UserPanel/Note/GetNoteList', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })

    it('should create action to getNoteContentById with correct parameter', async () => {
        const expectedResponse = {
            data: {
                CreatedBy: null,
                LastModifyText: "N/A",
                NoteContent: "B2CAFF226",
                NoteId: 1001
            }, status: 200
        };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));
        const param = {
            affId: 1271808,
            noteId: 1001
        };

        const result = await actions['getNoteContentById'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/UserPanel/Note/GetNoteContentById', param);

        expect(result).toEqual(expectedResponse.data);
    })

    it('should create action to getNoteContentById with incorrect parameter', async () => {
        ajaxRequest.post = jest.fn();
        const param = {
            affId: 123456,
            noteId: 123456
        };

        const result = await actions['getNoteContentById'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/UserPanel/Note/GetNoteContentById', param);
        expect(result).toEqual(null);
    })

    it('should create action to insertNote with correct parameter', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: '' }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));
        const param = {
            affId: 1271808,
            noteContent: "Insert note.",
            noteId: 0
        }

        const result = await actions['insertNote'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/UserPanel/Note/InsertNote', param);

        expect(result).toEqual({ IsSuccessful: true, Message: "" });
    })

    it('should create action to insertNote with incorrect parameter', async () => {
        ajaxRequest.post = jest.fn();
        const param = {
            affId: 123456,
            noteContent: "",
            noteId: 0
        }

        const result = await actions['insertNote'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/UserPanel/Note/InsertNote', param);
        expect(result).toEqual(null);
    })

    it('should create action to updateNote with correct parameter', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: '' }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));
        const param = {
            affId: 1271808,
            noteContent: "B2CAFF226",
            noteId: 1001
        }

        const result = await actions['updateNote'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/UserPanel/Note/UpdateNote', param);

        expect(result).toEqual({ IsSuccessful: true, Message: "" });
    })

    it('should create action to updateNote with incorrect parameter', async () => {
        ajaxRequest.post = jest.fn();
        const param = {
            affId: 123456,
            noteContent: "",
            noteId: 123456
        }

        const result = await actions['updateNote'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/UserPanel/Note/UpdateNote', param);
        expect(result).toEqual(null);
    })
});