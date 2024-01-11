import React from 'react';
import { ControlButtons } from '../ControlButtons';
import { shallow } from 'enzyme';

import ButtonDeleteItems from '../../../../common/components/ButtonDeleteItems';

describe('message - components - ControlButtons ', () => {
    const gridData = [{ Id: 123 }];

    const param = {
        AffType: 2
    };

    const defaultProps = {
        checkedMessages: { 123: false },
        param,
        gridData
    };

    it('render correctly', () => {
        const result = shallow(<ControlButtons {...defaultProps} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with gridData is null', () => {
        const props = Object.assign({}, defaultProps, { gridData: null })
        const result = shallow(<ControlButtons {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with gridData is empty', () => {
        const props = Object.assign({}, defaultProps, { gridData: [] })
        const result = shallow(<ControlButtons {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('fetchData should be call when click button Delete', () => {
        const fetchData = jest.fn();
        const props = Object.assign({}, defaultProps, { fetchData })
        const result = shallow(<ControlButtons {...props} />);

        result.find(ButtonDeleteItems).props().fetchData();

        expect(props.fetchData).toHaveBeenCalledWith(param);
    });

    it('_handleAddItem should be call when click button Add', () => {
        const setSelectedItemId = jest.fn();
        const setShouldResetScrollPosition = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedItemId, setShouldResetScrollPosition })
        const result = shallow(<ControlButtons {...props} />);

        result.find('Button').at(0).simulate('click', 0);

        expect(props.setSelectedItemId).toHaveBeenCalledWith(0);
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
    });

    it('_handleClickButton should be call when click button Add with gridData is empty', () => {
        const setSelectedItemId = jest.fn();
        const setShouldResetScrollPosition = jest.fn();
        const props = Object.assign({}, defaultProps, { gridData: [], setSelectedItemId, setShouldResetScrollPosition })
        const result = shallow(<ControlButtons {...props} />);

        result.find('Button').simulate('click');

        expect(props.setSelectedItemId).toHaveBeenCalledWith(0);
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
    });
})