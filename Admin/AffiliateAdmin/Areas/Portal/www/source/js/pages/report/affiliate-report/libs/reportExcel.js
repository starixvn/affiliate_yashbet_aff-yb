import Excel from '../../../../modules/excel';
import { titleCellStyle, headerCellStyle, bodyCellStyle, totalCellStyle, mergedHeaderCellByRowspanStyle } from '../../../../modules/excelStyleConstant';
import { headerText, totalText, resource } from './constants';
import { formatDecimal } from '../../../../modules/numberFormat';
import { formatDateTime } from '../../../../modules/utils';

const TITLE_ROW_INDEX = 0;
const HEADER_ROW_INDEX = 1;
const DATA_ROW_INDEX = 3;

const MAIN_VIEW = {
    TOTAL_COLUMN: 26,
    INDEX_PLAYER_STATISTICS_COLUMN: 8,
    INDEX_AFFILIATE_STATISTICS_COLUMN: 9,
    INDEX_DEPOSIT_COLUMN: 8,
    INDEX_COMM_COLUMN: 18,
    COLSPAN_PLAYER_STATISTICS: 10,
    COLSPAN_AFFILIATE_STATISTICS: 8
}

const DETAIL_VIEW = {
    TOTAL_COLUMN: 24,
    INDEX_PLAYER_STATISTICS_COLUMN: 7,
    INDEX_AFFILIATE_STATISTICS_COLUMN: 8,
    INDEX_DEPOSIT_COLUMN: 7,
    INDEX_COMM_COLUMN: 17,
    COLSPAN_PLAYER_STATISTICS: 10,
    COLSPAN_AFFILIATE_STATISTICS: 7
}

const MAIN_SECTION_STYLE = 'mainSectionStyle';
const MULTI_COMM_GROUP_SECTION_STYLE = 'multiCommGroupSectionStyle';
const TOTAL_SECTION_STYLE = 'totalSectionStyle';

const fields = {
    UserName: 'UserName',
    Description: 'Description',
    MonthDateString: 'MonthDateString',
    PaymentCurrency: 'PaymentCurrency',
    TotalClick: 'TotalClick',
    UniqueClick: 'UniqueClick',
    Signup: 'Signup',
    Fdm: 'Fdm',
    ActiveMember: 'ActiveMember',
    NoOfSubAff: 'NoOfSubAff',
    Deposit: 'Deposit',
    Withdrawal: 'Withdrawal',
    Turnover: 'Turnover',
    Ggr: 'Ggr',
    Bonus: 'Bonus',
    Rebate: 'Rebate',
    PlatformFee: 'PlatformFee',
    DepositFee: 'DepositFee',
    WithdrawalFee: 'WithdrawalFee',
    TotalFee: 'TotalFee',
    CommissionRate: 'CommissionRate',
    Earning: 'Earning',
    P2pEarning: 'P2pEarning',
    SubAffEarning: 'SubAffEarning',
    TotalEarning: 'TotalEarning',
    AffWithdrawal: 'AffWithdrawal',
    AffAdjustment: 'AffAdjustment',
    BalanceBf: 'BalanceBf'
}

export default class ReportExcel {
    constructor(data, title, isMainView) {
        this.worksheet = {};
        this.title = isMainView ? title : resource.reportName + ' > ' + title;
        this.data = data;
        this.isMainView = isMainView;

        this.currentRowIndex = DATA_ROW_INDEX;
        this.mergeConfig = [];
        this.columnConfigs = this._getcolumnConfigs(isMainView);
        this.viewInfo = isMainView ? MAIN_VIEW : DETAIL_VIEW;
    }

    export() {
        Excel.process(() => {
            this._createTitle();
            this._createHeader();
            this._createBodyData();
            this._createTotalRow();
            this._createExcelConfig();

            const fileName = resource.reportName + (this.isMainView ? '' : resource.space + resource.reportDetailName) + resource.space + formatDateTime(new Date(), 'MM.dd.yyyy hh.mm.ss');
            Excel.export(this.worksheet, fileName);
        })
    }

