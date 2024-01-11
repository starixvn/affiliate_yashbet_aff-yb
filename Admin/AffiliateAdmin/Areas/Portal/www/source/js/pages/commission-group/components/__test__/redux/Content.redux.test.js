import React from 'react'
import Content from '../../Content'
import { shallow } from 'enzyme'
import configureMockStore from 'redux-mock-store'

const mockStore = configureMockStore()

describe('aff report - Content - connect', () => {
    let wrapper
    let store

    beforeEach(() => {
        const initialState = {
            searchedGroupName: ''
        }
        store = mockStore(initialState)
        wrapper = shallow(<Content store={store} />).dive()
    })

    it('should show correct value', () => {
        expect(wrapper.props().searchedGroupName).toEqual('')
    })
})