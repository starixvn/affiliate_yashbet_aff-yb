import React from 'react';
import { Unactivated } from '../Unactivated';
import { shallow, mount } from 'enzyme';

describe('active member - components - Unactivated ', () => {
    const unactivatedGridData = [
        {
            FirstName: "",
            LastName: "",
            UserName: "xyl157"
        }
    ];

    const defaultProps = {
        shouldResetScrollPosition: true,
        userNames: 'xyl157',
        unactivatedGridData,
    };

    it('render correctly', () => {
        const result = shallow(<Unactivated {...defaultProps} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with unactivatedGridData is null', () => {
        const props = Object.assign({}, defaultProps, { unactivatedGridData: null })
        const result = shallow(<Unactivated {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with unactivatedGridData is empty', () => {
        const props = Object.assign({}, defaultProps, { unactivatedGridData: [] })
        const result = shallow(<Unactivated {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('_handleActive - should be called when click Active button', async () => {
        const props = Object.assign({}, defaultProps, { activeMember: jest.fn(), setShouldResetScrollPosition: jest.fn(), setMessage: jest.fn() });

        const wrapper = mount(<Unactivated {...props} />);

        await wrapper.find('Button').at(1).simulate('click');

        expect(props.activeMember).toHaveBeenCalledWith({ userNames: 'xyl157' });
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
        expect(props.setMessage).toHaveBeenCalledWith("Activate players is failed!");
    });

    it('_handleActive - should be called when click Active All button', async () => {
        const props = Object.assign({}, defaultProps, { activeMember: jest.fn(), setShouldResetScrollPosition: jest.fn(), setMessage: jest.fn() });

        const wrapper = mount(<Unactivated {...props} />);

        await wrapper.find('Button').at(0).simulate('click');

        expect(props.activeMember).toHaveBeenCalledWith({ userNames: 'xyl157' });
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
        expect(props.setMessage).toHaveBeenCalledWith("Activate players is failed!");
    });

    it('_handleActive - should be called when click Active All button is successful', async () => {
        const expectResult = { IsSuccessful: true };
        const props = Object.assign({}, defaultProps, { activeMember: jest.fn(() => expectResult), setShouldResetScrollPosition: jest.fn(), setMessage: jest.fn() });

        const wrapper = mount(<Unactivated {...props} />);

        await wrapper.find('Button').at(0).simulate('click');

        expect(props.activeMember).toHaveBeenCalledWith({ userNames: 'xyl157' });
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
        expect(props.setMessage).toHaveBeenCalledWith("Players has been activated successfully!");
    });

    it('_handleActive - should be called when click Active All button is failed', async () => {
        const expectResult = { IsSuccessful: false, Message: 'Failed' };
        const props = Object.assign({}, defaultProps, { activeMember: jest.fn(() => expectResult), setShouldResetScrollPosition: jest.fn(), setMessage: jest.fn() });

        const wrapper = mount(<Unactivated {...props} />);

        await wrapper.find('Button').at(0).simulate('click');

        expect(props.activeMember).toHaveBeenCalledWith({ userNames: 'xyl157' });
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
        expect(props.setMessage).toHaveBeenCalledWith("Failed");
    });
})