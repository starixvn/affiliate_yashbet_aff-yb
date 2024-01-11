jest.mock('redux');
jest.mock('react-redux');
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../../actions/index';

describe('note - Content - connect', () => {
    let mapStateToProps, mapDispatchToProps;
    const connectMock = jest.fn();

    beforeAll(() => {
        connect.mockImplementationOnce((mapStateToPropsFunc, mapDispatchToPropsFunc) => {
            mapStateToProps = mapStateToPropsFunc;
            mapDispatchToProps = mapDispatchToPropsFunc;
            return connectMock;
        });
        require('../../Content').default;
    });

    it('props should be mapped correctly', () => {
        const expectedResult = {
            pagingOption: { PageSize: 10 },
            noteList: [{ NoteId: 123 }],
            noteInfo: { NoteId: 123 },
            affId: 1271808
        };
        const state = {
            affCommonInfo: {
                affId: 1271808
            },
            note: {
                pagingOption: { PageSize: 10 },
                noteList: [{ NoteId: 123 }],
                noteInfo: { NoteId: 123 }
            },
            anotherProp: "Not included"
        };

        const result = mapStateToProps(state);

        expect(result).toEqual(expectedResult);
    })

    it('bindActionCreators should be called with correct parameter', () => {
        const mockBindAction = jest.fn();
        bindActionCreators.mockImplementationOnce(mockBindAction);
        const expectedActionCreator = {
            toggleLoaderUserPanel: actions['toggleLoaderUserPanel'],
            setShowPopup: actions['setShowPopup'],
            setNoteInfo: actions['setNoteInfo'],
            getNoteList: actions['getNoteList'],
            getNoteContentById: actions['getNoteContentById']
        }

        const dispatch = {};
        mapDispatchToProps(dispatch);

        expect(mockBindAction).toHaveBeenCalledWith(expectedActionCreator, dispatch);
    })

    it('returned function of connect should be called with correct parameter', () => {
        const Content = require('../../Content').Content;

        expect(connectMock).toHaveBeenCalledWith(Content);
    })
})