import React from 'react';
import { PlatformFeeSetting } from '../PlatformFeeSetting';
import { shallow } from 'enzyme';
import PlatformFeeItem from "../PlatformFeeItem";

describe('commission group - platform fee setting', () => {
    const defaultSelectedGroup = {
        BankingFee: { DeductedPercentage: 0.02, DeductionId: 12, ProductIds: 0 },
        Description: "",
        GroupId: 0,
        GroupName: "",
        PlatformFeeList: [{ GroupProductId: 123, DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3" },
            { GroupProductId: 234, DeductedPercentage: 0.12, DeductionId: 11, ProductIds: "4" }],
        P2pDeductionList: [{ DeductedPercentage: 0.15, DeductionId: 15, ProductIds: "2", Commission: 0.1 },
            { DeductedPercentage: 0.1, DeductionId: 15, ProductIds: "5", Commission: 0.15 }],
        TierSettingList: [
            { GroupProductId: 123, TierId: 1, MinAmount: 0, MinAp: 5, Commission: 0 },
            { GroupProductId: 234, TierId: 1, MinAmount: 0, MinAp: 5, Commission: 0 }
        ]
    };
    const defaultProps = {
        selectedGroup: defaultSelectedGroup,
        affInfo: {
            AffId: 927841,
            AffType: 4,
            Email: "walter.tran1@starixsoft.com",
            FirstName: "媒体册 获取以更新吧！",
            GroupId: 102,
            MailOption: false,
            PlayerUsername: "qatest01",
            PreferredLanguage: "hi-IN",
            UserName: "B2CAFF001"
        },
        deductionList: [
            { DeductionId: 10, DeductionName: "Platform Fee", DeductedPercentage: 0.17, IsDeductedPercent: true, DeductedAmount: "" },
            { DeductionId: 11, DeductionName: "Operational Fee", DeductedPercentage: 0.07, IsDeductedPercent: true, DeductedAmount: "" },
            { DeductionId: 13, DeductionName: "Deposit Fee", DeductedPercentage: 0.02, IsDeductedPercent: true, DeductedAmount: "" },
            { DeductionId: 14, DeductionName: "Withdrawal Fee", DeductedPercentage: 0.02, IsDeductedPercent: true, DeductedAmount: "" },
            { DeductionId: 15, DeductionName: "P2P Fee", DeductedPercentage: 0.02, Commission: 0.01 }
        ],
        productList: [
            { ProductId: 1, ProductName: "Sportsbook" },
            { ProductId: 2, ProductName: "Poker" },
            { ProductId: 3, ProductName: "Playtech Casino" },
            { ProductId: 4, ProductName: "Number Game" },
            { ProductId: 5, ProductName: "V8 Poker" }
        ],
        p2pProductList: [
            { ProductId: 2, ProductName: "Poker" },
            { ProductId: 5, ProductName: "V8 Poker" }
        ],
        selectedProductIds: [2, 5]
    };

    it('handleAdd - should call setSelectedGroup', () => {
        const props = Object.assign({}, defaultProps, { setSelectedGroup: jest.fn() });
        const wrapper = shallow(<PlatformFeeSetting {...props} />);

        wrapper.find(PlatformFeeItem).at(0).props().handleClick();

        expect(props.setSelectedGroup).toHaveBeenCalled();
    })

    it('handleRemove - should be called when click remove', () => {
        const wrapper = shallow(<PlatformFeeSetting {...defaultProps} />);
        wrapper.instance().handleRemove = jest.fn();
        wrapper.find(PlatformFeeItem).at(1).props().handleClick();

        expect(wrapper.instance().handleRemove).toHaveBeenCalledWith(1);
    })

    it('handleRemove - should call setSelectedGroup', () => {
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<PlatformFeeSetting {...props} />);
        wrapper.instance().handleRemove(1);

        expect(props.setSelectedGroup).toHaveBeenCalledWith({ PlatformFeeList: [{ GroupProductId: 123, DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4" }] });
    })

    it('handleSelect - should be called when change product', () => {
        const wrapper = shallow(<PlatformFeeSetting {...defaultProps} />);
        wrapper.instance().handleSelect = jest.fn();
        wrapper.find(PlatformFeeItem).at(0).props().onSelectProduct({ ProductId: 1, ProductName: "Sportsbook" });

        expect(wrapper.instance().handleSelect).toHaveBeenCalledWith(0, { ProductId: 1, ProductName: "Sportsbook" });
    })

    it('handleSelect - should call setSelectedGroup with checked', () => {
        const expectParam = {
            PlatformFeeList: [{ GroupProductId: 123, DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,5" },
                { GroupProductId: 234, DeductedPercentage: 0.12, DeductionId: 11, ProductIds: "4" }]
        };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<PlatformFeeSetting {...props} />);
        wrapper.instance().handleSelect(0, { ProductId: 5, ProductName: "V8 Poker", Checked: true });

        expect(props.setSelectedGroup).toHaveBeenCalledWith(expectParam);
    })

    it('handleSelect - should call setSelectedGroup with unchecked', () => {
        const expectParam = {
            PlatformFeeList: [{ GroupProductId: 123, DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3" },
                { GroupProductId: 234, DeductedPercentage: 0.12, DeductionId: 11, ProductIds: "4" }]
        };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<PlatformFeeSetting {...props} />);
        wrapper.instance().handleSelect(0, { ProductId: 1, ProductName: "Sportsbook", Checked: false });

        expect(props.setSelectedGroup).toHaveBeenCalledWith(expectParam);
    })

    it('handleSelect - should call setSelectedGroup with the second row is unchecked', () => {
        const expectParam = {
            PlatformFeeList: [{ GroupProductId: 123, DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4" },
                { GroupProductId: 234, DeductedPercentage: 0.12, DeductionId: 11, ProductIds: "" }]
        };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<PlatformFeeSetting {...props} />);
        wrapper.instance().handleSelect(1, { ProductId: 4, ProductName: "Number Game", Checked: false });

        expect(props.setSelectedGroup).toHaveBeenCalledWith(expectParam);
    })

    it('handleSelect - should call setSelectedGroup with PlatformFeeList is null', () => {
        const selectedGroup = Object.assign({}, defaultSelectedGroup, { PlatformFeeList: [] });
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup, selectedGroup: selectedGroup });
        const wrapper = shallow(<PlatformFeeSetting {...props} />);
        wrapper.instance().handleSelect(0, { ProductId: 1, ProductName: "Sportsbook", Checked: true });

        expect(props.setSelectedGroup).toHaveBeenCalledWith({ PlatformFeeList: [] });
    })

    it('handleChangeCommission - should be called when change commission', () => {
        const wrapper = shallow(<PlatformFeeSetting {...defaultProps} />);
        wrapper.instance().handleChangeCommission = jest.fn();
        wrapper.find(PlatformFeeItem).at(0).props().onChangeCommission(0.15);

        expect(wrapper.instance().handleChangeCommission).toHaveBeenCalledWith(0, 0.15);
    })

    it('handleChangeCommission - should call setSelectedGroup', () => {
        const expectParam = {
            PlatformFeeList: [{ GroupProductId: 123, DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3" },
                { GroupProductId: 234, DeductedPercentage: 0.12, DeductionId: 11, ProductIds: "4" }]
        };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<PlatformFeeSetting {...props} />);
        wrapper.instance().handleChangeCommission(0, 0.15);

        expect(props.setSelectedGroup).toHaveBeenCalledWith(expectParam);
    })

    it('should render correctly', () => {
        const wrapper = shallow(<PlatformFeeSetting {...defaultProps} />);
        expect(wrapper).toMatchSnapshot();
    })
})