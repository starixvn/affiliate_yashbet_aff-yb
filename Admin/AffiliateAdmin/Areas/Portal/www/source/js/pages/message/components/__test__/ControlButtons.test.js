import React from 'react';
import { ControlButtons } from '../ControlButtons';
import { shallow } from 'enzyme';

import ButtonDeleteItems from '../../../common/components/ButtonDeleteItems';

describe('message - components - ControlButtons ', () => {
    const gridData = [
        {
            Creator: "sa",
            Important: true,
            Message: "test",
            MessageId: 37,
            PostedDate: "2019-09-13T02:12:43.987",
            PostedDateText: "09/13/2019 02:12:43",
            UpdatedBy: "sa",
            UpdatedDate: "2019-09-16T02:38:23.327",
            UpdatedDateText: "09/16/2019 02:38:23"
        },
        {
            Creator: "pgmDung",
            Important: false,
            Message: "aaajj",
            MessageId: 6,
            PostedDate: "2013-02-18T05:58:53.23",
            PostedDateText: "02/18/2013 05:58:53",
            UpdatedBy: "pgmDung",
            UpdatedDate: "2013-02-18T22:39:10.73",
            UpdatedDateText: "02/18/2013 22:39:10"
        }
    ];

    const param = {
        FromDate: "9/20/2019",
        Important: null,
        IsGetAll: true,
        PageIndex: 1,
        PageSize: 20,
        ToDate: "9/20/2019"
    };

    const defaultProps = {
        checkedMessages: { 37: false, 6: false },
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
        const setShouldResetScrollPosition = jest.fn();
        const props = Object.assign({}, defaultProps, { fetchData })
        const result = shallow(<ControlButtons {...props} />);

        result.find(ButtonDeleteItems).props().fetchData();

        expect(props.fetchData).toHaveBeenCalledWith(param);
    });

    it('_handleClickButton should be call when click button Add', () => {
        const setSelectedMessageId = jest.fn();
        const setShouldResetScrollPosition = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedMessageId, setShouldResetScrollPosition })
        const result = shallow(<ControlButtons {...props} />);

        result.find('Button').at(0).simulate('click', 0);

        expect(props.setSelectedMessageId).toHaveBeenCalledWith(0);
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
    });

    it('_handleClickButton should be call when click button CurrentMessage', () => {
        const setSelectedMessageId = jest.fn();
        const setShouldResetScrollPosition = jest.fn();
        const props = Object.assign({}, defaultProps, { setSelectedMessageId, setShouldResetScrollPosition })
        const result = shallow(<ControlButtons {...props} />);

        result.find('Button').at(1).simulate('click');

        expect(props.setSelectedMessageId).toHaveBeenCalledWith(-1);
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
    });

    it('_handleClickButton should be call when click button Add with gridData is empty', () => {
        const setSelectedMessageId = jest.fn();
        const setShouldResetScrollPosition = jest.fn();
        const props = Object.assign({}, defaultProps, { gridData: [], setSelectedMessageId, setShouldResetScrollPosition })
        const result = shallow(<ControlButtons {...props} />);

        result.find('Button').simulate('click');

        expect(props.setSelectedMessageId).toHaveBeenCalledWith(0);
        expect(props.setShouldResetScrollPosition).toHaveBeenCalledWith(false);
    });
})