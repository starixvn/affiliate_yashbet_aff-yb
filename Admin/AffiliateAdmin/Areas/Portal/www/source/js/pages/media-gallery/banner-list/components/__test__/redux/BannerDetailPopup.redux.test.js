jest.mock('redux');
jest.mock('react-redux');
import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../../actions/index';

describe('banner list - BannerDetailPopup - connect', () => {
    let mapStateToProps, mapDispatchToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            mapDispatchToProps = mapDispatchToPropsFunc;
            return connectMock;
        });
        require('../../BannerDetailPopup').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            bannerDetailUrl: { bannerUrl: "abc" }
        };
        const state = {
            bannerDetailUrl: { bannerUrl: "abc" },
            anotherProp: "Not included"
        };

        const result = mapStateToProps(state);

        expect(result).toEqual(expectedResult);
    })

    it('bindActionCreators should be called with correct parameter', () => {
        const mockBindAction = jest.fn();
        bindActionCreators.mockImplementationOnce(mockBindAction);
        const expectedActionCreator = {
            setBannerDetailUrl: actions['setBannerDetailUrl']
        }

        const dispatch = {};
        mapDispatchToProps(dispatch);

        expect(mockBindAction).toHaveBeenCalledWith(expectedActionCreator, dispatch);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const BannerDetailPopup = require('../../BannerDetailPopup').BannerDetailPopup;

        expect(connectMock).toHaveBeenCalledWith(React.memo(BannerDetailPopup));
    })
})