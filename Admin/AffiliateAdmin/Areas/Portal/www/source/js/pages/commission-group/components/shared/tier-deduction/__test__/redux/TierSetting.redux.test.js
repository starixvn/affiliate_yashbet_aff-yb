jest.mock('redux');
jest.mock('react-redux');
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../../../../actions/index';

describe('commission group - TierSetting - connect', () => {
    let mapStateToProps, mapDispatchToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            mapDispatchToProps = mapDispatchToPropsFunc;
            return connectMock;
        });
        require('../../TierSetting').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            selectedGroup: 123,
            groupProductId: 123
        };

        const state = {
            selectedGroup: 123,
            anotherProp: "Not included"
        };

        const result = mapStateToProps(state, { groupProductId: 123 });

        expect(result).toEqual(expectedResult);
    })

    it('bindActionCreators should be called with correct parameter', () => {
        const mockBindAction = jest.fn();
        bindActionCreators.mockImplementationOnce(mockBindAction);
        const expectedActionCreator = {
            setSelectedGroup: actions['setSelectedGroup']
        }

        const dispatch = {};
        mapDispatchToProps(dispatch);

        expect(mockBindAction).toHaveBeenCalledWith(expectedActionCreator, dispatch);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const TierSetting = require('../../TierSetting').TierSetting;

        expect(connectMock).toHaveBeenCalledWith(TierSetting);
    })
})