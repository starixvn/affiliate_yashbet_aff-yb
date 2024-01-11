import React, { Component, createRef } from "react";

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import GridData from './GridData';
import ReportTitle from '../../../../components/ReportTitle';

import { setViewLevel, fetchData } from '../actions';
import { toggleLoader } from '../../../../actions/loaderAction';
import { viewLevel } from '../libs/constants';
import ReportExcel from '../libs/reportExcel';

export class Content extends Component {
    constructor(props) {
        super(props);

        this.gridDataRef = createRef();
    }

    shouldComponentUpdate(nextProps) {
        const { reportTitle } = this.props;

        return nextProps.reportTitle !== reportTitle;
    }

    handleClickBreadcrumb = () => {
        this.gridDataRef.current._resetScroll();
        this.props.setViewLevel(viewLevel.main);
    }

    handleExportExcel = async () => {
        this.props.toggleLoader(true);
        const { param, reportTitle, currentViewLevel } = this.props;
        const isMainView = currentViewLevel === viewLevel.main;
        const newParam = Object.assign({}, param, { IsExportExcel: true });
        const data = await this.props.fetchData(newParam, isMainView);

        if (data) {
            const reportExcel = new ReportExcel(data, reportTitle, isMainView);
            reportExcel.export();
        }
        this.props.toggleLoader(false);
    }

    _buildReportTitle() {
        const { currentViewLevel, reportTitle } = this.props;
        const isMainView = currentViewLevel === viewLevel.main;

        return isMainView ? (
            <div className="d-flex align-items-center">
                {reportTitle}
                <button type="button" className="ml-auto btn btn-link btn-excel" onClick={this.handleExportExcel} title="Export Excel">
                    <span className="oi icon-excel"></span>
                </button>
            </div>
        ) : (
            <div className="d-flex align-items-center">
                <a href="javascript:;" className="btn-link font-weight-bold" onClick={this.handleClickBreadcrumb}>AFFILIATE REPORT</a>&nbsp;&gt; {reportTitle}
                <button type="button" className="ml-auto btn btn-link btn-excel" onClick={this.handleExportExcel} title="Export Excel">
                    <span className="oi icon-excel"></span>
                </button>
            </div>
        )
    }

    render() {
        const { reportTitle, isYB } = this.props;

        return (
            <div className="data-content my-2 report-content-container">
                {
                    reportTitle && (
                        <React.Fragment>
                            {
                                isYB && (
                                    <p className="affiliate-report-note">
                                        Notes: First Comm Rate and Earning will calculate at 5th July 2020
                                    </p>
                                )
                            }
                            <ReportTitle content={this._buildReportTitle()} />
                        </React.Fragment>
                    )
                }
                <GridData ref={this.gridDataRef} />
            </div>
        );
    }
}

function mapStateToProps({ reportTitle, currentViewLevel, param, isYB }) {
    return Object.assign({}, { reportTitle, currentViewLevel, param, isYB });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setViewLevel, toggleLoader, fetchData }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Content);