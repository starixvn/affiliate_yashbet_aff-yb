import React from 'react';
import { SettingPanel } from '../SettingPanel';
import { shallow } from 'enzyme';
import CommissionGroupPopup from '../../../../../common/components/CommissionGroupPopup';

describe('left panel - components - panel - SettingPanel', () => {
    const affiliateInfo = {
        AffId: 1271808,
        AffLink: "http://localhost:55690/Auth/AdminLogin?chalengeStr=esh5u7&accKey=E16156C28&custName=B2CAFF226&accId=1000000026&accOnId=8405",
        AffName: "B2CAFF226",
        AffType: 1,
        BalanceBf: 9802280.5446,
        CommGroupName: "NoCommission",
        ContactApps: '[{"Name":"MSN/YM/QQ","Value":"@123","Order":0}]',
        FirstName: "bb",
        Gender: true,
        GroupId: 102,
        IsClosed: false,
        LastLoginDateText: "04/11/2019 22:24:00",
        LastLoginIp: "10.18.200.125",
        LastName: "B2CAFF226",
        MoreSiteUrl: "www.sportsbookie.com;www.gzyyy.com;www.123.com",
        NoSiteDescription: "",
        Phone: "abc",
        PreferredLanguage: "vi-VN",
        SiteUrl: "https://aff-yb.nexdev.net/YBUser/UserPanel/Main/#MyAccount",
        IsDisabledTransfer: true,
        TransferLimit: 100000000,
        PlayerCurrency: 'USD',
        Status: "Approved"
    };

    const affInfoClosed = {
        AffId: 1219631,
        AffLink: "http://localhost:55690/Auth/AdminLogin?chalengeStr=5omp0h&accKey=6B8465&custName=B2CAFF016&accId=1000000026&accOnId=8405",
        AffName: "B2CAFF016",
        AffType: 2,
        BalanceBf: 0,
        CommGroupName: "",
        ContactApps: '[{"Name":"MSN/YM/QQ","Value":"123123123","Order":0}]',
        FirstName: "",
        Gender: null,
        GroupId: 0,
        IsClosed: true,
        LastLoginDateText: "01/01/1900 00:00:00",
        LastLoginIp: "10.10.12.12",
        LastName: "",
        MoreSiteUrl: "",
        NoSiteDescription: "",
        Phone: "123456789",
        PreferredLanguage: "en-US",
        SiteUrl: "http://www.12345678.com",
        IsDisabledTransfer: false,
        TransferLimit: 0,
        PlayerCurrency: 'USD',
        Status: "Disapproved"
    }

    const affName = 'B2CAFF226';
    const defaultProps = {
        affiliateInfo: affInfoClosed,
        affName,
        affTypeList: [
            { AffTypeId: 1, AffTypeName: "Asian" }
        ]
    }

    it('render correctly', () => {
        const result = shallow(<SettingPanel {...defaultProps} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with isApproved is true', () => {
        window.globalVariables = { permissions: [{ FunctionName: "[TransferPlayerSetting]", IsHost: false }] }
        const props = Object.assign({}, defaultProps, { affiliateInfo })
        const result = shallow(<SettingPanel {...props} isApproved={true} />);
        result.instance().setState({ messageNotification: 'abc' })

        expect(result).toMatchSnapshot();
    });

    it('render correctly with ulimited transfer setting', () => {
        window.globalVariables = { permissions: [{ FunctionName: "[TransferPlayerSetting]", IsHost: true}]}
        const props = Object.assign({}, defaultProps, { affiliateInfo: { ...affInfoClosed, TransferLimit: 100000000000 } })
        const result = shallow(<SettingPanel {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('toggleTransferSettingPopup - should call setState', () => {
        const info = { abc: 123 };
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        const instance = wrapper.instance();
        instance.setState = jest.fn();

        instance.toggleTransferSettingPopup(info);

        expect(instance.setState).toHaveBeenCalledWith({ transferLimitSettingInfo: info });
    });

    it('handleCloseTransferSettingPopup - should call setState', async () => {
        const info = { abc: 123 };
        const props = { ...defaultProps, getTransferLimitSetting: jest.fn(() => info) }
        const wrapper = shallow(<SettingPanel {...props} />);
        const instance = wrapper.instance();
        instance.setState = jest.fn();

        await instance.handleOpenTransferSettingPopup();

        expect(instance.setState).toHaveBeenCalledWith({ transferLimitSettingInfo: info });
    });

    it('handleCloseTransferSettingPopup - should call setState', () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        const instance = wrapper.instance();
        instance.setState = jest.fn();

        instance.handleCloseTransferSettingPopup();

        expect(instance.setState).toHaveBeenCalledWith({ transferLimitSettingInfo: null });
    });

    it('toggleChangingAffType - should call setState when onBlur AffType select', () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState = jest.fn((val, fn) => fn());

        wrapper.find('select').at(0).simulate('blur', false);

        expect(wrapper.setState).toHaveBeenCalledWith({ isChangingAffType: false }, expect.any(Function));
    });

    it('toggleChangingAffType - should call setState when click AffType button', () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState({ isChangingAffType: true });
        wrapper.setState = jest.fn((val, fn) => fn());
        wrapper.instance().affTypeDropdown.current = jest.fn();
        wrapper.instance().affTypeDropdown.current.focus = jest.fn();

        wrapper.find('Button').at(0).simulate('click', true);

        expect(wrapper.setState).toHaveBeenCalledWith({ isChangingAffType: true }, expect.any(Function));
    });

    it('toggleChangingAffStatus - should call setState when onBlur AffStatus select', () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState = jest.fn((val, fn) => fn());

        wrapper.find('select').at(1).simulate('blur', false);

        expect(wrapper.setState).toHaveBeenCalledWith({ isChangingAffStatus: false }, expect.any(Function));
    });

    it('toggleChangingAffStatus - should call setState when click AffStatus button', () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState({ isChangingAffStatus: true });
        wrapper.setState = jest.fn((val, fn) => fn());
        wrapper.instance().affStatusDropdown.current = jest.fn();
        wrapper.instance().affStatusDropdown.current.focus = jest.fn();

        wrapper.find('Button').at(1).simulate('click', true);

        expect(wrapper.setState).toHaveBeenCalledWith({ isChangingAffStatus: true }, expect.any(Function));
    });

    it('toggleAssignCommissionPopup - should call setState when setShowPopupCommGroup false', () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState({ isAssignCommissionGroup: true });
        wrapper.setState = jest.fn();

        wrapper.find(CommissionGroupPopup).props().setShowPopupCommGroup(false);

        expect(wrapper.setState).toHaveBeenCalledWith({ isAssignCommissionGroup: false });
    });

    it('handleChangeStatus - should call setState', () => {
        const event = {
            target: {
                value: 'Approved'
            }
        }
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState = jest.fn();

        wrapper.find('select').at(1).simulate('change', event);

        expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: 'Are you sure you want to change status for this affiliate?', isConfirmPopup: true, isConfirmStatus: true, selectedStatus: 'Approved' });
    });

    it('_saveStatus - should call setState', () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState = jest.fn();

        wrapper.instance()._saveStatus('Approved');

        expect(wrapper.setState).toHaveBeenCalledWith({ isAssignCommissionGroup: true });
    });

    it('_saveStatus - should call updateStatus and setState', async () => {
        const data = { IsSuccessful: false, Message: 'Update status is failed!' };
        const updateStatus = jest.fn(() => data);
        const props = Object.assign({}, defaultProps, { updateStatus });
        const wrapper = shallow(<SettingPanel {...props} />);
        wrapper.setState = jest.fn();

        await wrapper.instance()._saveStatus('Pending');

        expect(props.updateStatus).toHaveBeenCalledWith({ AffId: 1219631, Status: "Pending" });
        expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: 'Update status is failed!', isConfirmPopup: false });
    });

    it('_saveStatus - should call updateStatus, getAffiliateInfo and setState', async () => {
        const data = { IsSuccessful: true, Message: '' };
        const updateStatus = jest.fn(() => data);
        const getAffiliateInfo = jest.fn();
        const props = Object.assign({}, defaultProps, { updateStatus, getAffiliateInfo });
        const wrapper = shallow(<SettingPanel {...props} />);
        wrapper.setState = jest.fn();

        await wrapper.instance()._saveStatus('Pending');

        expect(props.updateStatus).toHaveBeenCalledWith({ AffId: 1219631, Status: "Pending" });
        expect(props.getAffiliateInfo).toHaveBeenCalledWith(affName);
        expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: "The status has been updated successfully!", isConfirmPopup: false });
    });

    //it('handleOpenCloseAff - should call setState with open status', async () => {
    //    const wrapper = shallow(<SettingPanel {...defaultProps} isApproved={true}/>);
    //    wrapper.setState = jest.fn();

    //    await wrapper.find('Button').at(1).simulate('click');

    //    expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: "Are you sure you want to open this affiliate?", isConfirmPopup: true, isConfirmStatus: false, isClosed: false});
    //});

    //it('handleOpenCloseAff - should call wrapper.setState with close status', async () => {
    //    const props = Object.assign({}, defaultProps, { affiliateInfo });
    //    const wrapper = shallow(<SettingPanel {...props} isApproved={true}/>);
    //    wrapper.setState = jest.fn();

    //    await wrapper.find('Button').at(1).simulate('click');

    //    expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: "Are you sure you want to close this affiliate?", isConfirmPopup: true, isConfirmStatus: false, isClosed: true});
    //});

    //it('_openCloseAff - should call updateClosedStatus and setState', async () => {
    //    const data = { IsSuccessful: false, Message: 'Update status is failed!' };
    //    const updateClosedStatus = jest.fn(() => data);
    //    const props = Object.assign({}, defaultProps, { updateClosedStatus });
    //    const wrapper = shallow(<SettingPanel {...props} isApproved={true}/>);
    //    wrapper.setState = jest.fn();

    //    await wrapper.instance()._openCloseAff(false);

    //    expect(props.updateClosedStatus).toHaveBeenCalledWith({ AffId: 1219631, IsClosed: false });
    //    expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: 'Update status is failed!', isConfirmPopup: false });
    //});

    //it('_openCloseAff - should call updateClosedStatus, getAffiliateInfo and setState', async () => {
    //    const data = { IsSuccessful: true, Message: '' };
    //    const updateClosedStatus = jest.fn(() => data);
    //    const getAffiliateInfo = jest.fn();
    //    const props = Object.assign({}, defaultProps, { updateClosedStatus, getAffiliateInfo });
    //    const wrapper = shallow(<SettingPanel {...props} isApproved={true}/>);
    //    wrapper.setState = jest.fn();

    //    await wrapper.instance()._openCloseAff(false);

    //    expect(props.updateClosedStatus).toHaveBeenCalledWith({ AffId: 1219631, IsClosed: false });
    //    expect(props.getAffiliateInfo).toHaveBeenCalledWith(affName);
    //    expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: "The status has been updated successfully!", isConfirmPopup: false });
    //});

    it('handleChangeAffType - should call updateAffType and wrapper.setState', async () => {
        const event = {
            target: {
                value: 'Asian'
            }
        }
        const data = { IsSuccessful: false, Message: 'Update afftype is failed!' };
        const updateAffType = jest.fn(() => data);
        const props = Object.assign({}, defaultProps, { updateAffType });
        const wrapper = shallow(<SettingPanel {...props} />);
        wrapper.setState = jest.fn();

        await wrapper.find('select').at(0).simulate('change', event);

        expect(props.updateAffType).toHaveBeenCalledWith({ AffId: 1219631, AffType: 'Asian' });
        expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: "Occur an error when updating affiliate type!", isConfirmPopup: false });
    });

    it('handleChangeAffType - should call updateAffType, getAffiliateInfo and toggleChangingAffType', async () => {
        const event = {
            target: {
                value: 'Asian'
            }
        }
        const data = { IsSuccessful: true, Message: '' };
        const updateAffType = jest.fn(() => data);
        const getAffiliateInfo = jest.fn();
        const props = Object.assign({}, defaultProps, { updateAffType, getAffiliateInfo });
        const wrapper = shallow(<SettingPanel {...props} />);
        wrapper.setState = jest.fn();

        await wrapper.find('select').at(0).simulate('change', event);

        expect(props.updateAffType).toHaveBeenCalledWith({ AffId: 1219631, AffType: 'Asian' });
        expect(props.getAffiliateInfo).toHaveBeenCalledWith('B2CAFF226');
        expect(wrapper.setState).toHaveBeenCalledWith({ isChangingAffType: false }, expect.any(Function));
    });

    it('handleCloseNotificationPopup - should call setState and _saveStatus', async () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState({ selectedStatus: 'Approved', isConfirmStatus: true });
        wrapper.setState = jest.fn();
        const instance = wrapper.instance();
        instance._saveStatus = jest.fn();

        await instance.handleCloseNotificationPopup(true);

        expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: '' });
        expect(instance._saveStatus).toHaveBeenCalledWith('Approved');
    });

    it('handleCloseNotificationPopup - should call setState and _openCloseAff', async () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState({ selectedStatus: '', isConfirmStatus: false });
        wrapper.setState = jest.fn();
        const instance = wrapper.instance();
        //instance._openCloseAff = jest.fn();

        await instance.handleCloseNotificationPopup(true);

        expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: '' });
        //expect(instance._openCloseAff).toHaveBeenCalledWith(false);
    });

    it('handleCloseNotificationPopup - should call setState but not call _saveStatus and _openCloseAff', async () => {
        const wrapper = shallow(<SettingPanel {...defaultProps} />);
        wrapper.setState = jest.fn();
        const instance = wrapper.instance();
        instance._saveStatus = jest.fn();
        //instance._openCloseAff = jest.fn();

        await instance.handleCloseNotificationPopup(false);

        expect(wrapper.setState).toHaveBeenCalledWith({ messageNotification: '' });
        expect(instance._saveStatus).not.toHaveBeenCalled();
        //expect(instance._openCloseAff).not.toHaveBeenCalled();
    });
});