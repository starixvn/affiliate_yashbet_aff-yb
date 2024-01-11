import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import MultipleSelect from '../../../../components/multiple-select';
import MonthPicker from '@9softstudio/react-monthpicker';
import { defaultSortBy, sortByMainView, sortByMonthOnMonthView, viewLevel, sortDirectionType, defaultBaseCurrency, minMonthDate, resource } from '../libs/constants';
import { setReportTitle, setParam, setCurrencyList, setCommGroupList, setProductList, setPaymentCurrencyList } from '../actions';
import { formatUnit, handleChangeMultipleSelected } from '../../../../modules/utils';
import { globalResource } from '../../../../modules/globalResource';

export class Filter extends Component {
    constructor(props) {
        super(props);

        const { today, productList, baseCurrencyList, currencyList, paymentCurrencyList } = this.props;
        this.now = new Date(today);
        const defaultMonth = { month: this.now.getMonth() + 1, year: this.now.getFullYear() };

        this.state = {
            userName: '',
            selectedLanguage: '',
            fromMonth: defaultMonth,
            toMonth: defaultMonth,
            sortBy: defaultSortBy,
            isSortAscending: true,
            selectedProductList: productList.filter(item => item.checked).map(item => item.key).join(',') || '',
            selectedCommGroupList: 'all',
            isGetAll: false,
            selectedBaseCurrency: baseCurrencyList.length && baseCurrencyList[0].ExchangeId || defaultBaseCurrency,
            selectedCurrencyList: currencyList.filter(item => item.checked).map(item => item.key).join(',') || '',
            selectedPaymentCurrencyList: paymentCurrencyList.filter(item => item.checked).map(item => item.key).join(',') || ''
        }

        this.cachedMainReportParam = null;
    }

    componentDidUpdate(prevProps) {
        const { param, currentViewLevel, pagingOption } = this.props;

        if (currentViewLevel !== prevProps.currentViewLevel) {
            const callback = () => {
                this._updateReportTitle();
            }

            if (this._isMainView()) {
                const { UserName, GroupIds, PreferredLanguage, FromDate, ToDate, SortType, SortDirectionType, ProductIds, CurrencyIds, PaymentCurrencyIds, BaseCurrencyId } = this.cachedMainReportParam;
                const from = new Date(FromDate);
                const to = new Date(ToDate);

                this.setState({
                    userName: UserName,
                    selectedLanguage: PreferredLanguage,
                    selectedCommGroupList: GroupIds,
                    fromMonth: { month: from.getMonth() + 1, year: from.getFullYear() },
                    toMonth: { month: to.getMonth() + 1, year: to.getFullYear() },
                    sortBy: SortType,
                    isSortAscending: SortDirectionType === sortDirectionType.ascending,
                    selectedProductList: ProductIds,
                    selectedBaseCurrency: BaseCurrencyId,
                    selectedCurrencyList: CurrencyIds,
                    selectedPaymentCurrencyList: PaymentCurrencyIds,
                    isGetAll: false
                }, callback);

                this.props.setParam(this.cachedMainReportParam);
            }
            else {
                this.cachedMainReportParam = Object.assign({}, this.cachedMainReportParam, {
                    PageIndex: pagingOption.PageIndex,
                    PageSize: pagingOption.PageSize
                });

                this.setState({
                    userName: param.UserName,
                    sortBy: sortByMonthOnMonthView.Month,
                    isSortAscending: false
                }, callback);
            }
        }
    }

    _isMainView() {
        return this.props.currentViewLevel === viewLevel.main;
    }

    handlePressEnter = (event) => {
        if (event.charCode === 13) {
            event.preventDefault();

            this.handleClickSearch();
        }
    }

    handleSelectFromMonth = (month, year) => {
        const fromMonth = { month, year };
        if (!this._isMainView()) {
            this.setState({ fromMonth });
            return;
        }

        const toMonthRange = this._getToMonthRange(fromMonth);

        const { min, max } = toMonthRange;
        const { toMonth } = this.state;

        const isInRange = min.month <= toMonth.month && min.year <= toMonth.year &&
            max.month >= toMonth.month && max.year >= toMonth.year;

        const newState = isInRange ? {
            fromMonth: { month, year }
        } :
            {
                fromMonth: { month, year },
                toMonth: { month, year }
            }

        this.setState(newState);
    }

    handleSelectToMonth = (month, year) => {
        this.setState({ toMonth: { month, year } });
    }

    handleChangeCurrency = (item, selectedCurrencyList) => {
        this.setState({ selectedCurrencyList });

        this.props.setCurrencyList(handleChangeMultipleSelected(this.props.currencyList, selectedCurrencyList));
    }

