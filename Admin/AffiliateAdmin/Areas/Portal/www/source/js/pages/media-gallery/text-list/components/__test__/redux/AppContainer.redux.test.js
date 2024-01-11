import React from 'react'
import AppContainer from '../../AppContainer'
import { shallow } from 'enzyme'
import configureMockStore from 'redux-mock-store'
import { SET_SELECTED_TEXT_ID } from '../../../actions/types'

const mockStore = configureMockStore()

describe('text list - AppContainer - connect', () => {
    let wrapper, store

    beforeEach(() => {
        const initialState = {
            isShowLoader: true,
            checkedTexts: { 123: false },
            param: {},
            gridData: [{id: 123}]
        }
        store = mockStore(initialState)
        wrapper = shallow(<AppContainer store={store} />).dive()
    })

    it('should show correct value', () => {
        const { isShowLoader, param, gridData, checkedTexts } = wrapper.props();
        expect(isShowLoader).toBeTruthy()
        expect(checkedTexts).toEqual({ 123: false })
        expect(gridData).toEqual([{ id: 123 }])
        expect(param).toEqual({})
    })

    it('should call correct action', () => {
        wrapper.props().setSelectedTextId('456')

        const actions = store.getActions();
        expect(actions).toEqual([{ type: SET_SELECTED_TEXT_ID, data: '456'}])
    })
})