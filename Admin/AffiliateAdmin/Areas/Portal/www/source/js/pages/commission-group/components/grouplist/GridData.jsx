import React, { Component, Fragment } from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table, Row, Cell } from '@9softstudio/react-fixed-table-header';

import { resolveClientUrl } from '../../../../modules/utils';
import { globalResource, format, formatDecimal } from '../../modules/constant';
import { deleteCommGroup, getGroupSettingList } from '../../actions';
import { htmlDecode } from '../../../../modules/htmlHelper';
import NotificationPopup from '../../../common/components/NotificationPopup';

export class GridData extends Component {
    constructor(props) {
        super(props);

        const { productList } = this.props;
        this.products = {};
        for (let product of productList) {
            this.products[product.ProductId] = product.ProductName;
        }

        this.state = {
            shouldResetScrollPosition: true,
            selectedGroupId: 0,
            messageNotification: '',
            isConfirmPopup: true
        }
    }

    _convertToProductNameText(productIdList) {
        return productIdList.split(',').reduce((accumulator, currentItem) => {
            const productName = this.products[currentItem.trim()];
            if (productName) {
                accumulator.push(productName);
            }

            return accumulator;
        }, []).join(',');
    }

    handleCloseNotificationPopup = (confirmResult) => {
        this.setState({ messageNotification: '' });

        const { selectedGroupId } = this.state;
        if (confirmResult && selectedGroupId) {
            this._deleteCommGroup(selectedGroupId);
        }
    }

    _handleDeleteCommGroup = (selectedGroupId, groupName) => {
        this.setState({ messageNotification: `Are you sure you want to delete the Revenue Group ${groupName} ?`, isConfirmPopup: true, selectedGroupId, shouldResetScrollPosition: false });
    }

    _deleteCommGroup = async (groupId) => {
        const response = await this.props.deleteCommGroup({ groupId });
        if (response && response.status === 200) {
            const result = response.data;

            if (result.IsSuccessful) {
                const { pagingOption } = this.props;
                const gettingParam = Object.assign({}, { PageIndex: pagingOption.PageIndex, PageSize: pagingOption.PageSize, GroupName: this.props.searchedGroupName });
                this.props.getGroupSettingList(gettingParam);

                this.setState({ messageNotification: "Delete Revenue Group Successfully", isConfirmPopup: false, shouldResetScrollPosition: true });
            }
            else {
                this.setState({ messageNotification: result.Message, isConfirmPopup: false, shouldResetScrollPosition: true });
            }
        }
    }

    _renderHeader() {

        return [
            <Row key="theard">
                <Cell header={true} rowSpan="2" colWidth={40}></Cell>
                <Cell header={true} rowSpan="2" colWidth={150}>Name</Cell>
                <Cell header={true} rowSpan="2" colWidth={50}>No.Aff</Cell>
                <Cell header={true} rowSpan="2" colWidth={150}>Description</Cell>
                <Cell header={true} rowSpan="2" colWidth={100}>Creation</Cell>
                <Cell header={true} colSpan="5">Group Details</Cell>
                <Cell header={true} rowSpan="2" colWidth={120}>Deposit Fee</Cell>
                <Cell header={true} rowSpan="2" colWidth={120}>Withdrawal Fee</Cell>
                <Cell header={true} rowSpan="2" colWidth={120}>Sub-Affiliate Comm Rate</Cell>
                <Cell header={true} rowSpan="2" colWidth={50}>Action</Cell>
            </Row>,
            <Row key="sub-theard" className="d-none">
                <Cell header={true} colWidth={120}></Cell>
                <Cell header={true} colWidth={50}></Cell>
                <Cell header={true} colWidth={120}></Cell>
                <Cell header={true} colWidth={120}></Cell>
                <Cell header={true} colWidth={120}></Cell>
            </Row>
        ];
    }

