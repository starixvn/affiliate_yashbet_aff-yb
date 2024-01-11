import React from 'react';
import { TextDetailForm } from '../TextDetailForm';
import { shallow } from 'enzyme';

describe('text list - components - TextDetailForm ', () => {
    const textInfo = {
        Category: "Basketball",
        Currency: "EURO",
        DeviceType: 1,
        Language: "Japanese",
        TextContent: "anna 02",
        TextId: 2437,
        UserType: 1
    };

    const param = {
        PageIndex: 1,
        PageSize: 20,
        category: "",
        currency: "",
        deviceType: 0,
        language: "",
        userType: ''
    };

    const defaultProps = {
        selectedTextId: 0,
        param,
        categoryList: [
            { CategoryName: "Basketball", UserType: 1 },
            { CategoryName: "Bonus", UserType: 2 }
        ],
        languageList: [
            { LanguageName: "English" },
            { LanguageName: "Vietnamese" }
        ],
        currencyList: [
            { Name: "EURO" },
            { Name: "VND" }
        ],
        defaultLanguage: 'Vietnamese'
    };

    it('render correctly with insert', () => {
        const result = shallow(<TextDetailForm {...defaultProps} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with update', () => {
        const props = Object.assign({}, defaultProps, { selectedTextId: 2437, getTextDetail: jest.fn() });
        const result = shallow(<TextDetailForm {...props} />);
        result.setState({ isSuccess: false, isPlayer: false });

        expect(result).toMatchSnapshot();
    });

    it('render correctly with not show popup', () => {
        const props = Object.assign({}, defaultProps, { selectedTextId: null });
        const result = shallow(<TextDetailForm {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('componentDidUpdate should call setState', () => {
        const props = Object.assign({}, defaultProps, { selectedTextId: 2437, getTextDetail: jest.fn(() => textInfo) });
        const result = shallow(<TextDetailForm {...props} />);

        result.setState = jest.fn();

        result.instance().componentDidUpdate(defaultProps);

        expect(result.setState).toHaveBeenCalledWith({ isInsert: false });
    });

    it('componentDidUpdate should call setState and _handleGetTextDetail', () => {
        const props = Object.assign({}, defaultProps, { selectedTextId: 2437, getTextDetail: jest.fn(() => textInfo) });
        const result = shallow(<TextDetailForm {...props} />);
        result.setState = jest.fn();

        const instance = result.instance();
        instance._handleGetTextDetail = jest.fn();

        instance.componentDidUpdate(defaultProps);

        expect(result.setState).toHaveBeenCalledWith({ isInsert: false });
        expect(instance._handleGetTextDetail).toHaveBeenCalledWith(2437);
    });

    it('componentDidUpdate should not call setState and _handleGetTextDetail', () => {
        const result = shallow(<TextDetailForm {...defaultProps} />);
        result.setState = jest.fn();

        const instance = result.instance();
        instance._handleGetTextDetail = jest.fn();

        instance.componentDidUpdate(defaultProps);

        expect(result.setState).not.toHaveBeenCalled();
        expect(instance._handleGetTextDetail).not.toHaveBeenCalled();
    });

    it('_handleGetTextDetail should call getTextDetail and setState', async () => {
        const selectedTextId = 2437;
        const props = Object.assign({}, defaultProps, { getTextDetail: jest.fn(() => textInfo) });
        const result = shallow(<TextDetailForm {...props} />);

        result.setState = jest.fn();

        await result.instance()._handleGetTextDetail(selectedTextId);

        expect(props.getTextDetail).toHaveBeenCalledWith(selectedTextId, '');
        expect(result.setState).toHaveBeenCalledWith({
            category: "Basketball",
            currency: "EURO",
            deviceType: 1,
            language: "Japanese",
            textContent: "anna 02",
            isPlayer: true,
            userType: 1
        });
    });

    it('_handleChange should be called when change category select', () => {
        const event = {
            target: {
                name: 'category',
                value: 'Bonus'
            }
        };

        const result = shallow(<TextDetailForm {...defaultProps} />);

        result.setState = jest.fn();

        result.find('Input[type="select"][id="category"]').simulate('change', event);

        expect(result.setState).toHaveBeenCalledWith({ category: 'Bonus' });
    });

    it('_handleChange should be called when change userType select', () => {
        const event = {
            target: {
                name: 'userType',
                value: '1'
            }
        };

        const result = shallow(<TextDetailForm {...defaultProps} />);

        result.setState = jest.fn();

        result.find('Input[type="select"][id="userType"]').simulate('change', event);

        expect(result.setState).toHaveBeenCalledTimes(2);
        expect(result.setState.mock.calls[0][0]).toEqual({ userType: '1' });
        expect(result.setState.mock.calls[1][0]).toEqual({ isPlayer: true, category: 'Basketball', currency: 'EURO' });
    });

    it('_handleChange should be called when change userType select', () => {
        const event = {
            target: {
                name: 'userType',
                value: '2'
            }
        };

        const result = shallow(<TextDetailForm {...defaultProps} />);

        result.setState = jest.fn();

        result.find('Input[type="select"][id="userType"]').simulate('change', event);

        expect(result.setState).toHaveBeenCalledTimes(2);
        expect(result.setState.mock.calls[0][0]).toEqual({ userType: '2' });
        expect(result.setState.mock.calls[1][0]).toEqual({ isPlayer: false, category: 'Sign up', currency: 'EURO' });
    });

    it('_handleChange should be called when change currency select', () => {
        const event = {
            target: {
                name: 'currency',
                value: 'VND'
            }
        };

        const result = shallow(<TextDetailForm {...defaultProps} />);

        result.setState = jest.fn();

        result.find('Input[type="select"][id="currency"]').simulate('change', event);

        expect(result.setState).toHaveBeenCalledTimes(1);
        expect(result.setState).toHaveBeenCalledWith({ currency: 'VND' });
    });

    it('_handleChange should be called when change deviceType select', () => {
        const event = {
            target: {
                name: 'deviceType',
                value: 1
            }
        };

        const result = shallow(<TextDetailForm {...defaultProps} />);

        result.setState = jest.fn();

        result.find('Input[type="select"][id="deviceType"]').simulate('change', event);

        expect(result.setState).toHaveBeenCalledWith({ deviceType: 1 });
    });

    it('_handleSubmit - should call toggleLoader and _handleShowAlert when insert with text content empty', async () => {
        const props = Object.assign({}, defaultProps, { toggleLoader: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Please input text to upload.");
    });

    it('_handleSubmit - insertText is errored', async () => {
        const props = Object.assign({}, defaultProps, { insertText: jest.fn(), toggleLoader: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState({ textContent: 'test' });
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Add Failed!");
    });

    it('_handleSubmit - insertText is failed', async () => {
        const result = { IsSuccessful: false, Message: "Adding has a error" };
        const props = Object.assign({}, defaultProps, { insertText: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState({ textContent: 'test' });
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Adding has a error");
    });

    it('_handleSubmit - insertText is successful', async () => {
        const result = { IsSuccessful: true, Message: "" };
        const props = Object.assign({}, defaultProps, { insertText: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState({ textContent: 'test' });
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(true, "Add Successfully!");
    });

    it('_handleSubmit - should call toggleLoader and _handleShowAlert when update with text content empty', async () => {
        const props = Object.assign({}, defaultProps, { toggleLoader: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState({ isInsert: false });
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Please input text to update.");
    });

    it('_handleSubmit - updateText is errored', async () => {
        const props = Object.assign({}, defaultProps, { selectedTextId: 2437, updateText: jest.fn(), toggleLoader: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState({ textContent: 'test', isInsert: false });
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(props.updateText).toHaveBeenCalledWith({ "TextId": 2437, "category": "Basketball", "currency": "EURO", "deviceType": 0, "language": "Vietnamese", "textContent": "test", "userType": 1 });
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Update Failed!");
    });

    it('_handleSubmit - updateText is failed', async () => {
        const result = { IsSuccessful: false, Message: "Updating has a error" };
        const props = Object.assign({}, defaultProps, { selectedTextId: 2437, updateText: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState({ textContent: 'test', isInsert: false, isPlayer: false });
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(props.updateText).toHaveBeenCalledWith({ "TextId": 2437, "category": "Basketball", "currency": "", "deviceType": 0, "language": "Vietnamese", "textContent": "test", "userType": 1 });
        expect(instance._handleShowAlert).toHaveBeenCalledWith(false, "Updating has a error");
    });

    it('_handleSubmit - updateText is successful', async () => {
        const result = { IsSuccessful: true, Message: "" };
        const props = Object.assign({}, defaultProps, { selectedTextId: 2437, updateText: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();

        instance._handleShowAlert = jest.fn();
        instance.setState({ textContent: 'test', isInsert: false });
        instance.setState = jest.fn();

        await instance._handleSubmit();

        expect(props.toggleLoader).toHaveBeenCalledTimes(2);
        expect(props.toggleLoader.mock.calls[0][0]).toBeTruthy();
        expect(props.toggleLoader.mock.calls[1][0]).toBeFalsy();
        expect(props.updateText).toHaveBeenCalledWith({ "TextId": 2437, "category": "Basketball", "currency": "EURO", "deviceType": 0, "language": "Vietnamese", "textContent": "test", "userType": 1 });
        expect(instance._handleShowAlert).toHaveBeenCalledWith(true, "Update Successfully!");
    });

    it('_handleShowAlert - should call setState, setTimeout and _hideMessage', async () => {
        const wrapper = shallow(<TextDetailForm {...defaultProps} />);
        wrapper.setState = jest.fn((obj, fn) => fn && fn());
        window.setTimeout = jest.fn((fn, val) => fn && fn());

        const instance = wrapper.instance();
        instance._hideMessage = jest.fn();

        instance._handleShowAlert(false, 'Updated failed!');

        expect(wrapper.setState.mock.calls[0]).toEqual([{ isShowMessage: true, isSuccess: false, message: 'Updated failed!' }, expect.any(Function)]);
        expect(window.setTimeout).toHaveBeenCalledWith(expect.any(Function), 3000);
        expect(instance._hideMessage).toHaveBeenCalled();
    });

    it('_hideMessage - should call setState, fetchData and _handleClosePopup', async () => {
        const props = Object.assign({}, defaultProps, { fetchData: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        wrapper.setState = jest.fn();
        const instance = wrapper.instance();
        instance._handleClosePopup = jest.fn();

        instance._hideMessage(true);

        expect(wrapper.setState).toHaveBeenCalledWith({ isShowMessage: false });
        expect(props.fetchData).toHaveBeenCalledWith(param);
        expect(instance._handleClosePopup).toHaveBeenCalled();
    });

    it('_hideMessage - should call setState and not call _handleFetchData', async () => {
        const props = Object.assign({}, defaultProps, { fetchData: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        wrapper.setState = jest.fn();
        const instance = wrapper.instance();
        instance._handleClosePopup = jest.fn();

        instance._hideMessage(false);

        expect(wrapper.setState).toHaveBeenCalledWith({ isShowMessage: false });
        expect(props.fetchData).not.toHaveBeenCalled();
        expect(instance._handleClosePopup).not.toHaveBeenCalled();
    });

    it('_handleClosePopup - should call setselectedTextId and setState', async () => {
        const props = Object.assign({}, defaultProps, { setSelectedTextId: jest.fn() });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();
        instance.setState = jest.fn();

        instance._handleClosePopup();

        expect(props.setSelectedTextId).toHaveBeenCalledWith(null);
        expect(instance.setState).toHaveBeenCalledWith({
            textContent: '',
            category: 'Basketball',
            language: 'Vietnamese',
            currency: 'EURO',
            deviceType: 0,
            isInsert: true,
            isShowMessage: false,
            isSuccess: true,
            message: '',
            isPlayer: true,
            userType: 1
        });
    });

    it('_handleClosePopup - not has defaultLanguage - should call setselectedTextId and setState', async () => {
        const props = Object.assign({}, defaultProps, { setSelectedTextId: jest.fn(), defaultLanguage: '' });
        const wrapper = shallow(<TextDetailForm {...props} />);
        const instance = wrapper.instance();
        instance.setState = jest.fn();

        instance._handleClosePopup();

        expect(props.setSelectedTextId).toHaveBeenCalledWith(null);
        expect(instance.setState).toHaveBeenCalledWith({
            textContent: '',
            category: 'Basketball',
            language: 'English',
            currency: 'EURO',
            deviceType: 0,
            isInsert: true,
            isShowMessage: false,
            isSuccess: true,
            message: '',
            isPlayer: true,
            userType: 1
        });
    });
})