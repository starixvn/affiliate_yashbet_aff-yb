import { deductionNameList, messages, INFINITY_AM, INFINITY_QM, defaultSubAffCommRate, regExp } from './constant';

const emptyCommGroup = {
    GroupId: 0,
    GroupName: '',
    Description: '',
    SubAffCommRate: defaultSubAffCommRate,
    IsActivePlayerSum: true,
    TierSettingList: [],
    PlatformFeeList: [],
    DepositFee: {},
    WithdrawalFee: {}
}

export const buildDefaultCommGroup = (deductionList, productList, p2pProductList) => {
    const deductionSetting = getDeductionSetting(deductionList);

    const p2pProductParam = { p2pProductList, deductionSetting };
    const p2pDeductionList = buildP2pDeductionList(p2pProductParam);

    const p2pProductIds = p2pProductList.map(i => i.ProductId);
    const platformFeeProductList = productList.filter(i => !p2pProductIds.includes(i.ProductId));
    const platformFeeParam = { platformFeeProductList, deductionSetting };
    const platformFeeList = buildPlatformFeeList(platformFeeParam);

    const depositFee = createDefaultDepositSetting(deductionSetting.depositFeeSetting);
    const withdrawalFee = createDefaultWithdrawalSetting(deductionSetting.withdrawalFeeSetting);

    return Object.assign({}, emptyCommGroup, {
        P2pDeductionList: p2pDeductionList,
        PlatformFeeList: platformFeeList,
        DepositFee: depositFee,
        WithdrawalFee: withdrawalFee
    });
}

export const getDeductionSetting = (deductionList) => {
    const deductionObj = convertDeductionList(deductionList);
    const p2pDeductionSetting = deductionObj[deductionNameList.p2pDeduction];
    const platformFeeSetting = deductionObj[deductionNameList.platformFee];
    const depositFeeSetting = deductionObj[deductionNameList.depositFee];
    const withdrawalFeeSetting = deductionObj[deductionNameList.withdrawalFee];

    return {
        p2pDeductionSetting,
        platformFeeSetting,
        depositFeeSetting,
        withdrawalFeeSetting
    }
}

export const createDefaultDepositSetting = (depositFeeSetting) => {
    return {
        DeductionId: depositFeeSetting.DeductionId,
        ProductIds: 0,
        DeductedPercentage: depositFeeSetting.DeductedPercentage,
        DeductedAmount: depositFeeSetting.DeductedAmount,
        IsDeductedPercent: depositFeeSetting.IsDeductedPercent
    }
}

export const createDefaultWithdrawalSetting = (withdrawalFeeSetting) => {
    return {
        DeductionId: withdrawalFeeSetting.DeductionId,
        ProductIds: 0,
        DeductedPercentage: withdrawalFeeSetting.DeductedPercentage,
        DeductedAmount: withdrawalFeeSetting.DeductedAmount,
        IsDeductedPercent: withdrawalFeeSetting.IsDeductedPercent
    }
}

export const buildP2pDeductionList = ({ p2pProductList, deductionSetting }) => {
    const { p2pDeductionSetting } = deductionSetting;

    const products = convertProductList(p2pProductList);
    return [{
        DeductionId: p2pDeductionSetting.DeductionId,
        ProductIds: products.map(p => p.ProductId).join(','),
        DeductedPercentage: p2pDeductionSetting.DeductedPercentage,
        Commission: p2pDeductionSetting.Commission,
    }];
}

export const buildPlatformFeeList = ({ platformFeeProductList, deductionSetting }) => {
    const { platformFeeSetting } = deductionSetting;

    const products = convertProductList(platformFeeProductList);
    return [{
        DeductionId: platformFeeSetting.DeductionId,
        ProductIds: products.map(p => p.ProductId).join(','),
        DeductedPercentage: platformFeeSetting.DeductedPercentage,
        DeductedAmount: platformFeeSetting.DeductedAmount,
        IsDeductedPercent: platformFeeSetting.IsDeductedPercent
    }];
}

export const createDefaultP2pDeduction = (p2pProductList, p2pDeductionSetting) => {
    return {
        deductionId: p2pDeductionSetting.DeductionId,
        productList: convertProductList(p2pProductList),
        deductedPercentage: p2pDeductionSetting.DeductedPercentage,
        commission: p2pDeductionSetting.Commission
    };
}

export const createDefaultPlatformfee = (productList, platformFeeSetting) => {
    return {
        deductionId: platformFeeSetting.DeductionId,
        productList: convertProductList(productList),
        commission: platformFeeSetting.DeductedPercentage
    };
}

export const convertDeductionList = (deductionList) => {
    const deductions = {};
    for (let item of deductionList) {
        deductions[item.DeductionName] = item;
    }

    return deductions;
}

export const convertProductList = (productList, productIds = '') => {
    const idList = productIds.split(',').filter(p => !!p);
    const isCheckAll = idList.length === productList.length;

    return productList.map(item => {
        return {
            ProductId: item.ProductId,
            ProductName: item.ProductName,
            Checked: isCheckAll || idList.filter(i => Number(i) === item.ProductId).length > 0
        }
    })
}

export const convertStringToArrayNumber = (stringIds) => {
    return stringIds.split(',').filter(id => !!id).map(i => Number(i));
}

