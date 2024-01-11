import React from 'react';
import AssignGroup from '../AssignGroup';
import * as ajaxRequest from '../../../../../modules/ajaxRequest';
import { shallow } from 'enzyme';

describe('commission group - AssignGroup', () => {
    beforeEach(() => {
        window.initialState = {
            commGroupList: [
                { Id: "133", Name: "NewGroup" },
                { Id: "152", Name: "CG22124" }
            ],
            selectedGroup: 102,
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
            }
        }
    });

    it('handleChange - should call setState when change commGroup', () => {
        const mockedEvent = { target: { name: "commGroup", value: 133 } };
        const wrapper = shallow(<AssignGroup />);
        wrapper.setState = jest.fn();
        wrapper.find('select[name="SelectedGroup"]').simulate('change', mockedEvent);

        expect(wrapper.setState).toHaveBeenCalledWith({ selectedGroup: mockedEvent.target.value });
    });

    it('handleCheck - should call setState when check private group with selectedGroup is 0', () => {
        window.initialState = Object.assign({}, window.initialState, { selectedGroup: 0 });
        const mockedEvent = { target: { name: "privateGroup", checked: true } };
        const wrapper = shallow(<AssignGroup />);
        wrapper.setState = jest.fn();
        wrapper.find('Input[id="isPrivateGroup"]').simulate('change', mockedEvent);

        expect(wrapper.setState).toHaveBeenCalledWith({ isPrivateGroup: mockedEvent.target.checked });
    });

    it('handleCheck - should call setState when check private group with selectedGroup is 0 and commGroupList is null', () => {
        window.initialState = Object.assign({}, window.initialState, { selectedGroup: 0, commGroupList: [] });
        const mockedEvent = { target: { name: "privateGroup", checked: false } };
        const wrapper = shallow(<AssignGroup />);
        wrapper.setState = jest.fn();
        wrapper.find('Input[id="isPrivateGroup"]').simulate('change', mockedEvent);

        expect(wrapper.setState).toHaveBeenCalledWith({ isPrivateGroup: mockedEvent.target.checked });
    });

    it('handleSave - should call parent.postMessage when check private group', () => {
        const wrapper = shallow(<AssignGroup />);
        wrapper.setState({ isPrivateGroup: true });
        parent.postMessage = jest.fn();

        wrapper.find('Button').simulate('click');

        expect(parent.postMessage).toHaveBeenCalledWith("changeAssignToPrivateGroup", "http://localhost");
    });

    it('handleSave - should call setState and hideMessage when GroupId same selectedGroup', () => {
        const wrapper = shallow(<AssignGroup />);
        wrapper.setState = jest.fn((obj, fn) => fn && fn());
        window.setTimeout = jest.fn((fn, time) => fn());
        wrapper.find('Button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(2);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({
            isShowMessage: true,
            isSuccess: false,
            message: "Please choose another group (not old group)!"
        });
        expect(wrapper.setState.mock.calls[0][1]).toEqual(expect.any(Function));

        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowMessage: false });
    });

    it('handleSave - should call setState, hideMessage and parent.postMessage with IsSuccessful is true', async () => {
        const expectedResponse = { data: { IsSuccessful: true, Message: "OK" }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        window.initialState = Object.assign({}, window.initialState, { selectedGroup: 133 });
        const wrapper = shallow(<AssignGroup />);
        wrapper.setState = jest.fn((obj, fn) => fn && fn());
        parent.postMessage = jest.fn();
        window.setTimeout = jest.fn((fn, time) => fn());
        global.confirm = jest.fn(() => true);

        await wrapper.find('Button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(4);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({
            isShowMessage: true,
            isSuccess: true,
            message: 'Assign affiliate to Revenue Group successfully'
        });
        expect(wrapper.setState.mock.calls[2][0]).toEqual({ isShowMessage: false });
        expect(wrapper.setState.mock.calls[3][0]).toEqual({ isShowLoader: false });
        expect(window.setTimeout).toHaveBeenCalledWith(expect.any(Function), 3000);
        expect(parent.postMessage).toHaveBeenCalledWith("assignGroupSuccessfully", "http://localhost");
    });

    it('handleSave - should call setState, hideMessage and not call parent.postMessage with IsSuccessful is false', async () => {
        const expectedResponse = { data: { IsSuccessful: false, Message: "failed!" }, status: 200 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        window.initialState = Object.assign({}, window.initialState, { selectedGroup: 133 });
        const wrapper = shallow(<AssignGroup />);
        wrapper.setState = jest.fn((obj, fn) => fn && fn());
        parent.postMessage = jest.fn();
        window.setTimeout = jest.fn((fn, time) => fn());
        global.confirm = jest.fn(() => true);

        await wrapper.find('Button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(4);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowMessage: true, isSuccess: false, message: "failed!" });
        expect(wrapper.setState.mock.calls[2][0]).toEqual({ isShowMessage: false });
        expect(wrapper.setState.mock.calls[3][0]).toEqual({ isShowLoader: false });
        expect(window.setTimeout).toHaveBeenCalledWith(expect.any(Function), 3000);
        expect(parent.postMessage).not.toHaveBeenCalled();
    });

    it('handleSave - should call setState but not call window.alert with reponse incorrect', async () => {
        const expectedResponse = { data: { IsSuccessful: false, Message: "failed!" }, status: 500 };
        ajaxRequest.post = jest.fn(() => Promise.resolve(expectedResponse));

        window.initialState = Object.assign({}, window.initialState, { selectedGroup: 133 });
        const wrapper = shallow(<AssignGroup />);
        wrapper.setState = jest.fn();
        window.alert = jest.fn();

        await wrapper.find('Button').simulate('click');

        expect(wrapper.setState).toHaveBeenCalledTimes(2);
        expect(wrapper.setState.mock.calls[0][0]).toEqual({ isShowLoader: true });
        expect(wrapper.setState.mock.calls[1][0]).toEqual({ isShowLoader: false });
        expect(window.alert).not.toHaveBeenCalled();
    });

    it('Should render correctly', () => {
        const wrapper = new AssignGroup();

        expect(wrapper).toMatchSnapshot();
    });

    it('Should render correctly with isSuccess state is false', () => {
        const wrapper = shallow(<AssignGroup />);

        wrapper.instance().setState({ isSuccess: false });

        expect(wrapper).toMatchSnapshot();
    });
});