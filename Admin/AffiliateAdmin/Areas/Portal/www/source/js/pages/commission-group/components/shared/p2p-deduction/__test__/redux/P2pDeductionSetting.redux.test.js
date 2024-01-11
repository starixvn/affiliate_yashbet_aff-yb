import React from 'react'
import P2pDeductionSetting from '../../P2pDeductionSetting'
import { shallow } from 'enzyme'
import configureMockStore from 'redux-mock-store'
import { SET_SELECTED_GROUP } from '../../../../../actions/types'

const mockStore = configureMockStore()

describe('commission group - P2pDeductionSetting - connect', () => {
    let wrapper, store

    beforeEach(() => {
        const initialState = {
            selectedGroup: {}
        }
        store = mockStore(initialState)
        wrapper = shallow(<P2pDeductionSetting store={store} />).dive()
    })

    it('should show correct value', () => {
        expect(wrapper.props().selectedGroup).toEqual({})
    })

    it('should call correct action', () => {
        wrapper.props().setSelectedGroup({ Abc: 123 })

        const actions = store.getActions();
        expect(actions).toEqual([{ type: SET_SELECTED_GROUP, data: { Abc: 123 }}])
    })
})