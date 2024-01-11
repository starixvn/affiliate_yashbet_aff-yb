import * as types from '../../actions/types';
import * as reducers from '../index';

const runTestReducer = ({ name, value, action, expectedResult }) => {
    it(`should handle ${name} reducer with type ${action.type}`, () => {
        const result = reducers[name](value, action);
        expect(result).toEqual(expectedResult);
    });
}

const testCaseList = [
    { name: 'userNames', value: null, action: { type: types.SET_USER_NAMES, data: "B2CAFF226" }, expectedResult: "B2CAFF226" },
    { name: 'isShowText', value: null, action: { type: types.SET_SHOW_TEXT, data: true }, expectedResult: true },
    { name: 'message', value: null, action: { type: types.SET_MESSAGE, data: "abc" }, expectedResult: "abc" },
    { name: 'memberNotExist', value: '', action: { type: types.SET_MEMBER_NOT_EXIST, data: "abc" }, expectedResult: "abc" },
    { name: 'shouldResetScrollPosition', value: true, action: { type: types.SHOULD_RESET_SCROLL_POSITION, data: false }, expectedResult: false },
    { name: 'activatedGridData', value: null, action: { type: types.SET_ACTIVATED_GRID_DATA, data: [{ name: 'abc' }] }, expectedResult: [{ name: 'abc' }] },
    { name: 'unactivatedGridData', value: null, action: { type: types.SET_UNACTIVATED_GRID_DATA, data: [{ name: '123' }] }, expectedResult: [{ name: '123' }] }
]

describe('active member reducers', () => {
    testCaseList.forEach((item) => {
        runTestReducer(item);
    })
})