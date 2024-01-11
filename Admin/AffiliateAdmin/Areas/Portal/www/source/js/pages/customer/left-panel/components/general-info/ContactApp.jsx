import React from 'react';
import { connect } from 'react-redux';
import { Row, Col } from 'reactstrap';
import { htmlDecode } from '../../../../../modules/htmlHelper';

export const ContactApp = (props) => {
    const { affiliateInfo } = props;
    const contactAppList = affiliateInfo.ContactApps && JSON.parse(htmlDecode(affiliateInfo.ContactApps));

    return contactAppList && (
        <React.Fragment>
            {
                contactAppList.map(contactApp => {
                    return (
                        <Row key={`contactApp-${contactApp.Name}`}>
                            <Col sm={5}>{contactApp.Name}</Col>
                            <Col sm={7} className='text-right'>{contactApp.Value}</Col>
                        </Row>
                    )
                })
            }
        </React.Fragment>
    )
}

function mapStateToProps({ leftPanel }) {
    const { affiliateInfo } = leftPanel;
    return Object.assign({}, { affiliateInfo });
}

export default connect(mapStateToProps)(React.memo(ContactApp));