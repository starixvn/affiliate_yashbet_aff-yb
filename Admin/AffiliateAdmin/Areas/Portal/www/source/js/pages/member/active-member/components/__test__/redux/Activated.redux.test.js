jest.mock('redux');
jest.mock('react-redux');
import React from 'react';
import { connect } from 'react-redux';

describe('active member - Content - connect', () => {
    let mapStateToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            return connectMock;
        });
        require('../../Activated').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            activatedGridData: []
        };
        const state = {
            activatedGridData: [],
            anotherProp: "Not included"
        };

        const result = mapStateToProps(state);

        expect(result).toEqual(expectedResult);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const Activated = require('../../Activated').Activated;

        expect(connectMock).toHaveBeenCalledWith(React.memo(Activated));
    })
})