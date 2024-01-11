import React from 'react';
import { DomainPopup } from '../DomainPopup';
import { shallow } from 'enzyme';

describe('domain - components - DomainPopup ', () => {
    const emptyDomain = {
        domainId: 0,
        domainName: '',
        isSSL: false
    };

    const selectedDomain = {
        domainId: 123,
        domainName: 'abc.com',
        isSSL: true
    };

    const defaultProps = {
        selectedDomain,
        isAffDomain: true
    };

    it('render correctly with insert', () => {
        const props = Object.assign({}, defaultProps, { selectedDomain: emptyDomain });
        const result = shallow(<DomainPopup {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with update', () => {
        const result = shallow(<DomainPopup {...defaultProps} />);
        result.setState({ isSuccess: false });

        expect(result).toMatchSnapshot();
    });

    it('render correctly with not show popup', () => {
        const props = Object.assign({}, defaultProps, { selectedDomain: null });
        const result = shallow(<DomainPopup {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('componentDidUpdate should call setState', () => {
        const props = Object.assign({}, defaultProps, { selectedDomain: emptyDomain });
        const result = shallow(<DomainPopup {...props} />);

        result.setState = jest.fn();

        result.instance().componentDidUpdate(defaultProps);

        expect(result.setState).toHaveBeenCalledWith({ isInsert: true });
    });

    it('_handleChange should be called when change domain name', () => {
        const event = {
            target: {
                type: 'text',
                name: 'domainName',
                value: '123'
            }
        };
        const props = Object.assign({}, defaultProps, { setSelectedDomain: jest.fn() })
        const result = shallow(<DomainPopup {...props} />);

        result.find('Input[type="text"][name="domainName"]').simulate('change', event);

        expect(props.setSelectedDomain).toHaveBeenCalledWith({ "domainId": 123, "domainName": "123", "isSSL": true });
    });

    it('_handleChange should be called when change checkbox', () => {
        const event = {
            target: {
                type: 'checkbox',
                name: 'isSSL',
                checked: false
            }
        };

        const props = Object.assign({}, defaultProps, { setSelectedDomain: jest.fn() })
        const result = shallow(<DomainPopup {...props} />);

        result.find('input[type="checkbox"][name="isSSL"]').simulate('change', event);

        expect(props.setSelectedDomain).toHaveBeenCalledWith({ "domainId": 123, "domainName": "abc.com", "isSSL": false });
    });

    it('_handleSubmit - should be called toggleLoader and _handleShowAlert with domain is empty', async () => {
        const props = Object.assign({}, defaultProps, { toggleLoader: jest.fn(), selectedDomain: emptyDomain });
        const wrapper = shallow(<DomainPopup {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Please input domain name!");
    });

    it('_handleSubmit - should be called toggleLoader, insertDomain and _handleShowAlert with with insert successfully', async () => {
        const result = { IsSuccessful: true, Message: "" };
        const props = Object.assign({}, defaultProps, { insertDomain: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<DomainPopup {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(true, "Add Successfully!");
    });

    it('_handleSubmit - should be called toggleLoader, insertDomain and _handleShowAlert with with insert is failed', async () => {
        const result = { IsSuccessful: false, Message: "Adding has a error" };
        const props = Object.assign({}, defaultProps, { insertDomain: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<DomainPopup {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Adding has a error");
    });

    it('_handleSubmit - should be called toggleLoader, insertDomain and _handleShowAlert with with insert is errored', async () => {
        const props = Object.assign({}, defaultProps, { insertDomain: jest.fn(), toggleLoader: jest.fn() });
        const wrapper = shallow(<DomainPopup {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Add Domain is failed!");
    });

    it('_handleSubmit - should be called toggleLoader, updateDomain and _handleShowAlert with with update successfully', async () => {
        const result = { IsSuccessful: true, Message: "" };
        const props = Object.assign({}, defaultProps, { updateDomain: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<DomainPopup {...props} />);
        wrapper.setState({ isInsert: false });
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(true, "Update Successfully!");
    });

    it('_handleSubmit - should be called toggleLoader, updateDomain and _handleShowAlert with with update is failed', async () => {
        const result = { IsSuccessful: false, Message: "Update has a error" };
        const props = Object.assign({}, defaultProps, { updateDomain: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<DomainPopup {...props} />);
        wrapper.setState({ isInsert: false });
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Update has a error");
    });

    it('_handleSubmit - should be called toggleLoader, updateDomain and _handleShowAlert with with update is errored', async () => {
        const props = Object.assign({}, defaultProps, { updateDomain: jest.fn(), toggleLoader: jest.fn() });
        const wrapper = shallow(<DomainPopup {...props} />);
        wrapper.setState({ isInsert: false });
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Update Domain is failed!");
    });

    it('_handleShowAlert - should call setState, setTimeout and _hideMessage', async () => {
        const wrapper = shallow(<DomainPopup {...defaultProps} />);
        wrapper.setState = jest.fn((obj, fn) => fn && fn());
        window.setTimeout = jest.fn((fn, val) => fn && fn());

        const instance = wrapper.instance();
        instance._hideMessage = jest.fn();

        instance._handleShowAlert(false, 'Updated failed!');

        expect(wrapper.setState.mock.calls[0]).toEqual([{ isShowMessage: true, isSuccess: false, message: 'Updated failed!' }, expect.any(Function)]);
        expect(window.setTimeout).toHaveBeenCalledWith(expect.any(Function), 3000);
        expect(instance._hideMessage).toHaveBeenCalled();
    });

    it('_hideMessage - should call setState, fetchData and setSelectedDomain', async () => {
        const props = Object.assign({}, defaultProps, { fetchData: jest.fn(), setSelectedDomain: jest.fn() });
        const wrapper = shallow(<DomainPopup {...props} />);
        wrapper.setState = jest.fn();
        const instance = wrapper.instance();
        instance._handleClosePopup = jest.fn();

        instance._hideMessage(true);

        expect(wrapper.setState).toHaveBeenCalledWith({ isShowMessage: false });
        expect(props.fetchData).toHaveBeenCalled();
        expect(props.setSelectedDomain).toHaveBeenCalledWith(null);
    });

    it('_hideMessage - should call setState and not call fetchData', async () => {
        const props = Object.assign({}, defaultProps, { fetchData: jest.fn() });
        const wrapper = shallow(<DomainPopup {...props} />);
        wrapper.setState = jest.fn();
        const instance = wrapper.instance();
        instance._handleClosePopup = jest.fn();

        instance._hideMessage(false);

        expect(wrapper.setState).toHaveBeenCalledWith({ isShowMessage: false });
        expect(props.fetchData).not.toHaveBeenCalled();
    });

    it('_handlePressEnter - should call _handleSubmit when press enter in domain name textbox', () => {
        const event = {
            charCode: 13,
            preventDefault: jest.fn()
        };
        const wrapper = shallow(<DomainPopup {...defaultProps} />);
        wrapper.instance()._handleSubmit = jest.fn();

        wrapper.find('Input[type="text"]').simulate('keyPress', event);

        expect(event.preventDefault).toHaveBeenCalled();
        expect(wrapper.instance()._handleSubmit).toHaveBeenCalled();
    });

    it('_handlePressEnter - should not call _handleSubmit when press other keys', () => {
        const event = {
            charCode: 16,
            preventDefault: jest.fn()
        };
        const wrapper = shallow(<DomainPopup {...defaultProps} />);

        wrapper.find('Input[type="text"]').simulate('keyPress', event);

        expect(event.preventDefault).not.toHaveBeenCalled();
    });

    it('_handleClosePopup - should call setSelectedDomain', () => {
        const props = Object.assign({}, defaultProps, { setSelectedDomain: jest.fn() })
        const wrapper = shallow(<DomainPopup {...props} />);

        wrapper.instance()._handleClosePopup();

        expect(props.setSelectedDomain).toHaveBeenCalledWith(null);
    });
})