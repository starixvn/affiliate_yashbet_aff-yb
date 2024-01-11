import React, { Suspense, useEffect } from 'react';
import { connect } from 'react-redux';
import Loader from '../../../../components/Loader';
import UserPanelMenu from './UserPanelMenu';
import { userPanelMenuList, userPanelPendingMenuList } from '../modules/menu';
import { userPanelMenuKey } from '../../common/modules/constants';
import { spinnerLoaderWidth, spinnerLoaderUserPanelHeight } from '../../../../modules/constants';

export const UserPanel = (props) => {
    const { selectedUserPanel, onClickMenu, isPending, isShowLoader } = props;

    useEffect(() => {
        if (isPending && selectedUserPanel === userPanelMenuKey.statementHistory) {
            props.onClickMenu(userPanelMenuKey.edit);
        }
    });

    const menuList = isPending ? userPanelPendingMenuList : userPanelMenuList;
    const componentIndex = menuList.findIndex(p => p.key === selectedUserPanel);
    const UserPanelComponent = componentIndex === -1 ? null : menuList[componentIndex].component;

    return (
        <div className="user-panel-container">
            <Loader show={isShowLoader} style={{ top: spinnerLoaderUserPanelHeight, left: spinnerLoaderWidth }}/>
            <UserPanelMenu selectedUserPanel={selectedUserPanel} onClickMenu={onClickMenu} isPending={isPending} menuList={menuList} />

            <div className="user-panel-content-container">
                {
                    componentIndex !== -1 &&
                    <Suspense fallback={null}>
                        <UserPanelComponent />
                    </Suspense>
                }
            </div>
        </div>
    )
}

function mapStateToProps({ isShowLoaderUserPanel }) {
    return Object.assign({}, { isShowLoader: isShowLoaderUserPanel });
}

export default connect(mapStateToProps)(React.memo(UserPanel));