    handleChangePaymentCurrency = (item, selectedPaymentCurrencyList) => {
        this.setState({ selectedPaymentCurrencyList });

        this.props.setPaymentCurrencyList(handleChangeMultipleSelected(this.props.paymentCurrencyList, selectedPaymentCurrencyList));
    }

    handleChangeCommGroup = (item, selectedCommGroupList) => {
        this.setState({ selectedCommGroupList });

        this.props.setCommGroupList(handleChangeMultipleSelected(this.props.commGroupList, selectedCommGroupList));
    }

    handleChangeProduct = (item, selectedProductList) => {
        this.setState({ selectedProductList });

        this.props.setProductList(handleChangeMultipleSelected(this.props.productList, selectedProductList));
    }

    handleChangeSortAscending = () => {
        this.setState((prevState) => ({
            isSortAscending: !prevState.isSortAscending
        }))
    }

    handleChange = (event) => {
        const { name, value } = event.target;

        this.setState({ [name]: value });
    }

    handleChangeGetAll = () => {
        this.setState((prevState) => ({
            isGetAll: !prevState.isGetAll
        }))
    }

    handleClickSearch = () => {
        const { selectedProductList } = this.state;

        if (selectedProductList === '') {
            return;
        }

        this._updateReportTitle();
        const { selectedLanguage, selectedCommGroupList, selectedBaseCurrency, selectedCurrencyList, selectedPaymentCurrencyList, fromMonth, toMonth, sortBy, isSortAscending, userName, isGetAll } = this.state;
        const { pagingOption } = this.props;

        const isSelectAllCommGroup = this.props.commGroupList.every(c => c.checked);

        const param = {
            UserName: userName,
            FromDate: `${fromMonth.month}/1/${fromMonth.year}`,
            ToDate: `${toMonth.month}/1/${toMonth.year}`,
            PreferredLanguage: selectedLanguage,
            BaseCurrencyId: Number(selectedBaseCurrency),
            GroupIds: isSelectAllCommGroup ? 'all' : selectedCommGroupList,
            CurrencyIds: selectedCurrencyList,
            PaymentCurrencyIds: selectedPaymentCurrencyList,
            ProductIds: selectedProductList,
            SortType: Number(sortBy),
            SortDirectionType: isSortAscending ? sortDirectionType.ascending : sortDirectionType.descending,
            PageIndex: 1,
            PageSize: pagingOption.PageSize,
            IsGetAll: isGetAll
        }

        if (this._isMainView()) {
            this.cachedMainReportParam = Object.assign({}, param);
        }

        this.props.setParam(param);
    }

    _updateReportTitle() {
        const { productList } = this.props;
        const isCheckedAllProduct = productList.every(p => p.checked);

        const reportTitle = this._isMainView() ? this._buildMainReportTitle(isCheckedAllProduct) : this._buildMonthOnMonthReportTile(isCheckedAllProduct);

        this.props.setReportTitle(reportTitle);
    }

    _buildMainReportTitle(isCheckedAllProduct) {
        const { productList, languageList, baseCurrencyList, commGroupList, paymentCurrencyList } = this.props;
        const { selectedLanguage, fromMonth, toMonth, userName, selectedBaseCurrency } = this.state;

        const userNameTitle = `${userName && ' - Affiliate contains \'' + userName + '\''}`;
        const language = languageList[selectedLanguage] || 'All languages';
        const commGroup = commGroupList.every(c => c.checked) ? 'All Revenue Group' : commGroupList.filter(c => c.checked).map(c => c.name).join(',');
        const paymentCurrency = paymentCurrencyList.every(c => c.checked) ? 'All Linked Acc. Currencies' : paymentCurrencyList.filter(c => c.checked).map(c => c.name).join(',');

        const productName = isCheckedAllProduct ? 'All Products' : productList.filter(p => p.checked).map(p => p.name).join(',');

        const fromToTitle = `FROM ${formatUnit(fromMonth.month)}/${fromMonth.year} TO ${formatUnit(toMonth.month)}/${toMonth.year}`;
        const baseCurrency = baseCurrencyList.filter(x => x.ExchangeId == selectedBaseCurrency).map(b => b.Code).join(',');

        return `${resource.reportName}${userNameTitle} - Base Currency ${baseCurrency} - ${paymentCurrency} - ${language} - ${productName} - ${commGroup} - ${fromToTitle}`;
    }

