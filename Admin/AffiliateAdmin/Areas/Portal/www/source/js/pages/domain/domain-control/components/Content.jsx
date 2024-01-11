import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table, Row, Cell } from '@9softstudio/react-fixed-table-header';
import { Button } from 'reactstrap';
import NotificationPopup from '../../../common/components/NotificationPopup';

import { htmlDecode } from '../../../../modules/htmlHelper';
import { headerText, globalResource } from '../libs/constants';
import { deleteDomain, fetchData, fetchDomain, toggleLoader, setSelectedDomain, setShouldResetScrollPosition } from '../actions';

export class Content extends PureComponent {
    constructor(props) {
        super(props);

        this.state = {
            selectedDomainId: 0,
            message: '',
            isConfirmPopup: true
        }
    }

    componentDidMount() {
        const { affDomainList, redirectDomainList } = this.props;

        (!affDomainList || !redirectDomainList) && this.props.fetchDomain();
    }

    _handleCloseNotificationPopup = (confirmResult) => {
        this.setState({ message: '' });

        const { selectedDomainId } = this.state;
        if (confirmResult && selectedDomainId) {
            this._deleteDomain(selectedDomainId);
        }
    }

    _handleDeleteDomain = (selectedDomainId) => {
        this.props.setShouldResetScrollPosition(false);
        this.setState({ message: `Are you sure you want to delete this domain setting?`, isConfirmPopup: true, selectedDomainId });
    }

    _handleEditDomain = (domainId, marketId, affDomainId, redirectDomainId, isUsed) => {
        this.props.setShouldResetScrollPosition(false);
        this.props.setSelectedDomain({ domainId, marketId, affDomainId, redirectDomainId, isUsed });
    }

    _deleteDomain = async (selectedDomainId) => {
        this.props.toggleLoader(true);

        const result = await this.props.deleteDomain({ DomainId: selectedDomainId });

        if (result) {
            if (result.IsSuccessful) {
                this.setState({ message: `The domain control has been removed successfully!`, isConfirmPopup: false });

                this.props.fetchData(this.props.selectedMarket);
            }
            else {
                this.setState({ message: result.Message, isConfirmPopup: false });
            }
        }

        this.props.toggleLoader(false);
    }

    _renderHeader() {
        return (
            <Row key="thead">
                <Cell header={true} colWidth={20} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell header={true} colWidth={100} title={headerText.Market}>{headerText.Market}</Cell>
                <Cell header={true} colWidth={120} title={headerText.AffDomain}>{headerText.AffDomain}</Cell>
                <Cell header={true} colWidth={120} title={headerText.RedirectDomain}>{headerText.RedirectDomain}</Cell>
                <Cell header={true} colWidth={60} title={headerText.Creator}>{headerText.Creator}</Cell>
                <Cell header={true} colWidth={60} title={headerText.CreatedDate}>{headerText.CreatedDate}</Cell>
                <Cell header={true} colWidth={20} title={headerText.Status}>{headerText.Status}</Cell>
                <Cell header={true} colWidth={20} title={headerText.Edit}>{headerText.Edit}</Cell>
                <Cell header={true} colWidth={20} title={headerText.Delete}>{headerText.Delete}</Cell>
            </Row>
        );
    }

    _renderDataSection(dataList) {
        const dataRows = [];

        if (dataList.length) {
            for (let i = 0; i < dataList.length; i++) {
                const dataItem = dataList[i];
                dataRows.push(this._buildDataRow(dataItem, i + 1));
            }
        }
        else {
            dataRows.push(<Row key="no-info">
                <Cell className="text-center" colSpan="9">{globalResource.NoInfo}</Cell>
            </Row>);
        }

        return dataRows;
    }

    _buildDataRow(dataItem, index) {
        const affDomain = htmlDecode(dataItem.AffDomain);
        const redirectDomain = htmlDecode(dataItem.RedirectDomain);
        const creator = htmlDecode(dataItem.Creator);

        return (
            <Row key={`dataRow-${index}`}>
                <Cell className="text-center" title={index}>{index}</Cell>
                <Cell className="text-center" title={dataItem.Market}>{dataItem.Market}</Cell>
                <Cell className="text-center" title={affDomain}>{affDomain}</Cell>
                <Cell className="text-center" title={redirectDomain}>{redirectDomain}</Cell>
                <Cell className="text-center" title={creator}>{creator}</Cell>
                <Cell className="text-center" title={dataItem.CreatedDateText}>{dataItem.CreatedDateText}</Cell>
                <Cell className="text-center">
                    <input type="checkbox" disabled checked={dataItem.IsUsed} />
                </Cell>
                <Cell className="text-center" title="Edit Domain">
                    <Button color="link p-0" onClick={() => this._handleEditDomain(dataItem.DomainId, dataItem.MarketId, dataItem.AffDomainId, dataItem.RedirectDomainId, dataItem.IsUsed)}>
                        <span className="oi oi-pencil text-secondary"></span>
                    </Button>
                </Cell>
                <Cell className="text-center" title="Delete Domain">
                    <Button color="link p-0" onClick={() => this._handleDeleteDomain(dataItem.DomainId)}>
                        <span className="oi oi-x text-danger"></span>
                    </Button>
                </Cell>
            </Row>
        )
    }

    render() {
        const { gridData, shouldResetScrollPosition, selectedMarket, affDomainList, redirectDomainList, marketList } = this.props;
        const { message, isConfirmPopup } = this.state;
        const languageCode = selectedMarket || (marketList[0].LanguageCode || 'en-US');

        return gridData && (
            <div className="data-content">
                <div className="my-2">
                    <Button outline type="button" onClick={() => this._handleEditDomain(0, languageCode, affDomainList[0] ? affDomainList[0].DomainId : 0, redirectDomainList[0] ? redirectDomainList[0].DomainId : 0, false)}>Add New <span className="oi oi-plus ml-2"></span></Button>
                </div>
                <div className="domain-list-content table-responsive table-striped my-2">
                    <Table
                        minWidth={1000}
                        autoWidth={true}
                        bodyHeight={300}
                        header={this._renderHeader()}
                        body={this._renderDataSection(gridData)}
                        containerPadding={20}
                        shouldResetScrollPosition={shouldResetScrollPosition}
                    />
                </div>
                {
                    !!message && <NotificationPopup
                        message={message}
                        isConfirmPopup={isConfirmPopup}
                        onClose={this._handleCloseNotificationPopup}
                    />
                }
            </div>
        );
    }
}

function mapStateToProps({ shouldResetScrollPosition, gridData, selectedMarket, affDomainList, redirectDomainList, marketList }) {
    return Object.assign({}, { shouldResetScrollPosition, gridData, selectedMarket, affDomainList, redirectDomainList, marketList });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ deleteDomain, fetchData, fetchDomain, toggleLoader, setSelectedDomain, setShouldResetScrollPosition }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Content);