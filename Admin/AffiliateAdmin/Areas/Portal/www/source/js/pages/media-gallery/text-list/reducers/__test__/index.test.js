import * as types from '../../actions/types';
import * as reducers from '../index';

const runTestReducer = ({ name, value, action, expectedResult }) => {
    it(`should handle ${name} reducer with type ${action.type}`, () => {
        const result = reducers[name](value, action);
        expect(result).toEqual(expectedResult);
    });
}

const testCaseList = [
    { name: 'param', value: null, action: { type: types.SET_PARAM, data: { AffId: 123456 } }, expectedResult: { AffId: 123456 } },
    { name: 'selectedTextId', value: null, action: { type: types.SET_SELECTED_TEXT_ID, data: 123 }, expectedResult: 123 },
    { name: 'checkedTexts', value: null, action: { type: types.SET_CHECKED_TEXTS, data: { 123: true } }, expectedResult: { 123: true } },
    { name: 'shouldResetScrollPosition', value: true, action: { type: types.SHOULD_RESET_SCROLL_POSITION, data: false }, expectedResult: false },
    { name: 'gridData', value: null, action: { type: types.SET_GRID_DATA, data: [{ name: 'abc' }] }, expectedResult: [{ name: 'abc' }] },
    { name: 'categoryList', value: null, action: { type: types.SET_CATEGORY_LIST, data: [{ name: 'abc' }] }, expectedResult: [{ name: 'abc' }] },
    { name: 'isDisableCurrency', value: null, action: { type: types.SET_DISABLE_CURRENCY, data: true }, expectedResult: true }
]

describe('text list reducers', () => {
    testCaseList.forEach((item) => {
        runTestReducer(item);
    })
});