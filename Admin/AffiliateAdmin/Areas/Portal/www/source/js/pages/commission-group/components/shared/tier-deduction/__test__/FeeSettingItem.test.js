import React from 'react';
import FeeSettingItem from '../FeeSettingItem';
import { shallow } from 'enzyme';
import DeductedPercentageGroup from '../../deduction/DeductedPercentageGroup';

describe('commission group - FeeSettingItem', () => {
    const data = { DeductedPercentage: 0.07, DeductionId: 11, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "100.0000" }
    const defaultProps = {
        label: "Operational Fee%",
        basedValueName: "| Gross Win Loss |",
        handleChange: jest.fn(),
        data,
        isInitial: true
    };

    it('handleChangeComm - should call handleChange when change DeductedPercentage', () => {
        const mockEvent = { target: { name: 'DeductedPercentage', value: '0.15' } };
        const wrapper = shallow(<FeeSettingItem {...defaultProps} />);

        wrapper.find(DeductedPercentageGroup).simulate('change', mockEvent);

        expect(wrapper.instance().props.handleChange).toHaveBeenCalledWith({ DeductedPercentage: 0.15 });
    });

    it('handleChangeComm - should call handleChange when change DeductedPercentage when update', () => {
        const mockEvent = { target: { name: 'DeductedPercentage', value: '0.15' } };
        const props = Object.assign({}, defaultProps, { isCreating: false });
        const wrapper = shallow(<FeeSettingItem {...props} />);

        wrapper.find(DeductedPercentageGroup).simulate('change', mockEvent);

        expect(wrapper.instance().props.handleChange).toHaveBeenCalledWith({ DeductedPercentage: 0.15 });
    });

    it('handleCheck - should call props.handleChange when check Fixed Amount', () => {
        const props = Object.assign({}, defaultProps, { handleChange: jest.fn() });
        const wrapper = shallow(<FeeSettingItem {...props} />);

        wrapper.find('Input[name="IsDeductionPercent"]').simulate('change');

        expect(props.handleChange).toHaveBeenCalledWith({ IsDeductedPercent: false });
    });

    it('handleChangeAmount - should call props.handleUpdateState but not call props.handleChange', () => {
        const event = {
            target: {
                name: 'amount',
                value: '100000000009'
            }
        }
        const props = Object.assign({}, defaultProps, { handleUpdateState: jest.fn(), handleChange: jest.fn() });
        const wrapper = shallow(<FeeSettingItem {...props} />);
        const instance = wrapper.instance();

        instance.handleChangeAmount(event)
        expect(props.handleUpdateState).toHaveBeenCalled();
        expect(props.handleChange).not.toHaveBeenCalled();
    });

    it('handleChangeAmount - should call handleChange with format amount', async () => {
        const event = {
            target: {
                name: 'amount',
                value: '123456'
            }
        }
        const props = Object.assign({}, defaultProps, { handleChange: jest.fn(), isInitial: false });
        const wrapper = shallow(<FeeSettingItem {...props} />);
        const instance = wrapper.instance();

        instance.handleChangeAmount(event)

        expect(props.handleChange).toHaveBeenCalledWith({ amount: '123,456' });
    });

    it('handleChangeAmount - should call handleChange with not format amount', async () => {
        const event = {
            target: {
                name: 'amount',
                value: '123.456'
            }
        }
        const props = Object.assign({}, defaultProps, { handleChange: jest.fn(), isInitial: false });
        const wrapper = shallow(<FeeSettingItem {...props} />);
        const instance = wrapper.instance();

        instance.handleChangeAmount(event)

        expect(props.handleChange).toHaveBeenCalledWith({ amount: '123.456' });
    });

    it('Should render correctly', () => {
        const wrapper = shallow(<FeeSettingItem {...defaultProps} />);

        expect(wrapper).toMatchSnapshot();
    });

    it('Should render correctly with is fixed amount', () => {
        const newData = Object.assign({}, data, { IsDeductedPercent: false, DeductedAmount: '' });
        const props = Object.assign({}, defaultProps, { data: newData });
        const wrapper = shallow(<FeeSettingItem {...props} />);

        expect(wrapper).toMatchSnapshot();
    });
});