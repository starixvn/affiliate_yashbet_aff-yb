import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import AppLayout from '../../../../layouts/AppLayout';
import Loader from '../../../../components/Loader';
import Filter from './Filter';
import ControlButtons from '../../common/components/ControlButtons';
import Content from './Content';
import EditBannerForm from './EditBannerForm';
import BannerDetailPopup from './BannerDetailPopup';

import { messages } from '../libs/constants';
import { setParam, fetchData, toggleLoader, setSelectedBannerId, setCheckedBanners, deleteBanners, setShouldResetScrollPosition } from '../actions';

export const AppContainer = (props) => {

    return (
        <AppLayout>
            <Loader show={props.isShowLoader} />
            <Filter />
            <ControlButtons
                messages={messages}
                gridData={props.gridData}
                fetchData={() => props.fetchData(props.param)}
                checkedItems={props.checkedBanners}
                setCheckedItems={props.setCheckedBanners}
                toggleLoader={props.toggleLoader}
                setShouldResetScrollPosition={props.setShouldResetScrollPosition}
                setSelectedItemId={props.setSelectedBannerId}
                deleteItems={(bannerIdList) => props.deleteBanners({ bannerIdList, userType: props.param.userType })}
            />
            <Content />
            <EditBannerForm />
            <BannerDetailPopup />
        </AppLayout>
    );
}

function mapStateToProps({ isShowLoader, param, gridData, checkedBanners }) {
    return Object.assign({}, { isShowLoader, param, gridData, checkedBanners });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setParam, fetchData, toggleLoader, setSelectedBannerId, setCheckedBanners, deleteBanners, setShouldResetScrollPosition }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(AppContainer));