import { defaultBaseCurrency, minMonthDate } from '../../../../modules/constants';

export { defaultBaseCurrency, minMonthDate };

const sortByCommon = {
    Turnover: 2,
    GGR: 3,
    Earning: 4
}

export const defaultSortBy = 1;
export const sortByMainView = Object.assign({}, {
    Affiliate: 1
}, sortByCommon);
export const sortByMonthOnMonthView = Object.assign({}, {
    Month: 1
}, sortByCommon);

export const viewLevel = {
    main: 1,
    monthOnMonth: 2
}

export const sortDirectionType = {
    ascending: 0,
    descending: 1
}

export const totalText = 'Total (all pages)';

export const resource = {
    reportName: 'AFFILIATE REPORT',
    reportDetailName: 'MONTH ON MONTH',
    space: ' - '
}

export const headerText = {
    UserName: 'Affiliate',
    MonthDateString: 'Month',
    PaymentCurrency: 'Linked Account Currency',
    TotalClick: 'Total Click',
    UniqueClick: 'Unique Click',
    Signup: 'Signup',
    Fdm: 'FDM',
    ActiveMember: 'Active Player',
    NoOfSubAff: 'Sub-Affiliate',
    Deposit: 'Deposit',
    Withdrawal: 'Withdrawal',
    Turnover: 'Turnover',
    Ggr: 'GGR',
    Bonus: 'Bonus',
    Rebate: 'Rebate',
    PlatformFee: 'Royalty Fee',
    DepositFee: 'Deposit Fee',
    WithdrawalFee: 'Withdrawal Fee',
    TotalFee: 'Total Fee',
    CommissionRate: 'Share%',
    Earning: 'Earning',
    P2pEarning: 'P2P Earning',
    SubAffEarning: 'Sub-Affiliate Earning',
    TotalEarning: 'Total Earning',
    AffWithdrawal: 'Withdrawal',
    AffAdjustment: 'Adjustment',
    BalanceBf: 'Balance B/F',
    MemberStatistics: 'Player Statistics',
    AffiliateStatistics: 'Affiliate Statistics',
    MonthOnMonth: 'Month on Month',
    PlayerGgr: 'Player GGR'
}