    _createTitle() {
        this.mergeConfig.push({
            s: {
                r: TITLE_ROW_INDEX,
                c: 0
            },
            e: {
                r: TITLE_ROW_INDEX,
                c: this.viewInfo.TOTAL_COLUMN - 1
            }
        });
        const address = Excel.getCellAddress({
            c: 0,
            r: TITLE_ROW_INDEX
        });
        this.worksheet[address] = {
            t: "s",
            v: this.title,
            s: titleCellStyle
        }
    }

    _createHeader() {
        const colspanPlayerStatistics = this.viewInfo.COLSPAN_PLAYER_STATISTICS;
        const colspanAffiliateStatistics = this.viewInfo.COLSPAN_AFFILIATE_STATISTICS;
        const length = this.columnConfigs.length - (colspanPlayerStatistics - 1) - (colspanAffiliateStatistics - 1);
        for (let i = 0; i < length; i++) {
            if (i === this.viewInfo.INDEX_PLAYER_STATISTICS_COLUMN) {
                this._buildColspanColumn(HEADER_ROW_INDEX, this.viewInfo.INDEX_DEPOSIT_COLUMN, colspanPlayerStatistics, headerText.MemberStatistics);
            } else if (i === this.viewInfo.INDEX_AFFILIATE_STATISTICS_COLUMN) {
                this._buildColspanColumn(HEADER_ROW_INDEX, this.viewInfo.INDEX_COMM_COLUMN, colspanAffiliateStatistics, headerText.AffiliateStatistics);
            } else {
                const fieldName = this.columnConfigs[i].field;
                let address = Excel.getCellAddress({
                    c: i,
                    r: HEADER_ROW_INDEX
                });
                this.worksheet[address] = {
                    t: "s",
                    v: headerText[fieldName],
                    s: headerCellStyle
                }

                let nextRowAddress = Excel.getCellAddress({
                    c: i,
                    r: HEADER_ROW_INDEX + 1
                });
                this.worksheet[nextRowAddress] = {
                    t: "s",
                    s: mergedHeaderCellByRowspanStyle
                }

                this.mergeConfig.push({
                    s: {
                        r: HEADER_ROW_INDEX,
                        c: i
                    },
                    e: {
                        r: HEADER_ROW_INDEX + 1,
                        c: i
                    }
                });
            }
        }
    }

    _buildColspanColumn(rowIndex, colIndex, colSpan, colSpanheaderText) {
        const colSpanCellAddress = Excel.getCellAddress({
            c: colIndex,
            r: rowIndex
        });
        this.worksheet[colSpanCellAddress] = {
            t: "s",
            v: colSpanheaderText,
            s: headerCellStyle
        }

        const totalColumn = colIndex + colSpan;
        this.mergeConfig.push({
            s: {
                r: rowIndex,
                c: colIndex
            },
            e: {
                r: rowIndex,
                c: totalColumn - 1
            }
        });

        for (let i = colIndex; i < totalColumn; i++) {
            const address = Excel.getCellAddress({
                c: i,
                r: rowIndex + 1
            });

            const fieldName = this.columnConfigs[i].field;
            this.worksheet[address] = {
                t: "s",
                v: headerText[fieldName] || '',
                s: headerCellStyle
            }
        }
    }

    _createBodyData() {
        const mainData = this.data.MainData;

        for (let item of mainData) {
            item.CommissionRate = formatDecimal(item.CommissionRate) + '%';

            if (!this.isMainView) {
                const {
                    MonthDateString,
                    Description
                } = item;
                item.MonthDateString = Description ? `${MonthDateString} (${Description})` : MonthDateString;
            }

            const totalColumn = this.viewInfo.TOTAL_COLUMN;
            for (let j = 0; j < totalColumn; j++) {
                const address = Excel.getCellAddress({
                    c: j,
                    r: this.currentRowIndex
                });

                this.worksheet[address] = this._buildDataRow(item, MAIN_SECTION_STYLE, j);
            }

            this.currentRowIndex++;
        }
    }

    _createTotalRow() {
        const totalData = this.data.Total;
        if (totalData) {
            const customTotalData = this.isMainView ? {
                [fields.UserName]: totalText,
                [fields.PaymentCurrency]: '',
                [fields.CommissionRate]: "N/A"
            } : {
                [fields.MonthDateString]: totalText,
                [fields.CommissionRate]: "N/A"
            }
            const total = Object.assign({}, totalData, customTotalData);

            const totalColumn = this.viewInfo.TOTAL_COLUMN;
            for (let j = 0; j < totalColumn; j++) {
                const address = Excel.getCellAddress({
                    c: j,
                    r: this.currentRowIndex
                });

                this.worksheet[address] = this._buildDataRow(total, TOTAL_SECTION_STYLE, j);
            }
        }
    }

