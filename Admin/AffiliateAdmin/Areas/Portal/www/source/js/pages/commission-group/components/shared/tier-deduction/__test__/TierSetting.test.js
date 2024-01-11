import React from 'react';
import { TierSetting } from '../TierSetting';
import TierSettingItem from '../TierSettingItem';
import { shallow } from 'enzyme';

describe('commission group - TierSetting', () => {
    const defaultSelectedGroup = {
        BankingFee: { DeductedPercentage: 0.02, DeductionId: 12, ProductIds: 0 },
        Description: "A B C",
        GroupId: 0,
        GroupName: "ABC",
        PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4,5,6", GroupProductId: 123 }],
        TierSettingList: [{ GroupProductId: 123, TierId: 1, MinAmount: 0, MinAp: 5, Commission: 0 },
            { GroupProductId: 123, TierId: 2, MinAmount: 1, MinAp: 5, Commission: 0.1 }]
    }
    const defaultProps = {
        groupProductId: 123,
        selectedGroup: defaultSelectedGroup
    };

    it('handleAdd - should call setSelectedGroup when click button Add Tier', () => {
        const expectParam = {
            TierSettingList: [
                { GroupProductId: 123, Commission: 0, MinAmount: 0, MinAp: 5, TierId: 1 },
                { GroupProductId: 123, Commission: 0.1, MinAmount: 1, MinAp: 5, TierId: 2 },
                { GroupProductId: 123, Commission: 0.2, MinAmount: 2, MinAp: 5, TierId: 3 }
            ]
        };

        const props = Object.assign({}, defaultProps, { setSelectedGroup: jest.fn() });
        const wrapper = shallow(<TierSetting {...props} />);

        wrapper.instance().handleAdd();

        expect(props.setSelectedGroup).toHaveBeenCalledWith(expectParam);
    });

    it('handleAdd - should call setSelectedGroup when click button Add Tier with TierSettingList is null', () => {
        const selectedGroup = Object.assign({}, defaultSelectedGroup, { TierSettingList: [] });
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup, selectedGroup: selectedGroup });
        const wrapper = shallow(<TierSetting {...props} />);

        wrapper.instance().handleAdd();
        expect(props.setSelectedGroup).toHaveBeenCalledWith({ TierSettingList: [{ GroupProductId: 123, Commission: 0, MinAmount: 0, MinAp: 5, TierId: 1 }] });
    });

    //it('handleRemove - Should be called when click remove', () => {
    //    const wrapper = shallow(<TierSetting {...defaultProps} />);
    //    wrapper.instance().handleRemove = jest.fn();

    //    wrapper.find(TierSettingItem).at(1).simulate('remove', 2);

    //    expect(wrapper.instance().handleRemove).toHaveBeenCalled();
    //});

    it('handleRemove - Should call setSelectedGroup', () => {
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<TierSetting {...props} />);
        wrapper.instance().handleRemove(1, 123);

        expect(props.setSelectedGroup).toHaveBeenCalledWith({ TierSettingList: [{ GroupProductId: 123, TierId: 2, Commission: 0.1, MinAmount: 1, MinAp: 5 }] })
    });

    it('handleChangeInput - Should call setSelectedGroup', () => {
        const expectParam = {
            TierSettingList: [
                { GroupProductId: 123, Commission: 0, MinAmount: 10, MinAp: 10, TierId: 1 },
                { GroupProductId: 123, Commission: 0.1, MinAmount: 1, MinAp: 5, TierId: 2 }
            ]
        };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<TierSetting {...props} />);
        wrapper.find(TierSettingItem).at(1).simulate('change', 1, { MinAmount: 10, MinAp: 10 });

        expect(props.setSelectedGroup).toHaveBeenCalledWith(expectParam);
    });

    it('handleChangeInput - Should call setSelectedGroup with tierItemValue is incorrect', () => {
        const props = Object.assign({}, defaultProps, { setSelectedGroup: jest.fn() });
        const wrapper = shallow(<TierSetting {...props} />);
        wrapper.find(TierSettingItem).at(1).simulate('change', 1, { MinAmount: 'abc', MinAp: 0 });

        expect(props.setSelectedGroup).not.toHaveBeenCalled();
    });

    it('Should render correctly', () => {
        const wrapper = shallow(<TierSetting {...defaultProps} />);

        expect(wrapper).toMatchSnapshot();
    });

    it('Should render correctly with TierSettingList is null', () => {
        const selectedGroup = Object.assign({}, defaultSelectedGroup, { TierSettingList: [] });
        const props = Object.assign({}, defaultProps, { selectedGroup, setSelectedGroup: jest.fn() });
        const wrapper = shallow(<TierSetting {...props} />);

        expect(wrapper).toMatchSnapshot();
    });
})