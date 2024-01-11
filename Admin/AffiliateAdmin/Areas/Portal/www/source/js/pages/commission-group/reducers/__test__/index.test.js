import * as types from '../../actions/types'
import * as reducers from '../index'

const runTestReducer = ({ name, value, action, expectedResult }) => {
    it(`should handle ${name} reducer with type ${action.type}`, () => {
        const result = reducers[name](value, action);
        expect(result).toEqual(expectedResult);
    });
}

const testCaseList = [
    { name: 'gridData', value: null, action: { type: types.SET_GRIDDATA, data: [{ gridData: 'gridData' }] }, expectedResult: [{ gridData: 'gridData' }] },
    { name: 'searchedGroupName', value: '', action: { type: types.SET_SEARCHED_GROUPNAME, data: 'abc' }, expectedResult: 'abc' },
    { name: 'selectedGroup', value: null, action: { type: types.SET_SELECTED_GROUP, data: { selectedGroup: 'selectedGroup' } }, expectedResult: { selectedGroup: 'selectedGroup' } },
    { name: 'selectedGroup', value: null, action: { type: types.SET_SELECTED_GROUP, data: undefined }, expectedResult: null },
    { name: 'selectedProductIds', value: [], action: { type: types.SET_SELECTED_PRODUCT_IDS, data: [1, 2, 3] }, expectedResult: [ 1, 2, 3 ] },
]

describe('commission group reducers', () => {
    testCaseList.forEach((item) => {
        runTestReducer(item);
    })
});