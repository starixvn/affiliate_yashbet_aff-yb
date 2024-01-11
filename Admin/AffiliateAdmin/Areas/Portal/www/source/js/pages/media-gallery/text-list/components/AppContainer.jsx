import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import AppLayout from '../../../../layouts/AppLayout';
import Loader from '../../../../components/Loader';
import Filter from './Filter';
import ControlButtons from '../../common/components/ControlButtons';
import Content from './Content';
import TextDetailForm from './TextDetailForm';

import { messages } from '../libs/constants';
import { setParam, fetchData, toggleLoader, setSelectedTextId, setCheckedTexts, deleteTexts, setShouldResetScrollPosition } from '../actions';

export const AppContainer = (props) => {
    return (
        <AppLayout>
            <Loader show={props.isShowLoader} />
            <Filter />
            <ControlButtons
                messages={messages}
                gridData={props.gridData}
                fetchData={() => props.fetchData(props.param)}
                checkedItems={props.checkedTexts}
                setCheckedItems={props.setCheckedTexts}
                toggleLoader={props.toggleLoader}
                setShouldResetScrollPosition={props.setShouldResetScrollPosition}
                setSelectedItemId={props.setSelectedTextId}
                deleteItems={(textIdList) => props.deleteTexts({ textIdList, userType: props.param.userType })}
            />
            <Content />
            <TextDetailForm />
        </AppLayout>
    );
}

function mapStateToProps({ isShowLoader, param, gridData, checkedTexts }) {
    return Object.assign({}, { isShowLoader, param, gridData, checkedTexts });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setParam, fetchData, toggleLoader, setSelectedTextId, setCheckedTexts, deleteTexts, setShouldResetScrollPosition }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(AppContainer));