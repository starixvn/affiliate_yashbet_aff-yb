import {
    messageList,
    regexEmail,
    countriesList,
    requiredFieldList,
    requiredBankTranferList,
    playerMethod,
    moneybookersMethod,
    wireMethod
} from './constants'

const getMessage = (fieldName, value) => {
    return value === '' ? messageList[fieldName] + "<br />" : '';
}

const validateMoreSiteUrls = (moreSiteUrlsList) => {
    const moreSiteUrls = moreSiteUrlsList.map(siteUrl => siteUrl.trim());
    const emptySiteUrlIndex = moreSiteUrls.indexOf('');
    const hasEmptySiteUrl = emptySiteUrlIndex !== -1;
    const message = hasEmptySiteUrl ? `Website URL ${emptySiteUrlIndex + 2} is required<br />` : '';

    return {
        message,
        isValid: !hasEmptySiteUrl
    }
}

const validatePaymentMethod = (userAccountInfo) => {
    let message = '';
    let isValid = true;

    switch (userAccountInfo.PreferredPayment) {
        case playerMethod:
            message = getMessage('PlayerUsername', userAccountInfo.PlayerUsername);
            isValid = userAccountInfo.PlayerUsername !== '';
            break;
        case moneybookersMethod:
            message = getMessage('MoneybookersEmail', userAccountInfo.MoneybookersEmail);
            isValid = userAccountInfo.MoneybookersEmail !== '';
            break;
        case wireMethod:
            message = requiredBankTranferList.reduce((accumulator, field) => accumulator + getMessage(field, userAccountInfo[field]), '');
            isValid = userAccountInfo.BankName !== '' && userAccountInfo.BankAddress !== '' &&
                userAccountInfo.BankAccountName !== '' && userAccountInfo.BankAccountNumber !== '';
            break;
    }

    return {
        message,
        isValid
    };
}

export const validateUserAccount = (userAccountInfo, moreSiteUrlsList, licenseeName) => {
    let message = '';
    const checkEmail = new RegExp(regexEmail);
    const isValidGender = userAccountInfo.Gender != null;
    const isValidEmail = checkEmail.test(userAccountInfo.Email);
    const country = countriesList[userAccountInfo.Country] === undefined ? "" : userAccountInfo.Country;
    const selectedField = userAccountInfo.HasWebSite ? 'SiteUrl' : `NoSiteDescription${licenseeName}`;

    const newRequiredFieldList = [...requiredFieldList, selectedField];
    message = isValidGender ? message : message + messageList.Gender + "<br />";
    message = isValidEmail ? message : message + messageList.Email + "<br />";
    message += getMessage('Country', country);

    message += newRequiredFieldList.reduce((accumulator, field) => { return accumulator + getMessage(field, userAccountInfo[field]) }, '');

    let isValid = isValidGender && isValidEmail && userAccountInfo.FirstName !== '' && userAccountInfo.LastName !== '' &&
        userAccountInfo.Phone !== '' && userAccountInfo.Address !== '' && userAccountInfo.City !== '' &&
        userAccountInfo.Postcode !== '' && userAccountInfo.Country !== '' && userAccountInfo[selectedField] !== '';

    const resultValidateMoreSiteUrls = (userAccountInfo.HasWebSite && moreSiteUrlsList) ? validateMoreSiteUrls(moreSiteUrlsList) : ({ message: '', isValid: true });
    const resultValidatePaymentMethod = validatePaymentMethod(userAccountInfo);

    return {
        message: message + resultValidateMoreSiteUrls.message + resultValidatePaymentMethod.message,
        isValid: isValid && resultValidateMoreSiteUrls.isValid && resultValidatePaymentMethod.isValid
    };
}