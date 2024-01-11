import React from 'react';
import { DomainControlPopup } from '../DomainControlPopup';
import { shallow } from 'enzyme';

describe('domain control - components - DomainControlPopup ', () => {
    const marketList = [
        {
            LanguageCode: "en-GB",
            DomainName: "English (Euro)"
        },
        {
            LanguageCode: "vi-VN",
            DomainName: "VietNamese"
        }
    ]

    const affDomainList = [
        {
            DomainId: 21,
            DomainName: "10.18.200.209:7613/B2CGo"
        },
        {
            DomainId: 34,
            DomainName: "go.test.com"
        }
    ]

    const redirectDomainList = [
        {
            DomainId: 46,
            DomainName: "smart.123.vn"
        },
        {
            DomainId: 60,
            DomainName: "12liammobile"
        }
    ]

    const emptyDomain = {
        domainId: 0,
        marketId: 'en-GB',
        affDomainId: 21,
        redirectDomainId: 46,
        isUsed: false
    };

    const selectedDomain = {
        domainId: 89,
        marketId: 'vi-VN',
        affDomainId: 34,
        redirectDomainId: 60,
        isUsed: true
    };

    const defaultProps = {
        selectedMarket: 'en-GB',
        affDomainList,
        redirectDomainList,
        marketList,
        selectedDomain
    };

    it('render correctly with insert', () => {
        const props = Object.assign({}, defaultProps, { selectedDomain: emptyDomain });
        const result = shallow(<DomainControlPopup {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with update', () => {
        const result = shallow(<DomainControlPopup {...defaultProps} />);
        result.setState({ isSuccess: false });

        expect(result).toMatchSnapshot();
    });

    it('render correctly with not show popup', () => {
        const props = Object.assign({}, defaultProps, { selectedDomain: null });
        const result = shallow(<DomainControlPopup {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('componentDidUpdate should call setState', () => {
        const props = Object.assign({}, defaultProps, { selectedDomain: emptyDomain });
        const result = shallow(<DomainControlPopup {...props} />);

        result.setState = jest.fn();

        result.instance().componentDidUpdate(defaultProps);

        expect(result.setState).toHaveBeenCalledWith({ isInsert: true });
    });

    it('_handleChange should be called when change makert', () => {
        const event = {
            target: {
                type: 'select',
                name: 'marketId',
                value: 'en-GB'
            }
        };
        const props = Object.assign({}, defaultProps, { setSelectedDomain: jest.fn() })
        const result = shallow(<DomainControlPopup {...props} />);

        result.find('Input[type="select"][name="marketId"]').simulate('change', event);

        expect(props.setSelectedDomain).toHaveBeenCalledWith({ domainId: 89, marketId: 'en-GB', affDomainId: 34, redirectDomainId: 60, isUsed: true });
    });

    it('_handleChange should be called when change aff domain', () => {
        const event = {
            target: {
                type: 'select',
                name: 'affDomainId',
                value: '12'
            }
        };
        const props = Object.assign({}, defaultProps, { setSelectedDomain: jest.fn() })
        const result = shallow(<DomainControlPopup {...props} />);

        result.find('Input[type="select"][name="affDomainId"]').simulate('change', event);

        expect(props.setSelectedDomain).toHaveBeenCalledWith({ domainId: 89, marketId: 'vi-VN', affDomainId: '12', redirectDomainId: 60, isUsed: true });
    });

    it('_handleChange should be called when change aff domain', () => {
        const event = {
            target: {
                type: 'select',
                name: 'redirectDomainId',
                value: '24'
            }
        };
        const props = Object.assign({}, defaultProps, { setSelectedDomain: jest.fn() })
        const result = shallow(<DomainControlPopup {...props} />);

        result.find('Input[type="select"][name="affDomainId"]').simulate('change', event);

        expect(props.setSelectedDomain).toHaveBeenCalledWith({ domainId: 89, marketId: 'vi-VN', affDomainId: 34, redirectDomainId: '24', isUsed: true });
    });

    it('_handleChange should be called when change aff domain', () => {
        const event = {
            target: {
                type: 'checkbox',
                name: 'isUsed',
                checked: false
            }
        };

        const props = Object.assign({}, defaultProps, { setSelectedDomain: jest.fn() })
        const result = shallow(<DomainControlPopup {...props} />);

        result.find('input[type="checkbox"][name="isUsed"]').simulate('change', event);

        expect(props.setSelectedDomain).toHaveBeenCalledWith({ domainId: 89, marketId: 'vi-VN', affDomainId: 34, redirectDomainId: 60, isUsed: false });
    });

    it('_handleSubmit - should be called toggleLoader, insertDomain and _handleShowAlert with with insert successfully', async () => {
        const result = { IsSuccessful: true, Message: "" };
        const props = Object.assign({}, defaultProps, { insertDomain: jest.fn(() => result), toggleLoader: jest.fn() });
        const wrapper = shallow(<DomainControlPopup {...props} />);
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
        const wrapper = shallow(<DomainControlPopup {...props} />);
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
        const wrapper = shallow(<DomainControlPopup {...props} />);
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
        const wrapper = shallow(<DomainControlPopup {...props} />);
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
        const wrapper = shallow(<DomainControlPopup {...props} />);
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
        const wrapper = shallow(<DomainControlPopup {...props} />);
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
        const wrapper = shallow(<DomainControlPopup {...defaultProps} />);
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
        const wrapper = shallow(<DomainControlPopup {...props} />);
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
        const wrapper = shallow(<DomainControlPopup {...props} />);
        wrapper.setState = jest.fn();
        const instance = wrapper.instance();
        instance._handleClosePopup = jest.fn();

        instance._hideMessage(false);

        expect(wrapper.setState).toHaveBeenCalledWith({ isShowMessage: false });
        expect(props.fetchData).not.toHaveBeenCalled();
    });

    it('_handleClosePopup - should call setSelectedDomain', () => {
        const props = Object.assign({}, defaultProps, { setSelectedDomain: jest.fn() })
        const wrapper = shallow(<DomainControlPopup {...props} />);

        wrapper.instance()._handleClosePopup();

        expect(props.setSelectedDomain).toHaveBeenCalledWith(null);
    });
})