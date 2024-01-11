import React from 'react'
import AppContainer from '../../AppContainer'
import { shallow } from 'enzyme'
import configureMockStore from 'redux-mock-store'
import { SET_MESSAGE } from '../../../actions/types'

const mockStore = configureMockStore()

describe('payment request - AppContainer - connect', () => {
    let wrapper, store

    beforeEach(() => {
        const initialState = {
            isShowLoader: true,
            message: '123',
            isShowText: false,
            memberNotExist: 'ABC'
        }
        store = mockStore(initialState)
        wrapper = shallow(<AppContainer store={store} />).dive()
    })

    it('should show correct value', () => {
        const { isShowLoader, isShowText, memberNotExist, message } = wrapper.props();
        expect(isShowLoader).toBeTruthy()
        expect(message).toEqual('123')
        expect(memberNotExist).toEqual('ABC')
        expect(isShowText).toBeFalsy()
    })

    it('should call correct action', () => {
        wrapper.props().setMessage('456')

        const actions = store.getActions();
        expect(actions).toEqual([{ type: SET_MESSAGE, data: '456'}])
    })
})