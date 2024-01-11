import React from 'react';
import { Filter } from '../Filter';
import { shallow } from 'enzyme';

describe('active member - components - Filter ', () => {
    it('render correctly', () => {
        const result = shallow(<Filter />);

        expect(result).toMatchSnapshot();
    });

    it('_handleSearch - should call _handleFetchData', async () => {
        const props = Object.assign({}, { setUserNames: jest.fn(), fetchData: jest.fn() });
        const wrapper = shallow(<Filter {...props} />);

        wrapper.find('Input[type="text"]').simulate('change', { target: { value: 'xyl157 ' } });
        await wrapper.find('Button').simulate('click');

        expect(props.setUserNames).toHaveBeenCalledWith('xyl157');
        expect(props.fetchData).toHaveBeenCalledWith({ userNames: 'xyl157' });
    });

    it('_handleSearch - should call setMessage with userName is exceed', async () => {
        const props = Object.assign({}, { setMessage: jest.fn() });
        const wrapper = shallow(<Filter {...props} />);

        wrapper.find('Input[type="text"]').simulate('change', { target: { value: ',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,' } });
        await wrapper.find('Button').simulate('click');

        expect(props.setMessage).toHaveBeenCalledWith('The function can only check up to 100 accounts per request!');
    });

    it('_handleSearch - should call setMessage with userName is exceed', async () => {
        const props = Object.assign({}, { setMessage: jest.fn() });
        const wrapper = shallow(<Filter {...props} />);

        wrapper.find('Input[type="text"]').simulate('change', { target: { value: '' } });
        await wrapper.find('Button').simulate('click');

        expect(props.setMessage).toHaveBeenCalledWith('Please input Deposit User Name!');
    });

    it('_handlePressEnter - should call _handleSearch when press enter in input', () => {
        const event = {
            charCode: 13,
            preventDefault: jest.fn()
        };
        const props = Object.assign({}, { setMessage: jest.fn() })
        const wrapper = shallow(<Filter {...props} />);

        wrapper.find('Input[type="text"]').simulate('keyPress', event);

        expect(event.preventDefault).toHaveBeenCalled();
        expect(props.setMessage).toHaveBeenCalledWith('Please input Deposit User Name!');
    });

    it('_handlePressEnter - should not call _handleSearch when press other keys', () => {
        const event = {
            charCode: 16,
            preventDefault: jest.fn()
        };
        const wrapper = shallow(<Filter />);

        wrapper.find('Input[type="text"]').simulate('keyPress', event);

        expect(event.preventDefault).not.toHaveBeenCalled();
    });
})