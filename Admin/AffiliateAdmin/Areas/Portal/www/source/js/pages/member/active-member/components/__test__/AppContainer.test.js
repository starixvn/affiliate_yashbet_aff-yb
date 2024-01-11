import React from 'react';
import { AppContainer } from '../AppContainer';
import { shallow } from 'enzyme';
import NotificationPopup from '../../../../common/components/NotificationPopup';

describe('active member - components - AppContainer', () => {
    const defaultProps = {
        isShowLoader: true,
        isShowText: true,
        userNames: '123abc',
        message: 'Please input Deposit User Name!'
    }
    it('render correctly', () => {
        const result = shallow(<AppContainer {...defaultProps} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with memberNotExist', () => {
        const props = Object.assign({}, defaultProps, { memberNotExist: 'These members do not exist: abc' });
        const result = shallow(<AppContainer {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('setMessage - should be called when close NotificationPopup', () => {
        const props = Object.assign({}, defaultProps, { setMessage: jest.fn() });
        const result = shallow(<AppContainer {...props} />);

        result.find(NotificationPopup).props().onClose();

        expect(props.setMessage).toHaveBeenCalledWith('');
    });
});