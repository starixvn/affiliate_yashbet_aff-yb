import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table, Row, Cell } from '@9softstudio/react-fixed-table-header';
import { Button } from 'reactstrap';

import { htmlDecode } from '../../../../modules/htmlHelper';
import { headerText } from '../libs/constants';
import { globalResource, pcLaptop, mobile, userTypeList } from '../../common/libs/constants';
import { setSelectedTextId, setPagingOption, fetchData, setCheckedTexts, setShouldResetScrollPosition, setParam } from '../actions';

export const Content = (props) => {

    useEffect(() => {
        const { param } = props;
        !!param && props.fetchData(param);
    }, [props.param]);

    const _onCheckItem = (isChecked, textId) => {
        const newCheckedTexts = Object.assign({}, props.checkedTexts, { [textId]: isChecked });

        props.setShouldResetScrollPosition(false);
        props.setCheckedTexts(newCheckedTexts);
    }

    const _handlePaging = (pageIndex, pageSize) => {
        const newPaging = {
            PageIndex: pageIndex,
            PageSize: pageSize
        };
        const newParam = Object.assign({}, props.param, newPaging);

        props.setPagingOption(newPaging);
        props.setParam(newParam);
    };

    function _renderHeader() {
        return (
            <Row key="thead">
                <Cell header={true} colWidth={50}></Cell>
                <Cell header={true} colWidth={50} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell header={true} colWidth={100} title={headerText.UserType}>{headerText.UserType}</Cell>
                <Cell header={true} colWidth={550} title={headerText.Content}>{headerText.Content}</Cell>
                <Cell header={true} colWidth={125} title={headerText.Category}>{headerText.Category}</Cell>
                <Cell header={true} colWidth={150} title={headerText.Language}>{headerText.Language}</Cell>
                <Cell header={true} colWidth={125} title={headerText.Currency}>{headerText.Currency}</Cell>
                <Cell header={true} colWidth={100} title={headerText.Device}>{headerText.Device}</Cell>
                <Cell header={true} colWidth={50} title={globalResource.Edit}>{globalResource.Edit}</Cell>
            </Row>
        );
    }

    function _renderDataSection(dataList) {
        const dataRows = [];

        if (dataList.length) {
            const { pagingOption } = props;
            const startIndex = (pagingOption.PageIndex - 1) * pagingOption.PageSize + 1;

            for (let i = 0; i < dataList.length; i++) {
                const dataItem = dataList[i];
                dataRows.push(_buildDataRow(dataItem, i + startIndex));
            }
        }
        else {
            dataRows.push(<Row key="no-info">
                <Cell className="text-center" colSpan="8">{globalResource.NoInfo}</Cell>
            </Row>);
        }

        return dataRows;
    }

    function _buildDataRow(dataItem, index) {
        const content = htmlDecode(dataItem.TextContent);
        const deviceType = dataItem.DeviceType == 0 ? pcLaptop : mobile;
        const userType = userTypeList[dataItem.UserType] || '';

        return (
            <Row key={`dataRow-${index}`}>
                <Cell className="text-center">
                    <input type="checkbox" checked={props.checkedTexts[dataItem.TextId] || false}
                        onChange={(e) => _onCheckItem(e.target.checked, dataItem.TextId)} />
                </Cell>
                <Cell className="text-center" title={index}>{index}</Cell>
                <Cell className="text-center" title={userType}>{userType}</Cell>
                <Cell className="text-content" title={content}>{content}</Cell>
                <Cell className="text-center" title={dataItem.Category}>{dataItem.Category}</Cell>
                <Cell className="text-center" title={dataItem.Language}>{dataItem.Language}</Cell>
                <Cell className="text-center" title={dataItem.Currency}>{dataItem.Currency}</Cell>
                <Cell className="text-center" title={deviceType}>{deviceType}</Cell>
                <Cell className="text-center" title="Edit Text">
                    <Button color="link" onClick={() => props.setSelectedTextId(dataItem.TextId)}>
                        <span className="oi oi-pencil text-secondary"></span>
                    </Button>
                </Cell>
            </Row>
        )
    }

    return props.gridData && (
        <div className="data-content">
            <div className="text-list-content table-responsive table-striped my-2">
                <Table
                    minWidth={1200}
                    autoWidth={true}
                    bodyHeight={300}
                    header={_renderHeader()}
                    body={_renderDataSection(props.gridData)}
                    containerPadding={20}
                    isPaging={true}
                    pageOption={props.pagingOption}
                    onPaging={_handlePaging}
                    shouldResetScrollPosition={props.shouldResetScrollPosition}
                />
            </div>
        </div>
    );
}

function mapStateToProps({ pagingOption, param, checkedTexts, shouldResetScrollPosition, gridData }) {
    return Object.assign({}, { pagingOption, param, checkedTexts, shouldResetScrollPosition, gridData });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedTextId, fetchData, setPagingOption, setCheckedTexts, setShouldResetScrollPosition, setParam }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(Content));