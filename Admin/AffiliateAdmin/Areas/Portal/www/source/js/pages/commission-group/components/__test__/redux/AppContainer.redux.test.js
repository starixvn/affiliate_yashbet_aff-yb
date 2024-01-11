import React from 'react'
import { shallow } from 'enzyme'
import configureMockStore from 'redux-mock-store'
import AppContainer from '../../AppContainer'

const mockStore = configureMockStore()

describe('commission group - AddGroup - connect', () => {
    let wrapper, store

    beforeEach(() => {
        const initialState = {
            isShowLoader: false
        }

        store = mockStore(initialState)
        wrapper = shallow(<AppContainer store={store} />).dive()
    })

    it('should show correct value', () => {
        expect(wrapper.props().isShowLoader).toBeFalsy()
    })
})