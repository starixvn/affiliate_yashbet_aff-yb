import * as actions from '../index';
import * as types from '../types';
import * as ajaxRequest from '../../../../../modules/ajaxRequest';

import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

const runTestAction = ({ name, type, data }) => {
    it(`should create action to ${name}`, () => {
        const expectedAction = { type, data };

        expect(actions[name](data)).toEqual(expectedAction);
    });
}

const actionList = [
    { name: 'setParam', type: types.SET_PARAM, data: { Id: 123 } },
    { name: 'setSelectedTextId', type: types.SET_SELECTED_TEXT_ID, data: 123456 },
    { name: 'setCheckedTexts', type: types.SET_CHECKED_TEXTS, data: { 123456: false } },
    { name: 'setShouldResetScrollPosition', type: types.SHOULD_RESET_SCROLL_POSITION, data: false },
    { name: 'setDisableCurrency', type: types.SET_DISABLE_CURRENCY, data: true },
    { name: 'setCategoryList', type: types.SET_CATEGORY_LIST, data: [{ id: 234, name: 'SP Bonus' }] },
    { name: 'setGridData', type: types.SET_GRID_DATA, data: 'setGridData' }
];

describe('text list actions', () => {
    actionList.forEach(item => {
        runTestAction(item);
    });

    it('should create action to getCategoryList', async () => {
        const expectedResponse = { data: [{ Id: 123, Name: 'SP Bonus' }], status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const expectedAction = [
            { type: 'SET_CATEGORY_LIST', data: [{ Id: 123, Name: 'SP Bonus' }] }
        ];

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getCategoryList'](1)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/GetCategoryList', { userType: 1 });
            expect(store.getActions()).toEqual(expectedAction);
        })
    })

    it('should create action to getCategoryList', async () => {
        ajaxRequest.post = jest.fn();

        const expectedAction = [
            { type: 'SET_CATEGORY_LIST', data: [] }
        ];

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getCategoryList'](1)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/GetCategoryList', { userType: 1 });
            expect(store.getActions()).toEqual(expectedAction);
        })
    })

    it('should create action to fetchData with correct parameter', () => {
        const expectedResponse = {
            data: {
                MainData: [
                    {
                        Category: "Tennis",
                        Currency: "EURO",
                        DeviceType: 1,
                        Language: "Japanese",
                        TextContent: "anna 03",
                        TextId: 2438
                    },
                    {
                        Category: "Basketball",
                        Currency: "EURO",
                        DeviceType: 1,
                        Language: "Japanese",
                        TextContent: "anna 02",
                        TextId: 2437
                    }
                ],
                TotalItem: 2
            },
            status: 200
        };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const expectedAction = [
            { type: 'TOGGLE_LOADER', data: true },
            { type: types.SET_GRID_DATA, data: expectedResponse.data.MainData },
            { type: 'SET_PAGING_OPTION', data: { PageIndex: 1, PageSize: 20, TotalItem: 2 } },
            { type: types.SHOULD_RESET_SCROLL_POSITION, data: true },
            { type: types.SET_CHECKED_TEXTS, data: { 2438: false, 2437: false } },
            { type: 'TOGGLE_LOADER', data: false },
        ];
        const param = {
            PageIndex: 1,
            PageSize: 20,
            category: "",
            currency: "",
            deviceType: 0,
            language: ""
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['fetchData'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/GetTextList', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })

    it('should create action to fetchData with incorrect parameter', () => {
        ajaxRequest.post = jest.fn();

        const expectedAction = [
            { type: 'TOGGLE_LOADER', data: true },
            { type: types.SET_GRID_DATA, data: [] },
            { type: 'SET_PAGING_OPTION', data: { PageIndex: 1, PageSize: 20, TotalItem: 0 } },
            { type: types.SHOULD_RESET_SCROLL_POSITION, data: true },
            { type: types.SET_CHECKED_TEXTS, data: {} },
            { type: 'TOGGLE_LOADER', data: false },
        ];
        const param = {
            PageIndex: 1,
            PageSize: 20,
            category: "Bonus",
            currency: "VND",
            deviceType: 0,
            language: "English"
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['fetchData'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/GetTextList', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })

    it('should create action to getTextDetail with correct parameter', async () => {
        const expectedResponse = {
            data: {
                Category: "Basketball",
                Currency: "EURO",
                DeviceType: 1,
                Language: "Japanese",
                TextContent: "anna 02",
                TextId: 2437
            }, status: 200
        };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));
        const selectedTextId = 2437;

        const result = await actions['getTextDetail'](selectedTextId)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/GetTextDetail', { selectedTextId: 2437 });

        expect(result).toEqual(expectedResponse.data);
    })

    it('should create action to getTextDetail with incorrect parameter', async () => {
        ajaxRequest.post = jest.fn();
        const selectedTextId = 123456;

        const result = await actions['getTextDetail'](selectedTextId)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/GetTextDetail', { selectedTextId: 123456 });
        expect(result).toEqual(null);
    })

    it('should create action to insertText with correct parameter', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: '' }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));
        const param = {
            TextId: 0,
            category: "Football",
            currency: "EURO",
            deviceType: 0,
            language: "Japanese",
            textContent: "Test"
        };

        const result = await actions['insertText'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/insertText', param);

        expect(result).toEqual({ IsSuccessful: true, Message: "" });
    })

    it('should create action to insertText with incorrect parameter', async () => {
        ajaxRequest.post = jest.fn();
        const param = {
            TextId: 0,
            category: "Football",
            currency: "EURO",
            deviceType: 0,
            language: "Japanese",
            textContent: ""
        };

        const result = await actions['insertText'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/insertText', param);
        expect(result).toEqual(null);
    })

    it('should create action to updateText with correct parameter', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: '' }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));
        const param = {
            Category: "Basketball",
            Currency: "EURO",
            DeviceType: 1,
            Language: "Japanese",
            TextContent: "anna 02 abc",
            TextId: 2437
        };

        const result = await actions['updateText'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/UpdateText', param);

        expect(result).toEqual({ IsSuccessful: true, Message: "" });
    })

    it('should create action to updateText with incorrect parameter', async () => {
        ajaxRequest.post = jest.fn();
        const param = {
            Category: "Basketball",
            Currency: "EURO",
            DeviceType: 1,
            Language: "Japanese",
            TextContent: "",
            TextId: 2437
        };

        const result = await actions['updateText'](param)();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/UpdateText', param);
        expect(result).toEqual(null);
    })

    it('should create action to deleteTexts with correct parameter', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: '' }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));
        const textIdList = '2437,2438';

        const result = await actions['deleteTexts']({ textIdList })();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/DeleteTexts', { textIdList });

        expect(result).toEqual({ IsSuccessful: true, Message: "" });
    })

    it('should create action to deleteTexts with incorrect parameter', async () => {
        ajaxRequest.post = jest.fn();
        const textIdList = '123456';

        const result = await actions['deleteTexts']({ textIdList })();

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/TextList/DeleteTexts', { textIdList });
        expect(result).toEqual(null);
    })
});