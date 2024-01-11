import React from 'react';
import { Filter } from '../Filter';
import { shallow } from 'enzyme';

import { defaultPagingOption } from '../../../../../modules/constants';

describe('text list - components - Filter ', () => {
    const defaultProps = {
        pagingOption: defaultPagingOption,
        categoryList: [
            { CategoryName: "Basketball" },
            { CategoryName: "Bonus" }
        ],
        languageList: [
            { LanguageName: "English" },
            { LanguageName: "Vietnamese" }
        ],
        currencyList: [
            { Name: "EURO" },
            { Name: "VND" }
        ]
    }

    it('render correctly', () => {
        const result = shallow(<Filter {...defaultProps} />);

        expect(result).toMatchSnapshot();
    });

    it('_handleSearch - should be called when click Search Button', () => {
        const setParam = jest.fn();
        const props = Object.assign({}, defaultProps, { setParam });
        const wrapper = shallow(<Filter {...props} />);

        wrapper.find('Button').simulate('click');

        expect(props.setParam).toHaveBeenCalledWith({ category: '', language: '', currency: '', deviceType: '', userType: 0, PageIndex: 1, PageSize: 20 });
    });

    it('_handleChangeUserType - should call setDisableCurrency and getCategoryList when change UserType select', () => {
        const event = {
            target: { value: '1' }
        };
        const props = { ...defaultProps, setDisableCurrency: jest.fn(), getCategoryList: jest.fn() }
        const wrapper = shallow(<Filter {...props} />);

        wrapper.find('Input[type="select"][id="userType"]').simulate('change', event);

        expect(wrapper.find('Input[type="select"][id="userType"]').prop('value')).toEqual('1');
        expect(props.setDisableCurrency).toHaveBeenCalledWith(false);
        expect(props.getCategoryList).toHaveBeenCalledWith('1');
    });

    it('setCategory - should change category when change category select', () => {
        const event = {
            target: { value: 'category' }
        };
        const wrapper = shallow(<Filter {...defaultProps} />);

        wrapper.find('Input[type="select"][id="category"]').simulate('change', event);

        expect(wrapper.find('Input[type="select"][id="category"]').prop('value')).toEqual('category');
    });

    it('setLanguage - should change category when change language select', () => {
        const event = {
            target: { value: 'language' }
        };
        const wrapper = shallow(<Filter {...defaultProps} />);

        wrapper.find('Input[type="select"][id="language"]').simulate('change', event);

        expect(wrapper.find('Input[type="select"][id="language"]').prop('value')).toEqual('language');
    });

    it('setCurrency - should change category when change currency select', () => {
        const event = {
            target: { value: 'currency' }
        };
        const wrapper = shallow(<Filter {...defaultProps} />);

        wrapper.find('Input[type="select"][id="currency"]').simulate('change', event);

        expect(wrapper.find('Input[type="select"][id="currency"]').prop('value')).toEqual('currency');
    });

    it('setDeviceType - should change category when change deviceType select', () => {
        const event = {
            target: { value: 'mobile' }
        };
        const wrapper = shallow(<Filter {...defaultProps} />);

        wrapper.find('Input[type="select"][id="deviceType"]').simulate('change', event);

        expect(wrapper.find('Input[type="select"][id="deviceType"]').prop('value')).toEqual('mobile');
    });
})