    _renderTierSetting = (tierSettingList) => {
        const rows = [];

        for (let tierSetting of tierSettingList) {
            const minAmount = format(tierSetting.MinAmount);
            const minAp = format(tierSetting.MinAp);

            rows.push(
                (<Row key={`row-${tierSetting.TierId}-${tierSetting.ProductIds}`}>
                    <Cell className="text-center cell-tier">{tierSetting.TierId}</Cell>
                    <Cell className="text-right cell-tier" title={minAmount}>{minAmount}</Cell>
                    <Cell className="text-right cell-tier" title={minAp}>{minAp}</Cell>
                    <Cell className="text-right cell-tier">{`${format(tierSetting.Commission * 100, 1)}%`}</Cell>
                </Row>)
            );
        }

        return rows;
    }

    _renderGroupData() {
        const { gridData } = this.props;
        const rows = [];

        if (gridData && gridData.length) {
            for (let index = 0; index < gridData.length; index++) {
                const item = gridData[index];

                const { TierSettingList, P2pDeductionList, PlatformFeeList, DepositFee, WithdrawalFee, SubAffCommRate, ...group } = item;
                const tierSettingRowSpan = TierSettingList.length;
                const platformFeeRowSpan = PlatformFeeList.length;
                const p2pDeductionRowSpan = P2pDeductionList.length;
                const rowSpan = tierSettingRowSpan + (platformFeeRowSpan + p2pDeductionRowSpan) * 2;

                const depositFee = DepositFee && (DepositFee.IsDeductedPercent ? `${format(DepositFee.DeductedPercentage * 100, 1)}%` : formatDecimal(Number(DepositFee.DeductedAmount)));
                const withdrawalFee = WithdrawalFee && (WithdrawalFee.IsDeductedPercent ? `${format(WithdrawalFee.DeductedPercentage * 100, 1)}%` : formatDecimal(Number(WithdrawalFee.DeductedAmount)));
                const subAffCommRate = SubAffCommRate && `${format(SubAffCommRate * 100, 1)}%`

                for (let iP2P = 0; iP2P < P2pDeductionList.length; iP2P++) {
                    const p2pDeduction = P2pDeductionList[iP2P];
                    const isFirstItem = iP2P === 0;
                    const isPrivateGroup = group.GroupId > 1000;
                    const p2pFixedRate = `${format(p2pDeduction.Commission * 100, 1)}%`;
                    const royaltyFee = `${format(p2pDeduction.DeductedPercentage * 100, 1)}%`;
                    const productIds = p2pDeduction.ProductIds;

                    rows.push(
                        [
                            <Row key={`row-platform-${index}-${productIds}-1`}>
                                {isFirstItem && (
                                    <Fragment>
                                        <Cell className="text-center" rowSpan={rowSpan}>{group.RowIdx}</Cell>
                                        <Cell className="text-center" rowSpan={rowSpan} title={isPrivateGroup ? "Private Group" : ""}>
                                            {isPrivateGroup && (<span className="oi oi-person"></span>)}{htmlDecode(group.GroupName)}
                                        </Cell>
                                        <Cell className="text-center" rowSpan={rowSpan}>
                                            <a href={resolveClientUrl("Portal/Affiliate?groupId=") + group.GroupId} target="_top">{group.NoAffs}</a>
                                        </Cell>
                                        <Cell className="text-center" rowSpan={rowSpan}>{htmlDecode(group.Description)}</Cell>
                                        <Cell className="text-center" rowSpan={rowSpan}><p>{group.Creator}</p><p>{group.CreatedDateText}</p></Cell>
                                    </Fragment>
                                )}
                                <Cell className="cell-deduction" rowSpan="2">
                                    <div dangerouslySetInnerHTML={{ __html: `<strong>P2P Group ${iP2P + 1}</strong><br />Royalty Fee ${royaltyFee}`}}></div>
                                </Cell>
                                <Cell className="font-weight-bold text-center cell-deduction" colSpan="4">{`Fixed Rate ${p2pFixedRate}`}</Cell>
                                {isFirstItem && (
                                    <Fragment>
                                        <Cell className="text-right" rowSpan={rowSpan} title={depositFee}>{depositFee}</Cell>
                                        <Cell className="text-right" rowSpan={rowSpan} title={withdrawalFee}>{withdrawalFee}</Cell>
                                        <Cell className="text-right" rowSpan={rowSpan} title={subAffCommRate}>{subAffCommRate}</Cell>
                                        <Cell className="text-center" rowSpan={rowSpan}>
                                            <button className="btn btn-link" onClick={(e) => this.props.onEdit(item.GroupId)}>
                                                <span className="oi oi-pencil text-secondary"></span>
                                            </button>

                                            {
                                                group.NoAffs === 0 && (
                                                    <button className="btn btn-link" onClick={() => this._handleDeleteCommGroup(item.GroupId, item.GroupName)}>
                                                        <span className="oi oi-x text-danger"></span>
                                                    </button>)
                                            }
                                        </Cell>
                                    </Fragment>
                                )}
                            </Row>,
                            <Row key={`row-platform-${index}-${productIds}-2`}>
                                <Cell className="cell-tier" colSpan="4">{this._convertToProductNameText(productIds)}</Cell>
                            </Row>,
                        ]
                    );
                }

                for (let iPlatform = 0; iPlatform < PlatformFeeList.length; iPlatform++) {
                    const platformFee = PlatformFeeList[iPlatform]
                    const percent = `${format(platformFee.DeductedPercentage * 100, 1)}%`;
                    const productIds = platformFee.ProductIds;
                    const tierSettings = TierSettingList.filter(t => t.GroupProductId === platformFee.GroupProductId);

                    rows.push(
                        <Row key={`row-platform-${index}-${productIds}`}>
                            <Cell className="cell-deduction" rowSpan={2 + tierSettings.length}>
                                <div dangerouslySetInnerHTML={{ __html: `<strong>Product Group ${iPlatform + 1}</strong><br />Royalty Fee ${percent}` }}></div>
                            </Cell>
                            <Cell className="font-weight-bold text-center cell-deduction">Tier</Cell>
                            <Cell className="font-weight-bold text-center cell-deduction">Min Net Win</Cell>
                            <Cell className="font-weight-bold text-center cell-deduction">Min Active Player</Cell>
                            <Cell className="font-weight-bold text-center cell-deduction">Rev. Share%</Cell>
                        </Row>
                    );

                    rows.push(this._renderTierSetting(tierSettings));

                    rows.push(
                        <Row key={`row-platform-${index}-${productIds}-2`}>
                            <Cell className="cell-tier" colSpan="4">{this._convertToProductNameText(productIds)}</Cell>
                        </Row>
                    );
                }
            }
        }
        else {
            rows.push(<Row key="no-info">
                <Cell className="text-center" colSpan="14">{globalResource.NoInfo}</Cell>
            </Row>);
        }

        return rows;
    }

    render() {
        const { shouldResetScrollPosition, messageNotification, isConfirmPopup } = this.state;

        return (
            <React.Fragment>
                <div className="table-responsive table-group-setting">
                    <Table
                        minWidth={1000}
                        autoWidth={true}
                        bodyHeight={300}
                        header={this._renderHeader()}
                        body={this._renderGroupData()}
                        containerPadding={20}
                        shouldResetScrollPosition={shouldResetScrollPosition}
                    />
                </div>
                {
                    !!messageNotification && <NotificationPopup
                        message={messageNotification}
                        isConfirmPopup={isConfirmPopup}
                        onClose={this.handleCloseNotificationPopup}
                    />
                }
            </React.Fragment>
        );
    }
}

function mapStateToProps({ gridData, productList, searchedGroupName, pagingOption }) {
    return Object.assign({}, { gridData, productList, searchedGroupName, pagingOption });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ deleteCommGroup, getGroupSettingList }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(GridData);
