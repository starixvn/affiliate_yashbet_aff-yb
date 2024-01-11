import React from 'react';
import { CommGroupInfo } from '../CommGroupInfo';
import { shallow } from 'enzyme';

describe('commission group - CommGroupInfo', () => {
    const defaultProps = {
        selectedGroup: {
            BankingFee: { DeductedPercentage: 0.02, DeductionId: 12, ProductIds: 0 },
            Description: "A B C",
            GroupId: 0,
            GroupName: "ABC",
            PlatformFeeList: [{ DeductedPercentage: 0.17, DeductionId: 10, ProductIds: "1,3,4,5,6" }],
            TierSettingList: [{ TierId: 1, MinAmount: 0, MaxAmount: 100000000000000, MinAp: 0, MaxAp: 1000000000, Commission: 0 }]
        }
    };

    it('handleChange - should call setSelectedGroup when change Group Name', () => {
        const mockEvent = { target: { name: 'groupName', value: 'XYZ' } };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<CommGroupInfo {...props} />);

        wrapper.find('Input[id="GroupName"]').simulate('change', mockEvent)
        expect(props.setSelectedGroup).toHaveBeenCalledWith({ groupName: "XYZ" });
    });

    it('handleChange - not call setSelectedGroup when change Group Name > MAX_LENGTH', () => {
        const mockEvent = { target: { name: 'groupName', value: 'ABCDEFGHJKMNOPQRUSTXWYZABCDEFGHJKMNOPQRUSTXWYZABCDEFGHJKMNOPQRUSTXWYZABCDEFGHJKMNOPQRUSTXWYZABCDEFGHJK' } };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<CommGroupInfo {...props} />);

        wrapper.find('Input[id="GroupName"]').simulate('change', mockEvent)
        expect(props.setSelectedGroup).not.toHaveBeenCalled();
    });

    it('handleChange - should call setSelectedGroup when change Description', () => {
        const mockEvent = { target: { name: 'description', value: 'X Y Z' } };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<CommGroupInfo {...props} />);

        wrapper.find('Input[id="Description"]').simulate('change', mockEvent)
        expect(props.setSelectedGroup).toHaveBeenCalledWith({ description: "X Y Z" });
    });

    it('handleChange - not call setSelectedGroup when change Group Name > MAX_LENGTH', () => {
        const mockEvent = { target: { name: 'description', value: 'ABCDEFGHJKMNOPQRUSTXWYZABCDEFGHJKMNOPQRUSTXWYZABCDEFGHJKMNOPQRUSTXWYZABCDEFGHJKMNOPQRUSTXWYZABCDEFGHJK' } };
        const mockSetSelectedGroup = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedGroup: mockSetSelectedGroup });
        const wrapper = shallow(<CommGroupInfo {...props} />);

        wrapper.find('Input[id="Description"]').simulate('change', mockEvent)
        expect(props.setSelectedGroup).not.toHaveBeenCalled();
    });

    it('Should render correctly', () => {
        const wrapper = shallow(<CommGroupInfo {...defaultProps} />);

        expect(wrapper).toMatchSnapshot();
    });
})