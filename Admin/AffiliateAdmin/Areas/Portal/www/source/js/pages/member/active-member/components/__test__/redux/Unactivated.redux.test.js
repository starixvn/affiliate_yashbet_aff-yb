jest.mock('redux');
jest.mock('react-redux');
import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../../actions/index';

describe('active member - Content - connect', () => {
    let mapStateToProps, mapDispatchToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            mapDispatchToProps = mapDispatchToPropsFunc;
            return connectMock;
        });
        require('../../Unactivated').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            userNames: 'B2CAFF226',
            shouldResetScrollPosition: true,
            unactivatedGridData: []
        };
        const state = {
            userNames: 'B2CAFF226',
            shouldResetScrollPosition: true,
            unactivatedGridData: [],
            anotherProp: "Not included"
        };

        const result = mapStateToProps(state);

        expect(result).toEqual(expectedResult);
    })

    it('bindActionCreators should be called with correct parameter', () => {
        const mockBindAction = jest.fn();
        bindActionCreators.mockImplementationOnce(mockBindAction);
        const expectedActionCreator = {
            setMessage: actions['setMessage'],
            activeMember: actions['activeMember'],
            setShouldResetScrollPosition: actions['setShouldResetScrollPosition']
        }

        const dispatch = {};
        mapDispatchToProps(dispatch);

        expect(mockBindAction).toHaveBeenCalledWith(expectedActionCreator, dispatch);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const Unactivated = require('../../Unactivated').Unactivated;

        expect(connectMock).toHaveBeenCalledWith(React.memo(Unactivated));
    })
})