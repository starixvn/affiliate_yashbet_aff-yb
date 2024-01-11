import React from 'react';
import { connect } from 'react-redux';

import AppLayout from '../../../layouts/AppLayout';
import Loader from '../../../components/Loader';
import Filter from './Filter';
import ControlButtons from './ControlButtons';
import Content from './Content';
import MessageDetailForm from './MessageDetailForm';

export const AppContainer = (props) => {
    return (
        <AppLayout>
            <Loader show={props.isShowLoader} />
            <div className="message mt-2">
                <Filter />
                <ControlButtons />
                <Content />
                <MessageDetailForm />
            </ div>
        </AppLayout>
    );
}

function mapStateToProps({ isShowLoader }) {
    return Object.assign({}, { isShowLoader });
}

export default connect(mapStateToProps)(React.memo(AppContainer));