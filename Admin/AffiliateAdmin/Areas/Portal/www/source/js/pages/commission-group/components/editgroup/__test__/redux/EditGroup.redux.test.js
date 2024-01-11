import React from 'react'
import { shallow } from 'enzyme'
import configureMockStore from 'redux-mock-store'
import EditGroup from '../../EditGroup'
import { SET_SELECTED_GROUP } from '../../../../actions/types'

const mockStore = configureMockStore()

describe('commission group - EditGroup - connect', () => {
    let wrapper, store

    beforeEach(() => {
        const initialState = {
            selectedGroup: {}
        }

        store = mockStore(initialState)
        wrapper = shallow(<EditGroup store={store} />).dive()
    })

    it('should show correct value', () => {
        expect(wrapper.props().selectedGroup).toEqual({})
    })

    it('should call correct action', () => {
        wrapper.props().setSelectedGroup({ Abc: 123 })

        const actions = store.getActions();
        expect(actions).toEqual([{ type: SET_SELECTED_GROUP, data: { Abc: 123 } }])
    })
})