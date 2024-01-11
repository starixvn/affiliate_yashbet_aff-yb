import React from 'react';
import DeductedPercentageGroup  from '../../deduction/DeductedPercentageGroup';
import { shallow } from 'enzyme';

describe('commission group - DeductedPercentageGroup ', () => {
    it('Should render correctly', () => {
        const wrapper = shallow(<DeductedPercentageGroup onChange={() => null} />);

        expect(wrapper).toMatchSnapshot();
    });
})