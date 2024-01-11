import { globalResource } from '../../../../modules/globalResource';

export { globalResource };

export const headerText = {
    Index: '#',
    LandingPageName: 'Landing Page Name (Shown in dropdown)',
    LandingPageCode: 'Landing Page Code (Contained in links)'
}

export const defaultState = {
    pageName: '',
    pageCode: '',
    isClickedSubmit: false,
    isInsert: true,
    isShowMessage: false,
    isSuccess: true,
    message: ''
}

export const restrictedCharactersPattern = /[;\/\?:@=&"<>#%{}\|\\^~\[\]`\s+]/;
export const maxLength = 50;

export const messages = {
    requiredErrorMessage: 'This field is required',
    maxLengthErrorMessage: 'This field must not exceed 50 characters',
    PleaseSelect: 'Please select landing page(s) to delete',
    AreYouSureDeleteItems: 'Are you sure to delete the selected landing page(s)?',
    DeleteItemsSuccessfully: 'Delete landing page(s) Successfully'
}