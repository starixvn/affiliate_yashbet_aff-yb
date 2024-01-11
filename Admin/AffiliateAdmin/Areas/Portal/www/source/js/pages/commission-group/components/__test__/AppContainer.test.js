import React from 'react';
import { AppContainer } from '../AppContainer';
import { shallow } from 'enzyme';

describe('commission group - components - AppContainer', () => {
    it('render correctly', () => {
        const wrapper = shallow(<AppContainer isShowLoader={true} />);

        expect(wrapper).toMatchSnapshot();
    })
});