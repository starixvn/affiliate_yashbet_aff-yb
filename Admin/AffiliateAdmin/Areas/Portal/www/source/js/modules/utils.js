const _rootUrl = document.getElementById('RootUrl') ? document.getElementById('RootUrl').value : '/';

export const resolveClientUrl = (relativeUrl) => {
    return _rootUrl + relativeUrl;
}

export const getCookie = (cookieName) => {
    const name = cookieName + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookies = decodedCookie.split(';');
    for (const cookie of cookies) {
        const cookieAfterTrim = cookie.trim();

        if (cookieAfterTrim.indexOf(name) == 0) {
            return cookieAfterTrim.substring(name.length, cookieAfterTrim.length);
        }
    }
    return "";
}

export const renderMode = () => {
    const mode = getCookie("mode");

    const n88Link = document.getElementById('n88-link');
    n88Link && n88Link.setAttribute('href', resolveClientUrl(`Areas/Portal/www/dist/style/N88-${mode || 'lightmode'}.css`));

    if (!mode) {
        document.cookie = "mode=lightmode; path=/";
    }

    return mode !== 'darkmode';
}

export const changeMode = (isLightMode) => {
    const mode = isLightMode ? 'darkmode' : 'lightmode';

    const n88Link = document.getElementById('n88-link');
    n88Link && n88Link.setAttribute('href', resolveClientUrl(`Areas/Portal/www/dist/style/N88-${mode}.css`));

    document.cookie = "mode=" + mode + "; path=/";
}

export const formatUnit = (value) => {
    return value < 10 ? '0' + value : value;
}

const formatMilisecond = (value) => {
    const greaterThanTenFormat = value < 100 ? ('0' + value) : value;

    return value < 10 ? ('00' + value) : greaterThanTenFormat;
}

export const formatDateTime = (dateTimeValue, format) => {
    if (!dateTimeValue) {
        return "";
    }

    if (!format) {
        format = "MM/dd/yyyy";
    }

    var dateTime = typeof dateTimeValue === 'string' ? new Date(dateTimeValue) : dateTimeValue;
    var hours = dateTime.getHours();
    var minutes = dateTime.getMinutes();
    var seconds = dateTime.getSeconds();
    var ampm = hours >= 12 ? 'PM' : 'AM';

    var shortedHours = hours % 12;
    shortedHours = shortedHours === 0 ? 12 : shortedHours; // the hour '0' should be '12'

    var date = dateTime.getDate();
    var month = dateTime.getMonth() + 1;

    var result = format.replace("MM", formatUnit(month))
        .replace("dd", formatUnit(date))
        .replace("yyyy", dateTime.getFullYear());

    return result.replace("hh", formatUnit(shortedHours))
        .replace("mm", formatUnit(minutes))
        .replace("ss", formatUnit(seconds))
        .replace("tt", ampm)
        .replace("HH", formatUnit(hours))
        .replace("SSS", formatMilisecond(dateTime.getMilliseconds()));
}

export const parseDateTime = (value) => {
    var reISO = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*))(?:Z|(\+|-)([\d|:]*))?$/;
    var reMsAjax = /^\/Date\((d|-|.*)\)[\/|\\]$/;

    if (typeof value === 'string') {
        var a = reISO.exec(value);
        if (a)
            return new Date(value);
        a = reMsAjax.exec(value);
        if (a) {
            var b = a[1].split(/[-+,.]/);
            return new Date(b[0] ? +b[0] : 0 - +b[1]);
        }
    }

    return value;
}

export function getDateRange(selectedFromDate, toDateString) {
    const selectedFromDateString = selectedFromDate.format("MM/DD/YYYY");
    const fromDate = new Date(selectedFromDateString);
    const toDate = new Date(toDateString);

    const isOutsideRange = fromDate.getTime() > toDate.getTime();
    const newToDate = isOutsideRange ? selectedFromDateString : toDateString;

    return {
        fromDate: selectedFromDateString,
        toDate: newToDate
    };
}

export const isValidNumber = (inputValue) => {
    const regex = /^\d*$/;
    return regex.test(inputValue);
}

export const convertListToMutilpleSelected = (initialList, key, name, checked = true) => {
    return initialList ? initialList.map(item => {
        return {
            key: item[key],
            name: item[name],
            checked
        };
    }) : [];
}

export const handleChangeMultipleSelected = (initialList, selectedKey) => {
    const selectedList = typeof (selectedKey) === "string" ? selectedKey.split(',').filter(p => !!p) : selectedKey;

    return initialList && initialList.map(item => {
        const { key, name } = item;
        const checked = selectedList.indexOf(key.toString()) !== -1;

        return {
            key,
            name,
            checked
        };
    });
}

export function handleAfterChangeFromDate(selectedFromDate, toDateString, maxRangeDate) {
    const selectedFromDateString = selectedFromDate.format("MM/DD/YYYY");
    const fromDate = new Date(selectedFromDateString);
    const toDate = new Date(toDateString);

    const isOutsideRange = fromDate.getTime() > toDate.getTime();
    const newToDate = isOutsideRange ? selectedFromDateString : getToDate(toDate, selectedFromDateString, toDateString, maxRangeDate);

    return {
        fromDate: selectedFromDateString,
        toDate: newToDate
    };
}

function getToDate(toDate, selectedFromDateString, toDateString, maxRangeDate) {
    if (!maxRangeDate) {
        return toDateString;
    }

    const nextSixMonth = new Date(selectedFromDateString);
    nextSixMonth.setMonth(nextSixMonth.getMonth() + maxRangeDate);

    return toDate > nextSixMonth ? `${formatUnit(nextSixMonth.getMonth() + 1)}/${formatUnit(nextSixMonth.getDate())}/${nextSixMonth.getFullYear()}` : toDateString;
}

export function handleAfterChangeToDate(selectedToDate, fromDateString, maxRangeDate) {
    const selectedToDateString = selectedToDate.format("MM/DD/YYYY");
    const fromDate = new Date(fromDateString);
    const toDate = new Date(selectedToDateString);

    const isOutsideRange = fromDate.getTime() > toDate.getTime();
    const newFromDate = isOutsideRange ? selectedToDateString : getFromDate(fromDate, selectedToDateString, fromDateString, maxRangeDate);

    return {
        fromDate: newFromDate,
        toDate: selectedToDateString
    };
}

function getFromDate(fromDate, selectedToDateString, fromDateString, maxRangeDate) {
    if (!maxRangeDate) {
        return fromDateString;
    }

    const sixMonthAgo = new Date(selectedToDateString);
    sixMonthAgo.setMonth(sixMonthAgo.getMonth() - maxRangeDate);

    return sixMonthAgo < fromDate ? fromDateString : `${formatUnit(sixMonthAgo.getMonth() + 1)}/${formatUnit(sixMonthAgo.getDate())}/${sixMonthAgo.getFullYear()}`;
}
