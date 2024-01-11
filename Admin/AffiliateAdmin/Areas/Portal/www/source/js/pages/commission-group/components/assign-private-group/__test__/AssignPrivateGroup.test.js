import React from 'react';
import { AssignPrivateGroup } from '../AssignPrivateGroup';
import { shallow } from 'enzyme';
import * as ajaxRequest from '../../../../../modules/ajaxRequest';

describe('commission group - assign private group', () => {
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

    const selectedGroup = {
        DepositFee: { DeductedPercentage: 0.02, DeductionId: 13, ProductIds: 0, IsDeductedPercent: false, DeductedAmount: "10" },
        WithdrawalFee: { DeductedPercentage: 0.02, DeductionId: 14, ProductIds: 0, IsDeductedPercent: true, DeductedAmount: "" },
        Description: "",
        GroupId: 0,
        GroupName: "",
        PlatformFeeList: [{ GroupProductId: 123, DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4" }],
        P2pDeductionList: [{ DeductedPercentage: 0.15, DeductionId: 15, ProductIds: "2,5", Commission: 0.1 }],
        TierSettingList: [{ GroupProductId: 123, TierId: 1, MinAmount: 0, MinAp: 5, Commission: 0 }]
    };

    const selectedProductIds = [2, 5];

    const defaultProps = {
        selectedGroup: { GroupId: 102 },
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
        setSelectedGroup: jest.fn(),
        selectedProductIds,
        setSelectedProductIds: jest.fn(),
        getSelectedCommGroup: jest.fn()
    };

    it('componentDidMount - should call setState and setSelectedGroup', () => {
        const wrapper = shallow(<AssignPrivateGroup {...defaultProps} />);

        wrapper.setState = jest.fn();

        wrapper.instance().componentDidMount();

        expect(wrapper.setState).toHaveBeenCalledTimes(2);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowLoader: false });
        expect(defaultProps.setSelectedGroup).toHaveBeenCalledWith(expectParam);
    })

    it('componentDidMount - should call setState and getSelectedCommGroup', () => {
        const affInfo = Object.assign({}, defaultProps.affInfo, { GroupId: 927841, IsUsingPrivateCommGroup: true })
        const props = Object.assign({}, defaultProps, { getSelectedCommGroup: jest.fn(), affInfo });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);

        wrapper.setState = jest.fn();

        wrapper.instance().componentDidMount();

        expect(wrapper.setState).toHaveBeenCalledTimes(2);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowLoader: false });
        expect(props.getSelectedCommGroup).toHaveBeenCalledWith({ GroupId: 927841 });
    })

    it('componentWillUnmount - should call setSelectedGroup', () => {
        const wrapper = shallow(<AssignPrivateGroup {...defaultProps} />);

        wrapper.instance().componentWillUnmount();

        expect(defaultProps.setSelectedGroup).toHaveBeenCalledWith(null);
    })

    it('handleSave - should call setSate with isValid is false', async () => {
        const wrapper = shallow(<AssignPrivateGroup {...defaultProps} />);
        wrapper.setState = jest.fn((obj, fn) => fn && fn());
        window.setTimeout = jest.fn((fn, time) => fn());
        window.scrollTo = jest.fn();
        wrapper.instance()._validate = jest.fn(() => ({ isValid: false, messageList: 'failed!' }));

        await wrapper.find('button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(2);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowMessage: true, isSuccess: false, message: 'failed!' });
        expect(wrapper.setState.mock.calls[0][1]).toEqual(expect.any(Function));
        expect(window.scrollTo).toHaveBeenCalledWith({ top: 0, behavior: "smooth" });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowMessage: false });
    })

    it('handleSave - should call setSate,  window.scrollTo, window.setTimeout, parent.postMessage with isValid and IsSuccessful are true', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: "OK" }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const props = Object.assign({}, defaultProps, { selectedGroup: selectedGroup });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);
        wrapper.setState = jest.fn((obj, fn) => fn && fn());
        window.setTimeout = jest.fn((fn, time) => fn());
        window.scrollTo = jest.fn();
        parent.postMessage = jest.fn();

        wrapper.instance()._validate = jest.fn(() => ({ isValid: true, messageList: 'OK' }));

        await wrapper.find('button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(3);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowMessage: true, isSuccess: true, message: ["Update Revenue Group Successfully"], isShowLoader: false });
        expect(window.scrollTo).toHaveBeenCalledWith({ top: 0, behavior: "smooth" });
        expect(window.setTimeout).toHaveBeenCalledWith(expect.any(Function), 3000);
        expect(parent.postMessage).toHaveBeenCalledWith("updatePrivateGroupSuccessfully", "http://localhost");
    })

    it('handleSave - should call setSate,  window.scrollTo with isValid, GroupId is zero', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: "OK" }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const affInfo = Object.assign({}, defaultProps.affInfo, { GroupId: 0 });
        const props = Object.assign({}, defaultProps, { selectedGroup: selectedGroup, affInfo: affInfo });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);
        wrapper.setState = jest.fn();
        window.scrollTo = jest.fn();

        wrapper.instance()._validate = jest.fn(() => ({ isValid: true, messageList: 'OK' }));

        await wrapper.find('button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(2);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowMessage: true, isSuccess: true, message: ["Assign affiliate to Revenue Group successfully"], isShowLoader: false });
        expect(window.scrollTo).toHaveBeenCalledWith({ top: 0, behavior: "smooth" })
    })

    it('handleSave - should call setSate,  window.scrollTo, window.setTimeout and not call parent.postMessage with isValid is true and IsSuccessful is false', async () => {
        const expectedResponse = { data: { IsSuccessful: false, Message: "failed!" }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const props = Object.assign({}, defaultProps, { selectedGroup: selectedGroup });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);
        wrapper.setState = jest.fn((obj, fn) => fn && fn());
        window.setTimeout = jest.fn((fn, time) => fn());
        window.scrollTo = jest.fn();
        parent.postMessage = jest.fn();

        wrapper.instance()._validate = jest.fn(() => ({ isValid: true, messageList: 'OK' }));

        await wrapper.find('button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(3);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowMessage: true, isSuccess: false, message: ["failed!"], isShowLoader: false });
        expect(window.scrollTo).toHaveBeenCalledWith({ top: 0, behavior: "smooth" });
        expect(window.setTimeout).toHaveBeenCalledWith(expect.any(Function), 3000);
        expect(parent.postMessage).not.toHaveBeenCalled();
    })

    it('handleSave - should call setSate with respone erro', async () => {
        const expectedResponse = { data: { IsSuccessful: false, Message: "failed!" }, status: 500 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        const props = Object.assign({}, defaultProps, { selectedGroup: selectedGroup });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);
        wrapper.setState = jest.fn();
        window.scrollTo = jest.fn();

        wrapper.instance()._validate = jest.fn(() => ({ isValid: true, messageList: 'OK' }));

        await wrapper.find('button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(1);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(window.scrollTo).toHaveBeenCalledWith({ top: 0, behavior: "smooth" });
    })

    it('_validate', () => {
        const props = Object.assign({}, defaultProps, { selectedGroup: selectedGroup });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);

        const result = wrapper.instance()._validate();

        expect(result).toEqual({ isValid: true, messageList: [] });
    })

    it('_validate - TierSettingList has MinAmount > MaxAmount', () => {
        const selected_Group = Object.assign({}, selectedGroup, { TierSettingList: [{ TierId: 1, MinAmount: 1000000000000, MaxAmount: 100000000000, MinAp: 0, MaxAp: 1000000000, Commission: 0 }] });
        const props = Object.assign({}, defaultProps, { selectedGroup: selected_Group });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);

        const result = wrapper.instance()._validate();

        expect(result).toEqual({ isValid: false, messageList: ["Min Net Win must be lesser than or equal to 1,000,000,000!"] });
    })
    it('_validate - TierSettingList has MinAp > MaxAp', () => {
        const selected_Group = Object.assign({}, selectedGroup, { TierSettingList: [{ TierId: 1, MinAmount: 10, MaxAmount: 100000000000, MinAp: 10000000000, MaxAp: 1000000000, Commission: 0 }] });
        const props = Object.assign({}, defaultProps, { selectedGroup: selected_Group });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);

        const result = wrapper.instance()._validate();

        expect(result).toEqual({ isValid: false, messageList: ["Min Active Player must be lesser than or equal to 100,000,000,000!"] });
    })

    it('_validate - PlatformFeeList has ProductId is null', () => {
        const selected_Group = Object.assign({}, selectedGroup, { PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "" }, { DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "" }] });
        const props = Object.assign({}, defaultProps, { selectedGroup: selected_Group });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);

        const result = wrapper.instance()._validate();

        expect(result).toEqual({ isValid: false, messageList: ["At least 1 product must be selected per group"] });
    })

    it('_validate - validateDeductedFees has IsDeductedPercent is true but empty amount', () => {
        const selected_Group = Object.assign({}, selectedGroup, { DepositFee: { DeductionId: 10, DeductionName: "Deposit Fee", DeductedPercentage: 0.17, IsDeductedPercent: false, DeductedAmount: "" } });
        const props = Object.assign({}, defaultProps, { selectedGroup: selected_Group });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);

        const result = wrapper.instance()._validate();

        expect(result).toEqual({ isValid: false, messageList: ["Deposit Fee cannot be empty. Please check!"] });
    })

    it('_validate - validateDeductedFees has IsDeductedPercent is true but invalid amount', () => {
        const selected_Group = Object.assign({}, selectedGroup, { DepositFee: { DeductionId: 10, DeductionName: "Deposit Fee", DeductedPercentage: 0.17, IsDeductedPercent: false, DeductedAmount: "1." } });
        const props = Object.assign({}, defaultProps, { selectedGroup: selected_Group });
        const wrapper = shallow(<AssignPrivateGroup {...props} />);

        const result = wrapper.instance()._validate();

        expect(result).toEqual({ isValid: false, messageList: ["Deposit Fee is invalid."] });
    })

    it('should render correctly', () => {
        const wrapper = shallow(<AssignPrivateGroup {...defaultProps} />);
        expect(wrapper).toMatchSnapshot();
    })

    it('should render correctly with isSuccess is false', () => {
        const wrapper = shallow(<AssignPrivateGroup {...defaultProps} />);
        wrapper.setState({ isSuccess: false });
        expect(wrapper).toMatchSnapshot();
    })
})