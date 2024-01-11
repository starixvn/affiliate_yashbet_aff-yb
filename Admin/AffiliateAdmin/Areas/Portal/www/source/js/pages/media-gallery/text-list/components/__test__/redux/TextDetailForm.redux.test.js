jest.mock('redux');
jest.mock('react-redux');
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../../actions/index';

describe('text list - TextDetailForm - connect', () => {
    let mapStateToProps, mapDispatchToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            mapDispatchToProps = mapDispatchToPropsFunc;
            return connectMock;
        });
        require('../../TextDetailForm').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            param: { category: 'Bonus' },
            categoryList: { 123: 'Bonus' },
            languageList: { 'en-US': 'English' },
            currencyList: { 234: 'EURO' },
            selectedTextId: 123
        };
        const state = {
            param: { category: 'Bonus' },
            categoryList: { 123: 'Bonus' },
            languageList: { 'en-US': 'English' },
            currencyList: { 234: 'EURO' },
            selectedTextId: 123,
            anotherProp: "Not included"
        };

        const result = mapStateToProps(state);

        expect(result).toEqual(expectedResult);
    })

    it('bindActionCreators should be called with correct parameter', () => {
        const mockBindAction = jest.fn();
        bindActionCreators.mockImplementationOnce(mockBindAction);
        const expectedActionCreator = {
            setSelectedTextId: actions['setSelectedTextId'],
            fetchData: actions['fetchData'],
            getCategoryList: actions['getCategoryList'],
            getTextDetail: actions['getTextDetail'],
            insertText: actions['insertText'],
            updateText: actions['updateText'],
            toggleLoader: actions['toggleLoader']
        }

        const dispatch = {};
        mapDispatchToProps(dispatch);

        expect(mockBindAction).toHaveBeenCalledWith(expectedActionCreator, dispatch);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const TextDetailForm = require('../../TextDetailForm').TextDetailForm;

        expect(connectMock).toHaveBeenCalledWith(TextDetailForm);
    })
})