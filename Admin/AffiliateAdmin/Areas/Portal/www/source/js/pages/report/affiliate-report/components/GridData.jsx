import React, { PureComponent, createRef, Fragment } from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Table, Button } from 'reactstrap';
import { Pager } from '@9softstudio/react-fixed-table-header';

import { format, formatDecimal } from '../../../../modules/numberFormat';
import { setPagingOption, setViewLevel, setParam, fetchData } from '../actions';
import { toggleLoader } from '../../../../actions/loaderAction';
import { globalResource } from '../../../../modules/globalResource';
import { resolveClientUrl } from '../../../../modules/utils';
import { getMaxHeight } from '../../libs/constants';
import { viewLevel, sortByMonthOnMonthView, sortDirectionType, totalText, headerText } from '../libs/constants';

export class GridData extends PureComponent {
    constructor(props) {
        super(props);

        this.cachedMainReport = null;
        this.totalItemMainReport = 0;
        this.count = 0;

        this.state = {
            gridData: null,
            total: null
        }

        this.tableRef = createRef();
    }

    componentDidUpdate(prevProps) {
        const { param } = this.props;
        const isMainView = this._isMainView();

        if (param !== prevProps.param && isMainView) {
            if (this.cachedMainReport && this.cachedMainReport.gridData) {
                const { gridData, total, totalItem } = this.cachedMainReport;

                this.setState({ gridData, total }, () => {
                    this.cachedMainReport = null;
                });
                this.props.setPagingOption({ TotalItem: totalItem, PageIndex: param.PageIndex, PageSize: param.PageSize });
            }
            else {
                this.fetchData(param);
            }
        }
        else if (param !== prevProps.param) {
            if (this.cachedMainReport === null) {
                this.cachedMainReport = Object.assign(this.state, { totalItem: prevProps.pagingOption.TotalItem });
            }

            this.fetchData(param);
        }
    }

    _isMainView() {
        return this.props.currentViewLevel === viewLevel.main;
    }

    _isBeforeCutOffDate() {
        const { ToDate } = this.props.param;
        const toDate = new Date(ToDate);
        const reportCutOffDate = new Date(this.props.reportCutOffDate);

        return toDate.getTime() < reportCutOffDate.getTime();
    }

    async fetchData(param) {
        this._resetScroll();
        this.props.toggleLoader(true);
        const isMainView = this._isMainView();
        const data = await this.props.fetchData(param, isMainView);

        if (data) {
            const { MainData, Total, TotalItem } = data;

            this.setState({ gridData: MainData, total: Total });
            this.props.setPagingOption({ TotalItem, PageIndex: param.PageIndex, PageSize: param.PageSize });

            if (isMainView) {
                this.cachedMainReport = null;
            }
        }
        this.props.toggleLoader(false);
    }

    _renderHeader() {
        const isMainView = this._isMainView();
        const firstColName = isMainView ? headerText.UserName : headerText.MonthDateString;

        return [
            <tr className="freeze-row freeze-row-h1" key="thead-1">
                <th className="freeze-column-header" width={110} rowSpan="2" title={firstColName}>{firstColName}</th>
                {isMainView && (<th width={70} rowSpan="2" title={headerText.PaymentCurrency}>{headerText.PaymentCurrency}</th>)}
                <th width={60} rowSpan="2" title={headerText.TotalClick}>{headerText.TotalClick}</th>
                <th width={55} rowSpan="2" title={headerText.UniqueClick}>{headerText.UniqueClick}</th>
                <th width={55} rowSpan="2" title={headerText.Signup}>{headerText.Signup}</th>
                <th width={55} rowSpan="2" title={headerText.Fdm}>{headerText.Fdm}</th>
                <th width={55} rowSpan="2" title={headerText.ActiveMember}>{headerText.ActiveMember}</th>
                <th width={65} rowSpan="2" title={headerText.NoOfSubAff}>{headerText.NoOfSubAff}</th>
                <th colSpan="10" height={28} title={headerText.MemberStatistics}>{headerText.MemberStatistics}</th>
                <th colSpan={isMainView ? 8 : 7} title={headerText.AffiliateStatistics}>{headerText.AffiliateStatistics}</th>
                {isMainView && (<th width={55} rowSpan="2" title={headerText.MonthOnMonth}>{headerText.MonthOnMonth}</th>)}
                {isMainView && (<th width={55} rowSpan="2" title={headerText.PlayerGgr}>{headerText.PlayerGgr}</th>)}
            </tr>,
            <tr className="freeze-row freeze-row-h2" key="thead-2">
                <th width={75} title={headerText.Deposit}>{headerText.Deposit}</th>
                <th width={75} title={headerText.Withdrawal}>{headerText.Withdrawal}</th>
                <th width={75} title={headerText.Turnover}>{headerText.Turnover}</th>
                <th width={75} title={headerText.Ggr}>{headerText.Ggr}</th>
                <th width={70} title={headerText.Bonus}>{headerText.Bonus}</th>
                <th width={70} title={headerText.Rebate}>{headerText.Rebate}</th>
                <th width={70} title={headerText.PlatformFee}>{headerText.PlatformFee}</th>
                <th width={70} title={headerText.DepositFee}>{headerText.DepositFee}</th>
                <th width={70} title={headerText.WithdrawalFee}>{headerText.WithdrawalFee}</th>
                <th width={70} title={headerText.TotalFee}>{headerText.TotalFee}</th>

                <th width={65} title={headerText.CommissionRate}>{headerText.CommissionRate}</th>
                <th width={75} title={headerText.Earning}>{headerText.Earning}</th>
                <th width={75} title={headerText.P2pEarning}>{headerText.P2pEarning}</th>
                <th width={75} title={headerText.SubAffEarning}>{headerText.SubAffEarning}</th>
                <th width={75} title={headerText.TotalEarning}>{headerText.TotalEarning}</th>
                <th width={75} title={headerText.AffWithdrawal}>{headerText.AffWithdrawal}</th>
                {isMainView && (<th width={75} title={headerText.AffAdjustment}>{headerText.AffAdjustment}</th>)}
                <th width={80} title={headerText.BalanceBf}>{headerText.BalanceBf}</th>
            </tr>
        ];
    }