export const validate = (selectedGroup) => {
    const { GroupName, TierSettingList, P2pDeductionList, PlatformFeeList, DepositFee, WithdrawalFee } = selectedGroup;

    const groupNameValidation = validateGroupName(GroupName);
    const tierSettingValidation = validateTierSettingList(TierSettingList);
    const selectedProductValidation = validateItemMustHaveAProduct(P2pDeductionList, PlatformFeeList);
    const depositFeeValidation = validateDeductedFees(DepositFee, messages.requireDepositAmount, messages.invalidDepositAmount);
    const withdrawalFeeValidation = validateDeductedFees(WithdrawalFee, messages.requireWithdrawalAmount, messages.invalidWithdrawalAmount);

    const isValid = groupNameValidation.isValid && tierSettingValidation.isValid && selectedProductValidation.isValid
        && depositFeeValidation.isValid && withdrawalFeeValidation.isValid;
    const messageList = [...groupNameValidation.messageList, ...tierSettingValidation.messageList, ...selectedProductValidation.messageList,
        ...depositFeeValidation.messageList, ...withdrawalFeeValidation.messageList
    ];

    return { isValid, messageList };
}

export const validateDeductedFees = (deductedFee, requiredMessage, invalidMessage) => {
    let isValid = true;
    const messageList = [];

    if (!deductedFee.IsDeductedPercent) {
        let deductionAmount = deductedFee.DeductedAmount && deductedFee.DeductedAmount.replace(/,/g, '');
        if (!deductionAmount) {
            isValid = false;
            messageList.push(requiredMessage);
        } else {
            const regexValidAmount = new RegExp(regExp.validAmount);
            if (!regexValidAmount.test(deductionAmount)) {
                isValid = false;
                messageList.push(invalidMessage);
            }
        }
    }

    return { isValid, messageList };
}

export const validateGroupName = (groupName) => {
    let isValid = true;
    const messageList = [];

    if (!groupName) {
        isValid = false;
        messageList.push(messages.requireGroupName);
    }

    return { isValid, messageList };
}

export const validateTierSettingList = (tierSettingList) => {
    let isValid = true;
    const messageList = [];

    if (tierSettingList.length === 0) {
        isValid = false;
        messageList.push(messages.requireTier);
    } else if (tierSettingList.filter(item => item.MinAmount > INFINITY_AM).length > 0) {
        isValid = false;
        messageList.push(messages.amountConstrain);
    } else if (tierSettingList.filter(item => item.MinAp > INFINITY_QM).length > 0) {
        isValid = false;
        messageList.push(messages.qmConstrain);
    }

    return { isValid, messageList };
}

export const validateItemMustHaveAProduct = (p2pDeducctionList, platformFeeList) => {
    let isValid = true;

    if ((p2pDeducctionList.length > 1 && p2pDeducctionList.filter(item => !item.ProductIds).length > 0)
        || (platformFeeList.length > 1 && platformFeeList.filter(item => !item.ProductIds).length > 0)) {
        isValid = false;
    }

    return { isValid, messageList: isValid ? [] : [messages.requireSelectedProduct] };
}

export const getSelectedProductIds = (deductionList) => {
    return deductionList.map(p => p.ProductIds)
        .map(ids => convertStringToArrayNumber(ids))
        .reduce((cul, cur) => [...cul, ...cur], []);
}

export const updatePlatfromFeeList = (selectedProductIdsOfP2p, platformFeeList, allProductIds) => {
    const feeIdListInArrayForm = platformFeeList.map(fee => convertStringToArrayNumber(fee.ProductIds));
    const newFeeListIds = getPlatformFeeListIds(selectedProductIdsOfP2p, feeIdListInArrayForm, allProductIds);
    const newPlatformFeeList = [];

    newFeeListIds.forEach((arrId, index) => {
        const platformFee = platformFeeList[index];
        const newplatformFee = Object.assign({}, platformFee, { ProductIds: arrId.join(',') })
        newPlatformFeeList.push(newplatformFee);
    });

    return newPlatformFeeList;
}

export const removeTierItem = (tierId, groupProductId, dataList) => {
    return dataList.filter(item => !(item.TierId === tierId && item.GroupProductId === groupProductId));
}

const getPlatformFeeListIds = (selectedProductIdsOfP2p, prePlatformFeeIds, allProductIds) => {
    const preSelProductIds = prePlatformFeeIds.reduce((acc, cur) => acc.concat(cur), []);
    const additionIds = getAdditionId(selectedProductIdsOfP2p, preSelProductIds, allProductIds);
    const platformFeeIdsAfterRemoveProduct = removeCheckedProductId(selectedProductIdsOfP2p, prePlatformFeeIds);
    const firstPlatformFeeProductIds = platformFeeIdsAfterRemoveProduct[0];
    additionIds.length && firstPlatformFeeProductIds.push(additionIds);

    return [firstPlatformFeeProductIds, ...platformFeeIdsAfterRemoveProduct.slice(1)]
}

const getAdditionId = (curIds, preSelIds, allProductIds) => {
    return allProductIds.filter(i => !(curIds.find(c => c ===i) || preSelIds.find(t => t === i)));
}

const removeCheckedProductId = (curSelIds, preListIds) => {
    const newList = [];
    preListIds.forEach(list => {
        newList.push(list.filter(i => !curSelIds.find(p => p === i)));
    })
    return newList;
}