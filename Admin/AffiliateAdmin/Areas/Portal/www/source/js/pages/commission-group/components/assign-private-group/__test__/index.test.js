import React from 'react';
import App from '../index';
import { shallow } from 'enzyme';

jest.mock('react-dom', () => ({ render: jest.fn() }))

describe('commission group - App', () => {
    beforeEach(() => {
        window.initialState = {
            param: {
                FromDate: "6/25/2019",
                IsGetAll: false,
                PageIndex: 1,
                PageSize: 20,
                SearchBy: 1,
                Status: null,
                ToDate: "6/26/2019",
                Username: null
            },
            todate: "6/26/2019"
        }
    });

    it('Should render correctly', () => {
        const wrapper = shallow(<App />);

        expect(wrapper).toMatchSnapshot();
    });
})