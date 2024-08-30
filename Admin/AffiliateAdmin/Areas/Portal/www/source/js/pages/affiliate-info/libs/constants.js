import { globalResource } from '../../../modules/globalResource';

export { globalResource };

export const statusList = {
    Favorite: "Favorite",
    Pending: "Pending",
    Approved: "Approved",
    Disapproved: "Disapproved",
    Suspended: "Suspended"
}

export const defaultSortBy = 1;

export const defaultSelectedColumnNumber = 4;
export const allCommGroups = 'all';

export const sortByList = {
    'Username': 1,
    'Registration Date': 2,
    'Approval Date': 3,
    'Balance B/F': 4,
    'Player': 5
}

export const defaultColumns = ['Index', 'ParentAffName', 'UserName', 'Password'];

export const headerText = {
    Index: '#',
    ParentAffName: 'Parent Affiliate',
    UserName: 'Username',
    AffCode: 'Affiliate Code',
    Status: 'Status',
    FirstName: 'First Name',
    LastName: 'Last Name',
    Language: 'Language',
    Phone: 'Mobile Phone',
    Email: 'Email',
    RegistrationDateText: 'Registration Date',
    ApprovalDateText: 'Approval Date',
    ApprovedByText: 'Approved By',
    CommissionGroup: 'Revenue Group',
    BalanceBf: 'Balance B/F',
    Currency: 'Aff. Currency',
    PlayerUserName: 'Aff. Player',
    PlayerCurrency: 'Aff. Player Currency',
    Member: 'Player',
    NoOfSubAff: 'Sub-Affiliate',
    Password: 'Password'
}

export const minWidthOfColumns = {
    AffCode: 75,
    Status: 75,
    FirstName: 75,
    LastName: 75,
    Language: 75,
    Phone: 100,
    Email: 175,
    RegistrationDateText: 100,
    ApprovalDateText: 100,
    ApprovedByText: 100,
    CommissionGroup: 100,
    BalanceBf: 100,
    Currency: 75,
    PlayerUserName: 100,
    PlayerCurrency: 75,
    Member: 50,
    NoOfSubAff: 90,
}

export const resource = {
    Search: globalResource.Search,
    Space: ' - ',
    ReportName: 'AFFILIATE REPORT',
    AllCommGroups: 'All Revenue Groups',
    AllLanguages: 'All Languages',
    AllStatuses: 'All Statuses',
    UsernameContains: 'USERNAME CONTAINS ',
    EmailContains: 'EMAIL CONTAINS ',
    AffCodeContains: 'AFFILIATE CODE ',
    AffPlayerContains: 'AFFILIATE PLAYER CONTAINS ',
    SearchBy: 'Search By',
    SortBy: 'Sort By',
    AZ: 'A-Z',
    Apostrophe: "'",
    NA: 'N/A',
    DisplayedColumns: 'Displayed Columns'
}

export const regExp = {
    CheckAffCode: "(^[1-9])(\\d*)$"
}

export const defaultState = {
    selectedCommGroup: 0,
    selectedLanguage: '',
    selectedStatus: '',
    parentAffName: '',
    userName: '',
    email: '',
    affCode: '',
    affPlayer: '',
    selectedSortBy: defaultSortBy,
    isSortAscending: true
}