    _renderDataSection(dataList) {
        const dataRows = [];

        if (dataList.length) {
            const { productList } = this.props;
            const isMainView = this._isMainView();
            const isBeforeCutOffDate = this._isBeforeCutOffDate();
            const isSelectedAllProducts = productList.every(p => p.checked);
            const productColumnClassName = isSelectedAllProducts ? 'text-right' : 'text-right highlight';

            for (let i = 0; i < dataList.length; i++) {
                const dataItem = dataList[i];
                dataRows.push(this._buildDataRow(dataItem, productColumnClassName, i, isMainView, isBeforeCutOffDate));
            }
        }
        else {
            dataRows.push(<tr key="no-info">
                <td className="text-center" colSpan={24 + (this._isMainView() ? 3 : 0)}>{globalResource.NoInfo}</td>
            </tr>);
        }

        return dataRows;
    }

    _buildDataRow(dataItem, productColumnClassName, index, isMainView, isBeforeCutOffDate) {
        const isTotalRow = isMainView ? (dataItem.AffId === 0 && dataItem.UserName === null) : (dataItem.MonthDateString === "");
        const firstColValue = isTotalRow ?
            totalText :
            (isMainView ? dataItem.UserName : this._buildMonthCell(dataItem.MonthDateString, dataItem.Description));

        return (
            <tr key={`dataRow-${index}`}>
                <td className="freeze-column" title={firstColValue}>{firstColValue}</td>
                {this._buildCommonCell(dataItem, productColumnClassName, isTotalRow, isMainView, isBeforeCutOffDate)}
            </tr>
        )
    }

    _buildMonthCell(monthDate, description) {
        return description ? `${monthDate} (${description})` : monthDate;
    }

