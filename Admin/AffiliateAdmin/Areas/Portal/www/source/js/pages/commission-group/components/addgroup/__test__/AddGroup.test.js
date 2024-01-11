import React from 'react';
import { AddGroup } from '../AddGroup';
import { shallow } from 'enzyme';
import { defaultPagingOption } from '../../../../../modules/constants';

describe('commission group - AddGroup', () => {
    const expectParam = {
        DepositFee: { DeductedPercentage: 0.02, DeductionId: 13, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
        WithdrawalFee: { DeductedPercentage: 0.02, DeductionId: 14, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
        Description: "",
        GroupId: 0,
        GroupName: "",
        SubAffCommRate: 0.1,
        IsActivePlayerSum: true,
        PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4", IsDeductedPercent: true, DeductedAmount: "" }],
        P2pDeductionList: [{ DeductedPercentage: 0.02, DeductionId: 15, ProductIds: "2,5", Commission: 0.01 }],
        TierSettingList: []
    };
    const defaultProps = {
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
        selectedGroup: {
            DepositFee: { DeductedPercentage: 0.02, DeductionId: 13, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
            WithdrawalFee: { DeductedPercentage: 0.02, DeductionId: 14, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
            Description: "",
            GroupId: 0,
            GroupName: "",
            PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4", IsDeductedPercent: true, DeductedAmount: "" }],
            P2pDeductionList: [{ DeductedPercentage: 0.15, DeductionId: 15, ProductIds: "2,5", Commission: 0.1 }],
            TierSettingList: []
        },
        pagingOption: defaultPagingOption,
        searchedGroupName: '',
        onClose: jest.fn(), onSave: jest.fn(),
        setSelectedProductIds: jest.fn()
    };
    it('componentDidMount - should call setSelectedGroup', () => {
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });

        const componentInstance = new AddGroup(props);

        componentInstance.componentDidMount();

        expect(props.setSelectedGroup).toHaveBeenCalledWith(expectParam);

    });

    it('componentDidMount - should call setSelectedGroup with ', () => {
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });

        const componentInstance = new AddGroup(props);

        componentInstance.componentDidMount();

        expect(props.setSelectedGroup).toHaveBeenCalledWith(expectParam);

    });

    it('componentWillUnmount - should call setSelectedGroup', () => {
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });

        const componentInstance = new AddGroup(props);

        componentInstance.componentWillUnmount();

        expect(props.setSelectedGroup).toHaveBeenCalledWith(null);

    });

    it('handleSave - should call setState when click button SAVE', () => {
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const expectData = {
            isShowMessage: true,
            isSuccess: false,
            message: ["Group Name cannot be empty. Please check!", "Tier Setting must have at least 1 tier"]
        };
        window.scrollTo = jest.fn();
        const wrapper = shallow(<AddGroup {...props} />);
        wrapper.instance().setState = jest.fn();

        wrapper.find('button').simulate('click');

        expect(wrapper.instance().setState).toHaveBeenCalledWith(expectData);
    });

    it('handleSave - should call toggleLoader,  when click button SAVE with correct respone', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: "OK" }, status: 200 };
        const selectedGroupValid = {
            DepositFee: { DeductedPercentage: 0.02, DeductionId: 13, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
            WithdrawalFee: { DeductedPercentage: 0.02, DeductionId: 14, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
            Description: "test",
            GroupId: 0,
            GroupName: "test",
            PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4,5,6,8", IsDeductedPercent: true, DeductedAmount: "" }],
            P2pDeductionList: [{ DeductedPercentage: 0.15, DeductionId: 15, ProductIds: "2,5", Commission: 0.1 }],
            TierSettingList: [{ TierId: 1, MinAmount: 0, MaxAmount: 100000000000, MinAp: 0, MaxAp: 1000000000, Commission: 0 }]
        }
        const mockSetSelectedGroup = jest.fn();
        const mockToggleLoader = jest.fn();
        const mockInsertCommissionGroup = jest.fn(() => expectedResponse);
        const mockGetGroupSettingList = jest.fn();

        const props = Object.assign({}, defaultProps, {
            setSelectedGroup: mockSetSelectedGroup, insertCommissionGroup: mockInsertCommissionGroup,
            selectedGroup: selectedGroupValid, toggleLoader: mockToggleLoader, getGroupSettingList: mockGetGroupSettingList
        });
        window.setTimeout = jest.fn();
        window.scrollTo = jest.fn();
        const wrapper = shallow(<AddGroup {...props} />);
        wrapper.instance().setState = jest.fn();

        await wrapper.find('button').simulate('click');

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(window.setTimeout).toHaveBeenCalledTimes(1);
        expect(props.getGroupSettingList).toHaveBeenCalledWith({ PageIndex: 1, PageSize: 20, GroupName: '' });
        expect(wrapper.instance().setState).toHaveBeenCalledWith({ isShowMessage: true, isSuccess: true, message: ["Add Revenue Group Successfully"] });
    });

    it('handleSave - should call toggleLoader,  when click button SAVE with correct respone but not success', async () => {
        const expectedResponse = { data: { IsSuccessful: false, Message: "fail" }, status: 200 };
        const selectedGroupValid = {
            DepositFee: { DeductedPercentage: 0.02, DeductionId: 13, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
            WithdrawalFee: { DeductedPercentage: 0.02, DeductionId: 14, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
            Description: "test",
            GroupId: 0,
            GroupName: "test",
            PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4,5,6,8", IsDeductedPercent: true, DeductedAmount: "" }],
            P2pDeductionList: [{ DeductedPercentage: 0.15, DeductionId: 15, ProductIds: "2,5", Commission: 0.1 }],
            TierSettingList: [{ TierId: 1, MinAmount: 0, MaxAmount: 100000000000, MinAp: 0, MaxAp: 1000000000, Commission: 0 }]
        }
        const mockSetSelectedGroup = jest.fn();
        const mockToggleLoader = jest.fn();
        const mockInsertCommissionGroup = jest.fn(() => expectedResponse);
        const mockGetGroupSettingList = jest.fn();

        const props = Object.assign({}, defaultProps, {
            setSelectedGroup: mockSetSelectedGroup, insertCommissionGroup: mockInsertCommissionGroup,
            selectedGroup: selectedGroupValid, toggleLoader: mockToggleLoader, getGroupSettingList: mockGetGroupSettingList
        });
        window.setTimeout = jest.fn();
        window.scrollTo = jest.fn();
        const wrapper = shallow(<AddGroup {...props} />);
        wrapper.instance().setState = jest.fn();

        await wrapper.find('button').simulate('click');

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(window.setTimeout).not.toHaveBeenCalled();
        expect(props.getGroupSettingList).not.toHaveBeenCalled();
        expect(wrapper.instance().setState).toHaveBeenCalledWith({ isShowMessage: true, isSuccess: false, message: ["fail"] });
    });

    it('handleSave - should call toggleLoader,  when click button SAVE with incorrect response', async () => {
        const expectedResponse = { status: 500 };

        const selectedGroupValid = {
            DepositFee: { DeductedPercentage: 0.02, DeductionId: 13, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
            WithdrawalFee: { DeductedPercentage: 0.02, DeductionId: 14, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
            Description: "test",
            GroupId: 0,
            GroupName: "test",
            PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4,5,6,8", IsDeductedPercent: true, DeductedAmount: "" }],
            P2pDeductionList: [{ DeductedPercentage: 0.15, DeductionId: 15, ProductIds: "2,5", Commission: 0.1 }],
            TierSettingList: [{ TierId: 1, MinAmount: 0, MaxAmount: 100000000000, MinAp: 0, MaxAp: 1000000000, Commission: 0 }]
        }
        const mockSetSelectedGroup = jest.fn();
        const mockToggleLoader = jest.fn();
        const mockInsertCommissionGroup = jest.fn(() => expectedResponse);
        const mockGetGroupSettingList = jest.fn();

        const props = Object.assign({}, defaultProps, {
            setSelectedGroup: mockSetSelectedGroup, insertCommissionGroup: mockInsertCommissionGroup,
            selectedGroup: selectedGroupValid, toggleLoader: mockToggleLoader, getGroupSettingList: mockGetGroupSettingList
        });
        window.setTimeout = jest.fn();
        window.scrollTo = jest.fn();
        const wrapper = shallow(<AddGroup {...props} />);
        wrapper.instance().setState = jest.fn();

        await wrapper.find('button').simulate('click');

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(window.setTimeout).not.toHaveBeenCalled();
        expect(props.getGroupSettingList).not.toHaveBeenCalled();
        expect(wrapper.instance().setState).not.toHaveBeenCalled();
    });

    it('should render correctly with isSuccess is true', () => {
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<AddGroup {...props} />);

        expect(wrapper).toMatchSnapshot();
    });

    it('should render correctly with isSuccess is false', () => {
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<AddGroup {...props} />);
        wrapper.setState({ isSuccess: false });

        expect(wrapper).toMatchSnapshot();
    });
});