    _buildMonthOnMonthReportTile(isCheckedAllProduct) {
        const { productList } = this.props;
        const { fromMonth, toMonth, userName, isGetAll, selectedBaseCurrency } = this.state;

        const userNameTitle = userName && resource.space + userName;
        const productName = isCheckedAllProduct ? 'All Products' : productList.filter(p => p.checked).map(p => p.name).join(',');

        const fromToTitle = isGetAll ? 'Get All Dates' :
            `FROM ${formatUnit(fromMonth.month)}/${fromMonth.year} TO ${formatUnit(toMonth.month)}/${toMonth.year}`;
        const baseCurrency = this.props.baseCurrencyList.filter(x => x.ExchangeId == selectedBaseCurrency).map(b => b.Code).join(',');

        return `${resource.reportDetailName}${userNameTitle} - Base Currency ${baseCurrency} - ${productName} - ${fromToTitle}`;
    }

    _getToMonthRange(from) {
        if (!this._isMainView()) {
            return {
                min: Object.assign({}, from),
                max: { month: this.now.getMonth() + 1, year: this.now.getFullYear() }
            };
        }

        let min = Object.assign({}, from);
        let maxToMonth = from.month + 5;
        let maxToYear = maxToMonth > 12 ? from.year + 1 : from.year;

        let actualMaxMonth = maxToMonth % 12;

        const nowMonth = this.now.getMonth() + 1;
        const nowYear = this.now.getFullYear();

        if (maxToYear > nowYear) {
            actualMaxMonth = nowMonth;
            maxToYear = nowYear;
        }

        if (maxToMonth > nowMonth && maxToYear === nowYear) {
            actualMaxMonth = min.year < nowYear ? actualMaxMonth : nowMonth;
            maxToYear = nowYear;
        }

        return {
            min,
            max: { month: actualMaxMonth || 12, year: maxToYear }
        };
    }