    _buildCommonCell(dataItem, productColumnClassName, isTotalRow, isMainView, isBeforeCutOffDate) {
        const totalClick = format(dataItem.TotalClick);
        const uniqueClick = format(dataItem.UniqueClick);
        const signup = format(dataItem.Signup);
        const fdm = format(dataItem.Fdm);
        const activeMember = format(dataItem.ActiveMember);
        const noOfSubAff = format(dataItem.NoOfSubAff);
        const deposit = formatDecimal(dataItem.Deposit);
        const withdrawal = formatDecimal(dataItem.Withdrawal);
        const turnover = formatDecimal(dataItem.Turnover);
        const ggr = formatDecimal(dataItem.Ggr);
        const bonus = formatDecimal(dataItem.Bonus);
        const rebate = formatDecimal(dataItem.Rebate);
        const platformFee = formatDecimal(dataItem.PlatformFee);
        const depositFee = formatDecimal(dataItem.DepositFee);
        const withdrawalFee = formatDecimal(dataItem.WithdrawalFee)
        const totalFee = formatDecimal(dataItem.TotalFee);
        const earning = formatDecimal(dataItem.Earning);
        const p2pEarning = formatDecimal(dataItem.P2pEarning);
        const affWithdrawal = formatDecimal(dataItem.AffWithdrawal);
        const affAdjustment = formatDecimal(dataItem.AffAdjustment);
        const balanceBf = formatDecimal(dataItem.BalanceBf);
        const commRate = isTotalRow ? 'N/A' : dataItem.CommissionRate + '%';
        const subAffEarning = formatDecimal(dataItem.SubAffEarning);
        const totalEarning = formatDecimal(dataItem.TotalEarning);
        const paymentCurrency = isTotalRow ? '' : dataItem.PaymentCurrency;
        return (
            <React.Fragment>
                {isMainView && (<td className="text-center" title={paymentCurrency}>{paymentCurrency}</td>)}
                <td className="text-right" title={totalClick}>{totalClick}</td>
                <td className="text-right" title={uniqueClick}>{uniqueClick}</td>
                <td className="text-right" title={signup}>{signup}</td>
                <td className="text-right" title={fdm}>{fdm}</td>
                <td className="text-right" title={activeMember}>{activeMember}</td>
                <td className="text-right" title={noOfSubAff}>{noOfSubAff}</td>
                <td className="text-right" title={deposit}>{deposit}</td>
                <td className="text-right" title={withdrawal}>{withdrawal}</td>
                <td className={productColumnClassName} title={turnover}>{turnover}</td>
                <td className={productColumnClassName} title={ggr}>{ggr}</td>
                <td className="text-right" title={bonus}>{bonus}</td>
                <td className="text-right" title={rebate}>{rebate}</td>
                <td className="text-right" title={platformFee}>{platformFee}</td>
                <td className="text-right" title={depositFee}>{depositFee}</td>
                <td className="text-right" title={withdrawalFee}>{withdrawalFee}</td>
                <td className="text-right" title={totalFee}>{totalFee}</td>
                {
                    dataItem.IsMultiGroupProduct && isMainView ? (
                        <td className="text-right text-danger" title={isBeforeCutOffDate ? "First product group's share% of multiple groups" : "The highest share % of multiple groups"}>{commRate}</td>
                    ) : (
                            <td className="text-right" title={commRate}>{commRate}</td>
                        )
                }
                <td className="text-right" title={earning}>{earning}</td>
                <td className="text-right" title={p2pEarning}>{p2pEarning}</td>
                <td className="text-right" title={subAffEarning}>{subAffEarning}</td>
                <td className="text-right" title={totalEarning}>{totalEarning}</td>
                <td className="text-right" title={affWithdrawal}>{affWithdrawal}</td>
                {isMainView && (<td className="text-right" title={affAdjustment}>{affAdjustment}</td>)}
                <td className="text-right" title={balanceBf}>{balanceBf}</td>
                {
                    isMainView && (
                        <td className="text-center">
                            {dataItem.AffId > 0 ? (
                                <Button color="link" className="p-0"
                                    onClick={() => this.handleViewMonthOnMonth(dataItem.UserName)}>View</Button>) : ''}
                        </td>)
                }
                {
                    isMainView && (
                        <td className="text-center">
                            {dataItem.AffId > 0 ? (
                                <Button color="link" className="p-0"
                                    onClick={() => this.handleViewPlayerGgrReport(dataItem.UserName)}>View</Button>) : ''}
                        </td>)
                }
            </React.Fragment>
        )
    }

    handleViewMonthOnMonth = (userName) => {
        const { param, pagingOption } = this.props;

        const postParam = Object.assign({}, param, {
            UserName: userName,
            SortType: sortByMonthOnMonthView.Month,
            SortDirectionType: sortDirectionType.descending,
            PageIndex: 1,
            PageSize: pagingOption.PageSize
        })

        this.props.setViewLevel(viewLevel.monthOnMonth);
        this.props.setParam(postParam);

        this._resetScroll();
    }

    handleViewPlayerGgrReport = (userName) => {
        const { param } = this.props;

        const paramFromAffReport = {
            affName: userName,
            baseCurrencyId: param.BaseCurrencyId,
            fromDate: param.FromDate,
            toDate: param.ToDate,
            productIds: param.ProductIds
        };

        const path = resolveClientUrl('Portal/PlayerGgrReport');
        document.cookie = `paramFromAffReport=${JSON.stringify(paramFromAffReport)}; path=${path}`;

        window.open(path, '_blank');
    }

    handlePaging = (index, size) => {
        const newPaging = {
            PageIndex: index,
            PageSize: size
        };
        const newParam = Object.assign({}, this.props.param, newPaging);

        this.fetchData(newParam);
    }

    _resetScroll = () => {
        if (!!this.tableRef.current) {
            this.tableRef.current.scrollTop = 0;
            this.tableRef.current.scrollLeft = 0;
        }
    }

    render() {
        const { gridData, total } = this.state;
        const maxHeight = getMaxHeight();

        return gridData && (
            <Fragment>
                <div className="table-responsive aff-report-table-container" style={{ maxHeight }} ref={this.tableRef}>
                    <Table className="freeze-table">
                        <thead>
                            {
                                this._renderHeader()
                            }
                        </thead>
                        <tbody>
                            {
                                this._renderDataSection(gridData)
                            }
                        </tbody>
                        <tfoot>
                            {total && this._renderDataSection([total])}
                        </tfoot>
                    </Table>
                </div>
                <Pager
                    pageOption={this.props.pagingOption}
                    onPaging={this.handlePaging} />
            </Fragment>
        );
    }
}

function mapStateToProps({ pagingOption, param, currentViewLevel, productList, reportCutOffDate }) {
    return Object.assign({}, { pagingOption, param, currentViewLevel, productList, reportCutOffDate });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setPagingOption, toggleLoader, setViewLevel, setParam, fetchData }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps, null, { forwardRef: true })(GridData);