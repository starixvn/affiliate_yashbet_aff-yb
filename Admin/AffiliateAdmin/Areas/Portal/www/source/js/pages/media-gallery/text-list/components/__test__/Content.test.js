import React from 'react';
import { Content } from '../Content';
import { shallow, mount } from 'enzyme';

import { defaultPagingOption } from '../../../../../modules/constants';
import { Table } from '@9softstudio/react-fixed-table-header';

describe('text list - components - Content ', () => {
    const gridData = [
        {
            Category: "Tennis",
            Currency: "EURO",
            DeviceType: 1,
            Language: "Japanese",
            TextContent: "anna 03",
            TextId: 2438
        },
        {
            Category: "Basketball",
            Currency: "EURO",
            DeviceType: 0,
            Language: "Japanese",
            TextContent: "anna 02",
            TextId: 2437
        }
    ];

    const param = {
        PageIndex: 1,
        PageSize: 20,
        category: "",
        currency: "",
        deviceType: 0,
        language: ""
    };

    const defaultProps = {
        pagingOption: defaultPagingOption,
        checkedTexts: { 2437: false, 2438: false },
        shouldResetScrollPosition: true,
        param,
        gridData
    };

    it('render correctly', () => {
        const result = shallow(<Content {...defaultProps} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with gridData is null', () => {
        const props = Object.assign({}, defaultProps, { gridData: null })
        const result = shallow(<Content {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with gridData is empty', () => {
        const props = Object.assign({}, defaultProps, { gridData: [] })
        const result = shallow(<Content {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('_handlePaging - should be called when onPaging in Table', () => {
        const props = Object.assign({}, defaultProps, { setPagingOption: jest.fn(), setParam: jest.fn() })
        const result = shallow(<Content {...props} />);

        result.find(Table).props().onPaging(50, 50);

        expect(props.setPagingOption).toHaveBeenCalledWith({ PageIndex: 50, PageSize: 50 });
        expect(props.setParam).toHaveBeenCalledWith({ PageIndex: 50, PageSize: 50, category: "", currency: "", deviceType: 0, language: "" });
    });

    it('setSelectedTextId - should be called when click edit button', () => {
        const props = Object.assign({}, defaultProps, { setSelectedTextId: jest.fn(), fetchData: jest.fn() })
        const result = mount(<Content {...props} />);

        result.find('Button').at(0).simulate('click');

        expect(props.setSelectedTextId).toHaveBeenCalledWith(2438);
        expect(props.fetchData).toHaveBeenCalledWith({ PageIndex: 1, PageSize: 20, category: "", currency: "", deviceType: 0, language: "" });
    });

    it('_onCheckItem - should be called when click edit button', () => {
        const event = {
            target: {
                checked: true
            }
        };
        const props = Object.assign({}, defaultProps, { setShouldResetScrollPosition: jest.fn(), setCheckedTexts: jest.fn(), fetchData: jest.fn() })
        const result = mount(<Content {...props} />);

        result.find('input[type="checkbox"]').at(0).simulate('change', event);

        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
        expect(props.setCheckedTexts).toHaveBeenCalledWith({ 2437: false, 2438: true });
        expect(props.fetchData).toHaveBeenCalledWith({ PageIndex: 1, PageSize: 20, category: "", currency: "", deviceType: 0, language: "" });
    });
})