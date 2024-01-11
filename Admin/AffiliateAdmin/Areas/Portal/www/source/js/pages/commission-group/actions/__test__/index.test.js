import * as actions from '../index';
import * as types from '../types';
import { defaultPagingOption } from '../../../../modules/constants';
import * as ajaxRequest from '../../../../modules/ajaxRequest';

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

const runTestActionReturnPost = ({ name, url }) => {
    it(`should create action to ${name}`, () => {
        ajaxRequest.post = jest.fn();
        actions[name](defaultPagingOption)();

        expect(ajaxRequest.post).toHaveBeenCalledWith(url, defaultPagingOption);
    });
}

const actionList = [
    { name: 'setGridData', type: types.SET_GRIDDATA, data: 'setGridData' },
    { name: 'setSelectedGroup', type: types.SET_SELECTED_GROUP, data: 'setSelectedGroup' },
    { name: 'setSelectedProductIds', type: types.SET_SELECTED_PRODUCT_IDS, data: '1,2,3' },
    { name: 'setSearchedGroupName', type: types.SET_SEARCHED_GROUPNAME, data: 'setSearchedGroupName' }
];

const actionListReturnPost = [
    { name: 'insertCommissionGroup', url: 'Portal/RevenueGroup/CreateCommissionGroup' },
    { name: 'updateCommissionGroup', url: 'Portal/RevenueGroup/UpdateCommissionGroup' },
    { name: 'deleteCommGroup', url: 'Portal/RevenueGroup/DeleteCommissionGroup' }
];

describe('commission group actions', () => {
    it('should create action to fetchGroupSettingData', () => {
        ajaxRequest.post = jest.fn();
        actions['fetchGroupSettingData'](defaultPagingOption);

        expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/RevenueGroup/GetCommissionGroupList', defaultPagingOption);
    })

    actionList.forEach(item => {
        runTestAction(item);
    });

    actionListReturnPost.forEach(item => {
        runTestActionReturnPost(item);
    });

    it('should create action to getGroupSettingList with correct parameter', () => {
        const expectedResponse = {
            data: {
                CommissionSettingList: [
                    {
                        BankingFee: {
                            DeductedPercentage: 0.02,
                            DeductionId: 12,
                            DeductionName: "Banking Fee",
                            GroupId: 133,
                            ProductIds: "0"
                        },
                        CreatedDate: "2018-08-13T04:57:37.803",
                        CreatedDateText: "08/13/2018 04:57:37",
                        Creator: "pgmPercy",
                        Description: "testing",
                        GroupId: 133,
                        GroupName: "NewGroup",
                        NoAffs: 8,
                        PlatformFeeList: [
                            {
                                DeductedPercentage: 0.17,
                                DeductionId: 10,
                                DeductionName: "Platform Fee",
                                GroupId: 133,
                                ProductIds: "1,3,4,5,6,7,8,13,14,15,16,17,18"
                            }
                        ],
                        P2pDeductionList: [
                            {
                                DeductedPercentage: 0.17,
                                DeductionId: 15,
                                DeductionName: "P2P Fee",
                                GroupId: 133,
                                ProductIds: "2,9,10,11,12"
                            }
                        ],
                        RowIdx: 1,
                        TierSettingList: [
                            {
                                Commission: 0.2,
                                GroupId: 133,
                                MaxAmount: 15,
                                MaxAp: 5,
                                MinAmount: 0,
                                MinAp: 0,
                                TierId: 1
                            },
                            {
                                Commission: 0,
                                GroupId: 133,
                                MaxAmount: 100000000000000,
                                MaxAp: 1000000000,
                                MinAmount: 16,
                                MinAp: 6,
                                TierId: 1
                            }
                        ]
                    }
                ],
                TotalRows: 1
            },
            status: 200
        };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const expectedAction = [
            { type: 'TOGGLE_LOADER', data: true },
            { type: types.SET_SEARCHED_GROUPNAME, data: '' },
            { type: types.SET_GRIDDATA, data: expectedResponse.data.CommissionSettingList },
            { type: 'SET_PAGING_OPTION', data: { PageIndex: 1, PageSize: 20, TotalItem: 1 } },
            { type: 'TOGGLE_LOADER', data: false },
        ];
        const param = {
            GroupName: '',
            PageIndex: 1,
            PageSize: 20
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getGroupSettingList'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/RevenueGroup/GetCommissionGroupList', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })

    it('should create action to getGroupSettingList with correct parameter and have not data', () => {
        const expectedResponse = {
            data: null,
            status: 200
        };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const expectedAction = [
            { type: 'TOGGLE_LOADER', data: true },
            { type: types.SET_GRIDDATA, data: null },
            { type: 'SET_PAGING_OPTION', data: { PageIndex: 1, PageSize: 20, TotalItem: 0 } },
            { type: 'TOGGLE_LOADER', data: false },
        ];
        const param = {
            GroupName: 'abc123abc',
            PageIndex: 1,
            PageSize: 20
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getGroupSettingList'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/RevenueGroup/GetCommissionGroupList', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })

    it('should create action to getGroupSettingList with incorrect parameter', () => {
        ajaxRequest.post = jest.fn();

        const expectedAction = [
            { type: 'TOGGLE_LOADER', data: true },
            { type: 'TOGGLE_LOADER', data: false }
        ];
        const param = {
            GroupName: 'abc123abc',
            PageIndex: -1,
            PageSize: -20
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getGroupSettingList'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/RevenueGroup/GetCommissionGroupList', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })

    it('should create action to getSelectedCommGroup with correct parameter', () => {
        const expectedResponse = {
            data: {
                BankingFee: { DeductedPercentage: 0.02, DeductionId: 12, ProductIds: 0 },
                Description: "",
                GroupId: 0,
                GroupName: "",
                PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4" }],
                P2pDeductionList: [{ DeductedPercentage: 0.15, DeductionId: 15, ProductIds: "2,5", Commission: 0.1 }],
                TierSettingList: []
            },
            status: 200
        };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const expectedAction = [
            { type: types.SET_SELECTED_PRODUCT_IDS, data: [2, 5] },
            { type: types.SET_SELECTED_GROUP, data: expectedResponse.data }
        ];
        const param = {
            GroupId: 102
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getSelectedCommGroup'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/RevenueGroup/GetCommissionGroup', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })

    it('should create action to getSelectedCommGroup with incorrect parameter', () => {
        ajaxRequest.post = jest.fn();

        const expectedAction = [
            { type: types.SET_SELECTED_PRODUCT_IDS, data: [] },
            { type: types.SET_SELECTED_GROUP, data: null }
        ];
        const param = {
            GroupId: 'abc'
        }

        const store = mockStore({ todos: [] });
        return store.dispatch(actions['getSelectedCommGroup'](param)).then(() => {
            expect(ajaxRequest.post).toHaveBeenCalledWith('Portal/RevenueGroup/GetCommissionGroup', param);
            expect(store.getActions()).toEqual(expectedAction)
        })
    })
});