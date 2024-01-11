import React, { useState } from 'react';
import { connect } from 'react-redux';
import { Row, Col, Button } from 'reactstrap';

import ChangePasswordForm from '../../../../common/components/ChangePasswordForm';
import { resource } from '../../libs/constants';
import { resolveClientUrl } from '../../../../../modules/utils';


export const ApprovedStatusInfo = (props) => {
    const [isChangingPassword, setChangingPassword] = useState(false);
    const { affiliateInfo, affName } = props;

    function toggleChangingPassword(isChanging) {
        setChangingPassword(isChanging);
    }

    return (
        <React.Fragment>
            <Row>
                <Col sm={5}>{resource.Password}</Col>
                <Col sm={7} className='text-right text-primary'>
                    <Button color="link left-panel-btn-link" onClick={() => toggleChangingPassword(true)}>
                        [{resource.Change}]
                    </Button>
                    <ChangePasswordForm
                        showPopupPassword={isChangingPassword}
                        selectedId={affiliateInfo.AffId}
                        setShowPopupPassword={toggleChangingPassword}
                    />
                </Col>
            </Row>
            <Row>
                <Col sm={6}><a href={resolveClientUrl(`Portal/Player?AffName=${affName}`)} target='_top'>{resource.MemberList}</a></Col>
                <Col sm={6} className='text-right'><a href={resolveClientUrl(`Portal/Affiliate?ParentAffName=${affName}`)} target='_top'>{resource.SubAffiliateList}</a></Col>
            </Row>
        </React.Fragment>
    )
}

function mapStateToProps({ affCommonInfo, leftPanel }) {
    const { affiliateInfo } = leftPanel;
    const { affName } = affCommonInfo;
    return Object.assign({}, { affiliateInfo, affName });
}

export default connect(mapStateToProps)(React.memo(ApprovedStatusInfo));