    render() {
        const { selectedLanguage, selectedBaseCurrency, fromMonth, toMonth, sortBy, isSortAscending, selectedProductList, userName, isGetAll } = this.state;
        const { productList, languageList, currencyList, baseCurrencyList, paymentCurrencyList, isProduction, commGroupList } = this.props;

        const isValidSelectedProduct = !!selectedProductList;

        const toMonthRange = this._getToMonthRange(fromMonth);
        const isMainView = this._isMainView();
        const sortByData = isMainView ? sortByMainView : sortByMonthOnMonthView;

        return (
            <div className="filter-container">
                <Form inline tag="div">
                    <FormGroup className="my-sm-1 mr-sm-2">
                        <Label for="Affiliate" className="mr-sm-2">Affiliate</Label>
                        <Input type="text" name="userName" id="Affiliate" maxLength="30"
                            onKeyPress={this.handlePressEnter}
                            onChange={this.handleChange}
                            value={userName} />
                    </FormGroup>
                    <FormGroup className="my-sm-1 mr-sm-2">
                        <Label className="mr-sm-2">{globalResource.BaseCurrency}</Label>
                        <Input type="select" name="selectedBaseCurrency" id="BaseCurrency" value={selectedBaseCurrency} onChange={this.handleChange}>
                            {
                                baseCurrencyList.map(c => {
                                    return (<option key={c.ExchangeId} value={c.ExchangeId}>{c.Code}</option>)
                                })
                            }
                            {
                                !isProduction && <option value="20">UUS</option>
                            }
                        </Input>
                    </FormGroup>
                    {
                        isMainView && <FormGroup className="my-sm-1 mr-sm-2 multiple-currency-filter">
                            <Label className="mr-sm-2">Linked Acc. Currency</Label>
                            <MultipleSelect
                                dataSource={paymentCurrencyList}
                                keyField="key"
                                valueField="name"
                                statusField="checked"
                                maxDisplayItemCount={1}
                                noneSelectedLabel="None Select"
                                selectedAllLabel="All"
                                onChange={this.handleChangePaymentCurrency} />
                        </FormGroup>
                    }
                    {
                        !isProduction &&
                        <FormGroup className="my-sm-1 mr-sm-2 multiple-currency-filter">
                            <Label className="mr-sm-2">{globalResource.Currency}</Label>
                            <MultipleSelect
                                dataSource={currencyList}
                                keyField="key"
                                valueField="name"
                                statusField="checked"
                                maxDisplayItemCount={1}
                                noneSelectedLabel="None Select"
                                selectedAllLabel="All"
                                onChange={this.handleChangeCurrency} />
                        </FormGroup>
                    }
                    {
                        isMainView &&
                        <FormGroup className="my-sm-1 mr-sm-2">
                            <Input type="select" name="selectedLanguage" value={selectedLanguage} onChange={this.handleChange}>
                                <option key="lang-all" value="">All Language</option>
                                {
                                    Object.keys(languageList).map(lang => {
                                        return (<option key={'lang-' + lang} value={lang}>{languageList[lang]}</option>)
                                    })
                                }
                            </Input>
                        </FormGroup>
                    }
                    <FormGroup className={"my-sm-1 mr-sm-2" + `${isValidSelectedProduct ? '' : ' multiple-select-wrapper is-invalid'}`}
                        style={{ width: 200 }}>
                        <MultipleSelect
                            dataSource={productList}
                            keyField="key"
                            valueField="name"
                            statusField="checked"
                            maxDisplayItemCount={1}
                            hasInputOption={true}
                            noneSelectedLabel="Please Select"
                            selectedAllLabel="All Products"
                            onChange={this.handleChangeProduct} />
                        <div className="mx-1 rounded-circle note-info" title="Filtering product affects only to Turnover and GGR columns. Others always show value for all products">
                            <span className="oi oi-info"></span>
                        </div>
                    </FormGroup>
                    {
                        isMainView &&
                        <FormGroup className="my-sm-1 mr-sm-2 comm-group">
                            <MultipleSelect
                                dataSource={commGroupList}
                                keyField="key"
                                valueField="name"
                                statusField="checked"
                                maxDisplayItemCount={1}
                                noneSelectedLabel="None Select"
                                selectedAllLabel="All Revenue Groups"
                                onChange={this.handleChangeCommGroup} />
                        </FormGroup>
                    }
                    <FormGroup className="my-sm-1 mr-sm-2">
                        <Label className="mr-sm-2">From</Label>
                        <MonthPicker minMonth={minMonthDate.month} minYear={minMonthDate.year}
                            selectedDropdownYear={fromMonth.year}
                            selectedMonth={fromMonth.month}
                            onSelect={this.handleSelectFromMonth}
                            iconElement={(<span className="oi oi-calendar icon-calendar ml-1"></span>)}
                            enable={!isGetAll} />
                    </FormGroup>
                    <FormGroup className="my-sm-1 mr-sm-2">
                        <Label className="mr-sm-2">To</Label>
                        <MonthPicker minMonth={toMonthRange.min.month} minYear={toMonthRange.min.year}
                            maxMonth={toMonthRange.max.month} maxYear={toMonthRange.max.year}
                            selectedDropdownYear={toMonth.year}
                            selectedYear={toMonth.year}
                            selectedMonth={toMonth.month}
                            onSelect={this.handleSelectToMonth}
                            iconElement={(<span className="oi oi-calendar icon-calendar ml-1"></span>)}
                            enable={!isGetAll} />
                    </FormGroup>
                    {
                        isMainView && (
                            <FormGroup check>
                                <Label check>
                                    Get All
                                    <Input className="ml-1 mr-2" type="checkbox" checked={isGetAll} onChange={this.handleChangeGetAll} />
                                </Label>
                            </FormGroup>
                        )
                    }
                    <FormGroup className="my-sm-1 mr-sm-2">
                        <Label for="SortBy" className="mr-sm-2">Sort By</Label>
                        <Input type="select" name="sortBy" id="SortBy" value={sortBy} onChange={this.handleChange}>
                            {
                                Object.keys(sortByData).map(k => {
                                    return (<option key={'sortBy-' + k} value={sortByData[k]}>{k}</option>);
                                })
                            }
                        </Input>
                    </FormGroup>
                    <FormGroup className="my-sm-1 mr-sm-2">
                        <Label check>
                            A-Z
                            <Input className="ml-1" type="checkbox" checked={isSortAscending} onChange={this.handleChangeSortAscending} />
                        </Label>
                    </FormGroup>
                    <Button outline color="primary" type="button" onClick={this.handleClickSearch} disabled={!isValidSelectedProduct}>
                        <span className="oi oi-magnifying-glass"></span> Search
                    </Button>
                </Form>
            </div>
        );
    }
}

function mapStateToProps(state) {
    return Object.assign({}, {
        productList: state.productList,
        pagingOption: state.pagingOption,
        languageList: state.languageList,
        currencyList: state.currencyList,
        baseCurrencyList: state.baseCurrencyList,
        paymentCurrencyList: state.paymentCurrencyList,
        isProduction: state.isProduction,
        today: state.today,
        currentViewLevel: state.currentViewLevel,
        param: state.param,
        commGroupList: state.commGroupList
    });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setReportTitle, setParam, setCurrencyList, setCommGroupList, setProductList, setPaymentCurrencyList }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Filter);