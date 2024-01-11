import React from 'react';
import { Content } from '../Content';
import { shallow } from 'enzyme';
import GridData from '../grouplist/GridData';
import AddGroup from '../addgroup';
import EditGroup from '../editgroup';

describe('commission group - Content', () => {
    const gridData = {
        BankingFee: {
            DeductedPercentage: 0.02,
            DeductionId: 12,
            DeductionName: "Banking Fee",
            GroupId: 133,
            ProductIds: "0"
        },
        CreatedDate: "2018-08-13T04:57:37.803",
        CreatedDateText: "08/13/2018 04:57:37",
        Creator: "pgmPercy",
        Description: "testing",
        GroupId: 133,
        GroupName: "NewGroup",
        NoAffs: 8,
        PlatformFeeList: [{
            DeductedPercentage: 0.17,
            DeductionId: 10,
            DeductionName: "Platform Fee",
            GroupId: 133,
            ProductIds: "1,3,4,5,6,7,8,13,14,15,16,17,18"
        }],
        RowIdx: 1,
        TierSettingList: [{
            Commission: 0,
            GroupId: 133,
            MaxAmount: 100000000000000,
            MaxAp: 1000000000,
            MinAmount: 0,
            MinAp: 0,
            TierId: 1
        }]
    }
    const defaultProps = {
        pagingOption: {},
        searchedGroupName: '',
        gridData
    }

    it('should call handleAddNewGroup when click button Add New Group', () => {
        const wrapper = shallow(<Content {...defaultProps} />);
        const instance = wrapper.instance();
        instance.setState = jest.fn();
        instance.handleHide = jest.fn();
        instance.handleSave = jest.fn();

        const expectedState = {
            Popup: <AddGroup onClose={instance.handleHide} onSave={instance.handleSave} />,
            popupTitle: "Add a Revenue Group"
        }
        wrapper.find('Button').simulate('click');

        expect(instance.setState).toHaveBeenCalledWith(expectedState);
    });

    it('should call handleEditGroup when click onEdit in GridData', () => {
        const wrapper = shallow(<Content {...defaultProps} />);
        const instance = wrapper.instance();
        instance.setState = jest.fn();
        instance.handleHide = jest.fn();
        instance.handleSave = jest.fn();

        const expectedState = {
            Popup: <EditGroup onClose={instance.handleHide} itemId={123} onSave={instance.handleSave} />,
            popupTitle: "Edit the Revenue Group"
        }

        wrapper.find(GridData).props().onEdit(123);
        expect(instance.setState).toHaveBeenCalledWith(expectedState);
    });

    it('handleHide - should call setState', () => {
        const wrapper = shallow(<Content {...defaultProps} />);
        const instance = wrapper.instance();
        instance.setState = jest.fn();

        instance.handleHide();

        expect(instance.setState).toHaveBeenCalledWith({ Popup: null });
    });

    it('handleSave - should call setState', () => {
        const wrapper = shallow(<Content {...defaultProps} />);
        const instance = wrapper.instance();
        instance.modalRef.current = jest.fn();
        instance.modalRef.current.scrollTo = jest.fn();

        instance.handleSave();

        expect(instance.modalRef.current.scrollTo).toHaveBeenCalledWith({ top: 0, behavior: "smooth" });
    });

    it('onPaging - should call getGroupSettingList', () => {
        const mockGetGroupSettingList = jest.fn();

        const props = Object.assign({}, defaultProps, { getGroupSettingList: mockGetGroupSettingList, searchedGroupName: 'B2C' });
        const componentInstance = new Content(props);

        componentInstance.onPaging(1, 20);

        expect(props.getGroupSettingList).toHaveBeenCalledWith({ GroupName: "B2C", PageIndex: 1, PageSize: 20 });
    });

    it('should render correctly', () => {
        const wrapper = shallow(<Content {...defaultProps} />);

        expect(wrapper).toMatchSnapshot();
    })
});