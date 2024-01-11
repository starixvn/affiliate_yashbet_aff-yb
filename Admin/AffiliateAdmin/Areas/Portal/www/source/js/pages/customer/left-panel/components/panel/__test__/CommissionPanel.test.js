import React from 'react';
import { CommissionPanel } from '../CommissionPanel';
import { shallow } from 'enzyme';
import CommissionGroupPopup from '../../../../../common/components/CommissionGroupPopup';

describe('left panel - components - panel - CommissionPanel', () => {
    const affiliateInfo = {
        AffId: 1271808,
        AffLink: "http://localhost:55690/Auth/AdminLogin?chalengeStr=esh5u7&accKey=E16156C28&custName=B2CAFF226&accId=1000000026&accOnId=8405",
        AffName: "B2CAFF226",
        AffType: 2,
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
        Status: "Approved"
    };

    const defaultProps = {
        affiliateInfo
    }

    it('render correctly with status is not closed', () => {
        const result = shallow(<CommissionPanel {...defaultProps} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with status is closed', () => {
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
            Status: "Closed"
        }
        const props = Object.assign({}, defaultProps, { affiliateInfo: affInfoClosed });
        const result = shallow(<CommissionPanel {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('toggleEditingCommissionPopup - should call setState when CommGroupName button', () => {
        const wrapper = shallow(<CommissionPanel {...defaultProps} />);
        wrapper.setState = jest.fn();

        wrapper.find('Button').simulate('click', true);

        expect(wrapper.setState).toHaveBeenCalledWith({ isChangingCommissionGroup: true });
    });

    it('toggleEditingCommissionPopup - should call setState when close CommissionGroupPopup', () => {
        const wrapper = shallow(<CommissionPanel {...defaultProps} />);
        wrapper.setState = jest.fn();

        wrapper.find(CommissionGroupPopup).props().setShowPopupCommGroup(false);

        expect(wrapper.setState).toHaveBeenCalledWith({ isChangingCommissionGroup: false });
    });
});