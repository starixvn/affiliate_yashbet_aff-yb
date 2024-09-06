import axios from 'axios';
import { resolveClientUrl } from './utils';

let _xsrfToken = null;

const getXsrfToken = () => {
    if (!_xsrfToken) {
        _xsrfToken = document.querySelector("input[name=__RequestVerificationToken]").value;
    }

    return _xsrfToken;
}

axios.interceptors.response.use(response => Promise.resolve(response),
    error => {
        const { response } = error;
        if (response) {
            const { data } = response;
            const status = data && data.ErrCode || response.status;

            switch (status) {
                case 401:
                    alert("Your session was expired! Please login again.");
                    location.reload(true);
                    break;
                case 403:
                    alert("You are not authorized on this function.");
                    location.reload(true);
                    break;
                case 500:
                    alert("An error occurred while processing your request");
                    break;
                default:
                    break;
            }
        }

        return Promise.reject(error);
    });

export const post = (relativeUrl, params, options) => {
    const url = resolveClientUrl(relativeUrl);
    return axios.post(url, params, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest',
            '__RequestVerificationToken': getXsrfToken()
        }
    })
        .then(response => response)
        .catch(err => {
            options && options.onError && options.onError(err);
        });
}