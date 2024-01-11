import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import AppLayout from '../../../../layouts/AppLayout';
import Loader from '../../../../components/Loader';
import Filter from './Filter';
import Unactivated from './Unactivated';
import Activated from './Activated';
import NotificationPopup from '../../../common/components/NotificationPopup';
import { setMessage } from '../actions';

export const AppContainer = (props) => {
    const { isShowLoader, memberNotExist, message } = props;

    return (
        <AppLayout>
            <Loader show={isShowLoader} />
            <div className="rule-text mt-2">Each username seperated by Comma ","</div>
            <Filter />
            {
                memberNotExist && <div className="text-red mt-1">{memberNotExist}</div>
            }
            <Unactivated />
            <Activated />
            {
                !!message && <NotificationPopup
                    message={message}
                    onClose={() => props.setMessage('')}
                />
            }
        </AppLayout>
    );
}

function mapStateToProps({ isShowLoader, memberNotExist, message }) {
    return Object.assign({}, { isShowLoader, memberNotExist, message });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setMessage }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(AppContainer));