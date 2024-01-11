import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table, Row, Cell } from '@9softstudio/react-fixed-table-header';
import { Button } from 'reactstrap';

import { htmlDecode } from '../../../../modules/htmlHelper';
import { headerText, maxWidth, globalResource } from '../modules/constants';
import { setShowPopup, setNoteInfo, getNoteList, getNoteContentById, toggleLoaderUserPanel } from '../actions';

export class Content extends PureComponent {
    constructor(props) {
        super(props);

        const { pagingOption, affId } = props;
        const param = Object.assign({}, { affId, pageIndex: 1, pageSize: pagingOption.PageSize });

        this.props.getNoteList(param);
    }

    _renderHeader() {
        return (
            <Row key="thead">
                <Cell header={true} colWidth={35} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell header={true} colWidth={580} title={headerText.Content}>{headerText.Content}</Cell>
                <Cell header={true} colWidth={150} title={headerText.CreatedBy}>{headerText.CreatedBy}</Cell>
                <Cell header={true} colWidth={200} title={headerText.LastModify}>{headerText.LastModify}</Cell>
                <Cell header={true} colWidth={35} title={globalResource.Edit}>{globalResource.Edit}</Cell>
            </Row>
        );
    }

    _renderDataSection(dataList) {
        const dataRows = [];

        if (dataList.length) {
            const { pagingOption } = this.props;
            const startIndex = (pagingOption.PageIndex - 1) * pagingOption.PageSize + 1;

            for (let i = 0; i < dataList.length; i++) {
                const dataItem = dataList[i];
                dataRows.push(this._buildDataRow(dataItem, i + startIndex));
            }
        }
        else {
            dataRows.push(<Row key="no-info">
                <Cell className="text-center" colSpan="5">{globalResource.NoInfo}</Cell>
            </Row>);
        }

        return dataRows;
    }

    _buildDataRow(dataItem, index) {
        const noteContent = htmlDecode(dataItem.NoteContent);
        const createdBy = htmlDecode(dataItem.CreatedBy);

        return (
            <Row key={`dataRow-${index}`}>
                <Cell title={index} className="text-center">{index}</Cell>
                <Cell title={noteContent}>{noteContent}</Cell>
                <Cell title={createdBy}>{createdBy}</Cell>
                <Cell title={dataItem.LastModifyText} className="text-center">{dataItem.LastModifyText}</Cell>
                <Cell title="Edit Note" className="text-center">
                    <Button className="p-0" color="link" onClick={() => this.handleShowPopup(dataItem.NoteId)}>
                        <span className="oi oi-pencil text-secondary"></span>
                    </Button>
                </Cell>
            </Row>
        )
    }

    handleShowPopup = (noteId) => {
        if (noteId === 0) {
            this.props.setNoteInfo({ noteId, noteContent: '' });
        }
        else {
            this.getNoteContentById(noteId);
        }

        this.props.setShowPopup(true);
    }

    async getNoteContentById(noteId) {
        this.props.toggleLoaderUserPanel(true);

        const { affId } = this.props;
        const data = await this.props.getNoteContentById({ affId, noteId });

        if (data) {
            const { NoteId, NoteContent } = data;
            const noteItem = { noteId: NoteId, noteContent: NoteContent };
            this.props.setNoteInfo(noteItem);
        }

        this.props.toggleLoaderUserPanel(false);
    }

    handlePaging = (pageIndex, pageSize) => {
        const newParam = { affId: this.props.affId, pageIndex, pageSize };
        this.props.getNoteList(newParam);
    };

    render() {
        const { noteList } = this.props;

        return noteList && (
            <div className="data-content">
                <div className="my-2">
                    <Button outline type="button" onClick={() => this.handleShowPopup(0)}>{globalResource.Add} <span className="oi oi-plus ml-2"></span></Button>
                </div>
                <div className="table-responsive table-striped">
                    <Table
                        maxWidth={maxWidth}
                        bodyHeight={300}
                        header={this._renderHeader()}
                        body={this._renderDataSection(noteList)}
                        containerPadding={10}
                        isPaging={true}
                        pageOption={this.props.pagingOption}
                        onPaging={this.handlePaging}
                    />
                </div>
            </div>
        );
    }
}


function mapStateToProps({ affCommonInfo, note }) {
    const { pagingOption, noteList, noteInfo } = note;
    const { affId } = affCommonInfo;

    return Object.assign({}, {
        affId,
        pagingOption,
        noteList,
        noteInfo
    });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setShowPopup, setNoteInfo, toggleLoaderUserPanel, getNoteList, getNoteContentById }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Content);