import React from 'react';
import TierSettingItem from '../TierSettingItem';
import { shallow } from 'enzyme';

describe('commission group - TierSettingItem', () => {
    const dataItem = { TierId: 1, MinAmount: 100, MinAp: 10, Commission: 0 };
    const defaultProps = {
        data: { TierId: 1, MinAmount: 0, MaxAmount: 100, MinAp: 0, MaxAp: 10, Commission: 0.01 },
        key: 'tier-1',
        isDisabledInfinity: true,
        onRemove: jest.fn(),
        onChange: jest.fn()
    };

    it('handleChange - should call onChange when change commission', () => {
        const mockEvent = { target: { name: 'Commission', value: 'a,15' } };
        const wrapper = shallow(<TierSettingItem {...defaultProps} />);

        wrapper.find('select').simulate('change', mockEvent);

        expect(wrapper.instance().props.onChange).toHaveBeenCalledWith(1, { Commission: 0 });
    });

    it('Should render correctly', () => {
        const wrapper = shallow(<TierSettingItem {...defaultProps} />);

        expect(wrapper).toMatchSnapshot();
    });

    it('Should render correctly with MaxAmout, MaxAp', () => {
        const props = Object.assign({}, defaultProps, { data: dataItem });
        const wrapper = shallow(<TierSettingItem {...props} />);

        expect(wrapper).toMatchSnapshot();
    });
});