import { formatDecimal, format } from "../../../modules/numberFormat";
import { globalResource } from '../../../modules/globalResource';

export { globalResource, formatDecimal, format };

export const INFINITY_AM = 100000000000;
export const INFINITY_QM = 1000000000;
export const INFINITY_SYMBOL = '∞';
export const FIRST_ITEM_INDEX = 0;
export const ONLY_ONE_ITEM = 1;

function generateCommissionValues() {
    let min = 0,
        max = 99.9;

    const commissionList = { 0.0: '0.0%'};

    while (min < max) {
        min = min + 0.1;
        commissionList[format(min/100, 3)] = format(min, 1) + "%";
    }

    return commissionList;
}
const _commValueList = generateCommissionValues();

export const commissionValueList = _commValueList;

function generateSubAffCommRateValues() {
    let min = 10.0,
        max = 20.0;

    const subAffCommRateList = { 0.1: '10.0%' };

    while (min < max) {
        min = min + 10;
        subAffCommRateList[format(min / 100, 3)] = format(min, 1) + "%";
    }

    return subAffCommRateList;
}
const _subAffCommRateValueList = generateSubAffCommRateValues();

export const subAffCommRateValueList = _subAffCommRateValueList;
export const defaultSubAffCommRate = 0.1;

export const GGR = '| Gross Game Revenue |'

export const messages = {
    requireGroupName: 'Group Name cannot be empty. Please check!',
    requireOperationalAmount: 'Operational Fee cannot be empty. Please check!',
    invalidOperationalAmount: 'Operational Fee is invalid.',
    requireDepositAmount: 'Deposit Fee cannot be empty. Please check!',
    invalidDepositAmount: 'Deposit Fee is invalid.',
    requireWithdrawalAmount: 'Withdrawal Fee cannot be empty. Please check!',
    invalidWithdrawalAmount: 'Withdrawal Fee is invalid.',
    requireTier: 'Tier Setting must have at least 1 tier',
    requireInfinity: 'Last Tier Setting must have maximum value=∞',
    amountConstrain: 'Min Net Win must be lesser than or equal to 1,000,000,000!',
    qmConstrain: 'Min Active Player must be lesser than or equal to 100,000,000,000!',
    requireSelectedProduct: 'At least 1 product must be selected per group',
    confirmChangeGroup: 'Are you sure you want to change the Revenue Group for affiliate: {0}',
    choseAnotherGroup: 'Please choose another group (not old group)!',
    assignGroupSuccessfully: 'Assign affiliate to Revenue Group successfully',
    addGroupSuccessfully: 'Add Revenue Group Successfully',
    updateGroupSuccessfully: 'Update Revenue Group Successfully',
    note: "Note: Change will take effect on current month (GMT-4)."
}

const _deductionNameList = {
    p2pDeduction: 'P2P Fee',
    platformFee: 'Platform Fee',
    depositFee: 'Deposit Fee',
    withdrawalFee: 'Withdrawal Fee'
}
export const deductionNameList = _deductionNameList;

export const regExp = {
    checkAmount: "^(\\d+\\.?\\d*)?$",
    shouldFormatAmount: "^\\d+$",
    validAmount: "^\\d+(\\.\\d{1,})?$"
}