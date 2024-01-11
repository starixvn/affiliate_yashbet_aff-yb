import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table, Row, Cell } from '@9softstudio/react-fixed-table-header';
import { Button } from 'reactstrap';

import { htmlDecode } from '../../../../modules/htmlHelper';
import { headerText, globalResource } from '../libs/constants';
import { setSelectedLandingPageId, setCheckedLandingPages, setShouldResetScrollPosition } from '../actions';

export const Content = (props) => {

    const _onCheckItem = (isChecked, landingPageId) => {
        const newCheckedLandingPages = Object.assign({}, props.checkedLandingPages, { [landingPageId]: isChecked });

        props.setShouldResetScrollPosition(false);
        props.setCheckedLandingPages(newCheckedLandingPages);
    }

    function _renderHeader() {
        return (
            <Row key="thead">
                <Cell header={true} colWidth={20}></Cell>
                <Cell header={true} colWidth={30} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell header={true} colWidth={250} title={headerText.LandingPageName}>{headerText.LandingPageName}</Cell>
                <Cell header={true} colWidth={250} title={headerText.LandingPageCode}>{headerText.LandingPageCode}</Cell>
                <Cell header={true} colWidth={30} title={globalResource.Edit}>{globalResource.Edit}</Cell>
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
                <Cell className="text-center" colSpan="5">{globalResource.NoInfo}</Cell>
            </Row>);
        }

        return dataRows;
    }

    function _buildDataRow(dataItem, index) {
        const landingPageName = htmlDecode(dataItem.PageName);
        const landingPageCode = htmlDecode(dataItem.PageCode);

        return (
            <Row key={`dataRow-${index}`}>
                <Cell className="text-center">
                    <input type="checkbox" checked={props.checkedLandingPages[dataItem.PageId] || false}
                        onChange={(e) => _onCheckItem(e.target.checked, dataItem.PageId)} />
                </Cell>
                <Cell className="text-center" title={index}>{index}</Cell>
                <Cell className="text-left" title={landingPageName}>{landingPageName}</Cell>
                <Cell className="text-left" title={landingPageCode}>{landingPageCode}</Cell>
                <Cell className="text-center" title="Edit Landing Page">
                    <Button color="link" onClick={() => props.setSelectedLandingPageId(dataItem.PageId)}>
                        <span className="oi oi-pencil text-secondary"></span>
                    </Button>
                </Cell>
            </Row>
        )
    }

    return props.gridData && (
        <div className="data-content">
            <div className="landing-page-content table-responsive table-striped my-2">
                <Table
                    minWidth={1000}
                    autoWidth={true}
                    bodyHeight={300}
                    header={_renderHeader()}
                    body={_renderDataSection(props.gridData)}
                    containerPadding={20}
                    shouldResetScrollPosition={props.shouldResetScrollPosition}
                />
            </div>
        </div>
    );
}

function mapStateToProps({ checkedLandingPages, shouldResetScrollPosition, gridData }) {
    return Object.assign({}, { checkedLandingPages, shouldResetScrollPosition, gridData });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedLandingPageId, setCheckedLandingPages, setShouldResetScrollPosition }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(Content));