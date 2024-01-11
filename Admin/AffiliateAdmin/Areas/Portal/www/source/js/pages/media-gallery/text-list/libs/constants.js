import { playerBanner } from "../../common/libs/constants";

export const headerText = {
    Index: '#',
    UserType: 'User Type',
    Content: 'Content',
    Category: 'Category',
    Language: 'Language',
    Currency: 'Currency',
    Device: 'Device'
}

export const messages = {
    PleaseSelect: 'Please select text(s) to delete',
    AreYouSureDeleteItems: 'Are you sure to delete the selected text(s)?',
    DeleteItemsSuccessfully: 'Delete Text(s) Successfully'
}

export const defaultState = {
    userType: playerBanner,
    textContent: '',
    category: 'Basketball',
    currency: 'EURO',
    deviceType: 0,
    isInsert: true,
    isPlayer: true,
    isShowMessage: false,
    isSuccess: true,
    message: ''
}