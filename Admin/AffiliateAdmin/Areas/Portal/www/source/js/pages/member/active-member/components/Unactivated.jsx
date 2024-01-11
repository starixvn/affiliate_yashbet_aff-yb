import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table, Row, Cell } from '@9softstudio/react-fixed-table-header';
import { Button } from 'reactstrap';

import { htmlDecode } from '../../../../modules/htmlHelper';
import { headerText, globalResource } from '../libs/constants';
import { setMessage, activeMember, setShouldResetScrollPosition } from '../actions';

export const Unactivated = (props) => {
    const { unactivatedGridData, shouldResetScrollPosition, userNames } = props;

    const _handleActive = async (userNames) => {
        props.setShouldResetScrollPosition(false);

        var data = await props.activeMember({ userNames });

        data ? props.setMessage(data.IsSuccessful ? "Players has been activated successfully!" : data.Message) : props.setMessage("Activate players is failed!");
    }

    function _renderHeader() {
        return (
            <Row key="thead">
                <Cell header={true} colWidth={20} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell header={true} colWidth={100} title={headerText.UserName}>{headerText.UserName}</Cell>
                <Cell header={true} colWidth={100} title={headerText.FirstName}>{headerText.FirstName}</Cell>
                <Cell header={true} colWidth={100} title={headerText.LastName}>{headerText.LastName}</Cell>
                {
                    unactivatedGridData && unactivatedGridData.length !== 0 && (<Cell header={true} colWidth={50} title={headerText.ActiveAll}>
                        <Button color="link p-0" onClick={() => _handleActive(userNames)}>
                            Active All
                    </Button>
                    </Cell>)
                }
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
                <Cell className="text-center" colSpan="4">{globalResource.NoInfo}</Cell>
            </Row>);
        }

        return dataRows;
    }

    function _buildDataRow(dataItem, index) {
        const userName = htmlDecode(dataItem.UserName);
        const firstName = htmlDecode(dataItem.FirstName);
        const lastName = htmlDecode(dataItem.LastName);

        return (
            <Row key={`dataRow-${index}`}>
                <Cell className="text-center" title={index}>{index}</Cell>
                <Cell className="text-center" title={userName}>{userName}</Cell>
                <Cell className="text-center" title={firstName}>{firstName || '---'}</Cell>
                <Cell className="text-center" title={lastName}>{lastName || '---'}</Cell>
                <Cell className="text-center" title="Active">
                    <Button color="link p-0" onClick={() => _handleActive(dataItem.UserName)}>
                        Active
                    </Button>
                </Cell>
            </Row>
        )
    }

    return unactivatedGridData && (
        <div className="data-content">
            <div className="active-member-content table-responsive table-striped my-2">
                <div className="title-text">Members have not been activated</div>
                <Table
                    maxWidth={690}
                    bodyHeight={300}
                    header={_renderHeader()}
                    body={_renderDataSection(unactivatedGridData)}
                    containerPadding={20}
                    shouldResetScrollPosition={shouldResetScrollPosition}
                />
            </div>
        </div>
    );
}

function mapStateToProps({ unactivatedGridData, shouldResetScrollPosition, userNames }) {
    return Object.assign({}, { unactivatedGridData, shouldResetScrollPosition, userNames });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setMessage, activeMember, setShouldResetScrollPosition }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(Unactivated));