    _buildDataRow(item, sectionStyleName, colIndex) {
        const columnConfig = this.columnConfigs[colIndex];
        const value = item[columnConfig.field];

        return {
            t: Excel.getCellType(value),
            v: value,
            s: columnConfig.field == fields.CommissionRate && item.IsMultiGroupProduct ? columnConfig[MULTI_COMM_GROUP_SECTION_STYLE] : columnConfig[sectionStyleName]
        }
    }

    _createExcelConfig() {
        const cellStart = Excel.getCellAddress({
            r: 0,
            c: 0
        });
        const cellEnd = Excel.getCellAddress({
            r: this.currentRowIndex,
            c: this.viewInfo.TOTAL_COLUMN
        });

        this.worksheet["!merges"] = this.mergeConfig;
        this.worksheet["!ref"] = `${cellStart}:${cellEnd}`;
        this.worksheet['!cols'] = [{
            wpx: 150
        }, {
            wpx: 150
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        },
        {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 120
        },
        {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 140
        }, {
            wpx: 140
        }, {
            wpx: 120
        }, {
            wpx: 120
        }, {
            wpx: 150
        }, {
            wpx: 150
        }
        ];
    }

    _getcolumnConfigs(isMainView) {
        const columnConfigList = [{
            field: isMainView ? fields.UserName : fields.MonthDateString,
            [MAIN_SECTION_STYLE]: bodyCellStyle.left,
            [TOTAL_SECTION_STYLE]: totalCellStyle.default
        },
        {
            field: fields.PaymentCurrency,
            [MAIN_SECTION_STYLE]: bodyCellStyle.center,
            [TOTAL_SECTION_STYLE]: totalCellStyle.center
        },
        {
            field: fields.TotalClick,
            [MAIN_SECTION_STYLE]: bodyCellStyle.number,
            [TOTAL_SECTION_STYLE]: totalCellStyle.number
        },
        {
            field: fields.UniqueClick,
            [MAIN_SECTION_STYLE]: bodyCellStyle.number,
            [TOTAL_SECTION_STYLE]: totalCellStyle.number
        },
        {
            field: fields.Signup,
            [MAIN_SECTION_STYLE]: bodyCellStyle.number,
            [TOTAL_SECTION_STYLE]: totalCellStyle.number
        },
        {
            field: fields.Fdm,
            [MAIN_SECTION_STYLE]: bodyCellStyle.number,
            [TOTAL_SECTION_STYLE]: totalCellStyle.number
        },
        {
            field: fields.ActiveMember,
            [MAIN_SECTION_STYLE]: bodyCellStyle.number,
            [TOTAL_SECTION_STYLE]: totalCellStyle.number
        },
        {
            field: fields.NoOfSubAff,
            [MAIN_SECTION_STYLE]: bodyCellStyle.number,
            [TOTAL_SECTION_STYLE]: totalCellStyle.number
        },
        {
            field: fields.Deposit,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.Withdrawal,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.Turnover,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.Ggr,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.Bonus,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.Rebate,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.PlatformFee,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.DepositFee,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.WithdrawalFee,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.TotalFee,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.CommissionRate,
            [MAIN_SECTION_STYLE]: bodyCellStyle.right,
            [MULTI_COMM_GROUP_SECTION_STYLE]: bodyCellStyle.rightHighlight,
            [TOTAL_SECTION_STYLE]: totalCellStyle.right
        },
        {
            field: fields.Earning,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.P2pEarning,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.SubAffEarning,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.TotalEarning,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.AffWithdrawal,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.AffAdjustment,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        },
        {
            field: fields.BalanceBf,
            [MAIN_SECTION_STYLE]: bodyCellStyle.decimal,
            [TOTAL_SECTION_STYLE]: totalCellStyle.decimal
        }
        ];

        if (!isMainView) {
            columnConfigList.splice(1, 1);
            columnConfigList.splice(24, 1);
        }

        return columnConfigList;
    }
}