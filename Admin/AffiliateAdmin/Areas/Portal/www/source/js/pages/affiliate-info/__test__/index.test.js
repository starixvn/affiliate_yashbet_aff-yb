import React from 'react';
import App from '../index';
import { shallow } from 'enzyme';

jest.mock('react-dom', () => ({ render: jest.fn() }))

describe('affiliate info - App', () => {
    const initialState = {
        languageList: [
            {
                LanguageCode: 'en-US',
                LanguageName: 'English'
            }
        ],
        commGroupList: [
            {
                Id: 123,
                Name: 'Test'
            }
        ],
        param: {},
        groupId: 0
    }

    it('Should render correctly', () => {
        window.initialState = initialState;

        const wrapper = shallow(<App />);

        expect(wrapper).toMatchSnapshot();
    });

    it('Should render correctly 2', () => {
        window.initialState = {
            ...initialState,
            groupId: 123
        }

        const wrapper = shallow(<App />);

        expect(wrapper).toMatchSnapshot();
    });
})