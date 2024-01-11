import React from 'react';
import { connect } from 'react-redux';
import { Table, Row, Cell } from '@9softstudio/react-fixed-table-header';

import { htmlDecode } from '../../../../modules/htmlHelper';
import { headerText, globalResource } from '../libs/constants';

export const Activated = (props) => {
    const { activatedGridData } = props;

    function _renderHeader() {
        return (
            <Row key="thead">
                <Cell header={true} colWidth={20} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell header={true} colWidth={100} title={headerText.UserName}>{headerText.UserName}</Cell>
                <Cell header={true} colWidth={100} title={headerText.IbcUserName}>{headerText.IbcUserName}</Cell>
                <Cell header={true} colWidth={100} title={headerText.FirstName}>{headerText.FirstName}</Cell>
                <Cell header={true} colWidth={100} title={headerText.LastName}>{headerText.LastName}</Cell>
                <Cell header={true} colWidth={100} title={headerText.IBCCustID}>{headerText.IBCCustID}</Cell>
                <Cell header={true} colWidth={80} title={headerText.CreatedDate}>{headerText.CreatedDate}</Cell>
            </Row>
        );
    }

    function _renderDataSection(dataList) {
        const dataRows = [];

        if (dataList.length) {
            for (let i = 0; i < dataList.length; i++) {
                const dataItem = dataList[i];
                dataRows.push(_buildDataRow(dataItem, i + 1));
            }
        }
        else {
            dataRows.push(<Row key="no-info">
                <Cell className="text-center" colSpan="7">{globalResource.NoInfo}</Cell>
            </Row>);
        }

        return dataRows;
    }

    function _buildDataRow(dataItem, index) {
        const userName = htmlDecode(dataItem.UserName);
        const ibcUserName = htmlDecode(dataItem.IbcUserName);
        const firstName = htmlDecode(dataItem.FirstName);
        const lastName = htmlDecode(dataItem.LastName);

        return (
            <Row key={`dataRow-${index}`}>
                <Cell className="text-center" title={index}>{index}</Cell>
                <Cell className="text-center" title={userName}>{userName}</Cell>
                <Cell className="text-center" title={ibcUserName}>{ibcUserName}</Cell>
                <Cell className="text-center" title={firstName}>{firstName || '---'}</Cell>
                <Cell className="text-center" title={lastName}>{lastName || '---'}</Cell>
                <Cell className="text-center" title={dataItem.IbcCustId}>{dataItem.IbcCustId}</Cell>
                <Cell className="text-center" title={dataItem.CreatedDateText}>{dataItem.CreatedDateText}</Cell>
            </Row>
        )
    }

    return activatedGridData && (
        <div className="data-content">
            <div className="active-member-content table-responsive table-striped my-2">
                <div className="title-text">Players have been activated already</div>
                <Table
                    maxWidth={1000}
                    bodyHeight={300}
                    header={_renderHeader()}
                    body={_renderDataSection(activatedGridData)}
                    containerPadding={20}
                />
            </div>
        </div>
    );
}

function mapStateToProps({ activatedGridData }) {
    return Object.assign({}, { activatedGridData });
}

export default connect(mapStateToProps)(React.memo(Activated));