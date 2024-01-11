import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Col, Form, FormGroup, Label, Input, Button } from 'reactstrap';

import { capitalizeFirstChar } from '../../modules/service';
import { setUserAccountInfo, setMoreSiteUrlsList } from '../../actions';
import NotificationPopup from '../../../../common/components/NotificationPopup';

export class WebSite extends PureComponent {
    constructor(props) {
        super(props);

        this.moreSiteUrlRefs = [];

        for (let _siteUrl of this.props.moreSiteUrlsList) {
            this.moreSiteUrlRefs.push(React.createRef());
        }

        this.state = {
            messageNotification: ''
        }
    }

    handleChange = (event) => {
        const target = event.target;
        const name = capitalizeFirstChar(target.name);

        const newUserAccountInfo = Object.assign({}, this.props.userAccountInfo, { [name]: target.value })
        this.props.setUserAccountInfo(newUserAccountInfo);
    }

    handleChangeMoreUrl = (event) => {
        const { moreSiteUrlsList } = this.props;
        const { name, value } = event.target;
        moreSiteUrlsList[name] = value;

        this.props.setMoreSiteUrlsList(moreSiteUrlsList);
    }

    handleAddUrl = () => {
        const { moreSiteUrlsList, userAccountInfo } = this.props;
        if (!userAccountInfo.SiteUrl.trim()) {
            this.setState({ messageNotification: `Please input Website URL` });
            return;
        }
        const newMoreSiteUrlsList = moreSiteUrlsList.map(siteUrl => siteUrl.trim());
        const findIndex = newMoreSiteUrlsList.indexOf('');

        if (findIndex === -1) {
            this.moreSiteUrlRefs.push(React.createRef());
            moreSiteUrlsList.push('');
            this.props.setMoreSiteUrlsList(moreSiteUrlsList);
        } else {
            this.moreSiteUrlRefs[findIndex].current.focus();
            this.setState({ messageNotification: `Please input Website URL ${findIndex + 2}` });
        }
    }

    handleDeleteUrl = (siteIndex) => {
        const { moreSiteUrlsList } = this.props;
        moreSiteUrlsList.splice(siteIndex, 1);
        this.moreSiteUrlRefs.splice(siteIndex, 1);

        this.props.setMoreSiteUrlsList(moreSiteUrlsList);
    }

    handleChangeHasWebSite = (hasWebSite) => {
        const newUserAccountInfo = Object.assign({}, this.props.userAccountInfo, { HasWebSite: hasWebSite });

        this.props.setUserAccountInfo(newUserAccountInfo);
    }

    handleCloseNotificationPopup = () => {
        this.setState({ messageNotification: '' })
    }

    _renderMoreSiteUrl(moreSiteUrlsList, hasWebSite) {
        return (
            <React.Fragment>
                {
                    moreSiteUrlsList.map((value, index) => {
                        return (
                            <FormGroup row key={`siteUrl${index} ${123}`}>
                                <Label for={`${index}`} sm={3} xl={2} className="required">Website URL {index + 2}</Label>
                                <Col sm={5} xl={6}>
                                    <input className="form-control" disabled={!hasWebSite} name={index} value={value} onChange={this.handleChangeMoreUrl} ref={this.moreSiteUrlRefs[index]} />
                                </Col>
                                <Button outline color="danger" disabled={!hasWebSite} className="mb-2" onClick={() => this.handleDeleteUrl(index)}><span aria-hidden>Del</span></Button>
                            </FormGroup>
                        )
                    })
                }
            </React.Fragment>
        );
    }

    render() {
        const { messageNotification } = this.state;
        const { userAccountInfo, moreSiteUrlsList } = this.props;
        const hasWebSite = userAccountInfo.HasWebSite;

        return (
            <React.Fragment>
                <Form tag="div">
                    <FormGroup>
                        <FormGroup check>
                            <Label check>
                                <Input type="radio" checked={hasWebSite} onChange={() => this.handleChangeHasWebSite(true)} />I have my own website
                        </Label>
                        </FormGroup>
                        <FormGroup row className="pt-1">
                            <Label for="siteUrl" sm={3} xl={2} className="required">Website URL</Label>
                            <Col sm={5} xl={6}>
                                <Input type="text" name="siteUrl" disabled={!hasWebSite} value={userAccountInfo.SiteUrl} onChange={this.handleChange} />
                            </Col>
                            <Button outline color="secondary" disabled={!hasWebSite} className="mb-2" onClick={this.handleAddUrl}>Add Url</Button>
                        </FormGroup>
                        {
                            this._renderMoreSiteUrl(moreSiteUrlsList, hasWebSite)
                        }
                    </FormGroup>
                    <FormGroup className="pt-3">
                        <FormGroup check>
                            <Label check>
                                <Input type="radio" checked={!hasWebSite} onChange={() => this.handleChangeHasWebSite(false)} />I don't have a website
                        </Label>
                        </FormGroup>
                        <FormGroup row className="pt-1">
                            <Label for="noSiteDescription" sm={3} xl={2} className="required">{`Please describe how you would like to promote ${this.props.licenseeName} if you don't have the URL`}</Label>
                            <Col sm={6} xl={7}>
                                <Input type="textarea" disabled={hasWebSite} style={{ height: 70 }} id="noSiteDescription" name="noSiteDescription" value={userAccountInfo.NoSiteDescription} onChange={this.handleChange} />
                            </Col>
                        </FormGroup>
                    </FormGroup>
                </Form>
                {
                    !!messageNotification && <NotificationPopup
                        message={messageNotification}
                        isConfirmPopup={false}
                        onClose={this.handleCloseNotificationPopup}
                    />
                }
            </React.Fragment>
        )
    }
}
function mapStateToProps({ edit, licenseeName }) {
    const { userAccountInfo, moreSiteUrlsList } = edit;
    return Object.assign({}, { userAccountInfo, moreSiteUrlsList, licenseeName });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setUserAccountInfo, setMoreSiteUrlsList }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(WebSite);