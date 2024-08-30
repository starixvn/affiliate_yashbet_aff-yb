import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Col, Button, Form, FormGroup, Label, Input, Modal, ModalHeader, ModalBody, Alert, FormFeedback } from 'reactstrap';
import { setSelectedLandingPageId, getLandingPageDetail, fetchData, insertLandingPage, updateLandingPage, toggleLoader } from '../actions/index';
import { defaultState, headerText, globalResource, messages, restrictedCharactersPattern, maxLength } from '../libs/constants';
import { defaultInsertId } from '../../common/libs/constants';
import { htmlDecode } from '../../../../modules/htmlHelper';

export class InsertUpdateForm extends PureComponent {
    constructor(props) {
        super(props)

        this.state = {
            ...defaultState
        }
    }

    componentDidMount() {
        this.props.fetchData();
    }

    componentDidUpdate(prevProps) {
        const { selectedLandingPageId } = this.props;
        if (prevProps.selectedLandingPageId !== selectedLandingPageId) {
            this.setState({ isInsert: selectedLandingPageId === defaultInsertId });

            selectedLandingPageId && this._handleGetLandingPageDetail(selectedLandingPageId);
        }
    }

    _handleGetLandingPageDetail = async (selectedLandingPageId) => {
        const item = await this.props.getLandingPageDetail(selectedLandingPageId);

        item && this.setState({
            pageName: htmlDecode(item.PageName),
            pageCode: htmlDecode(item.PageCode)
        });
    }

    _handleChange = (event) => {
        const value = event.target.value;
        const name = event.target.name;

        this.setState({ [name]: value });
    }

    _validatePageCode() {
        const { pageCode } = this.state;
        const hasRestrictedCharacter = restrictedCharactersPattern.test(pageCode);
        const result = {
            isValid: !!pageCode && !hasRestrictedCharacter && pageCode.length <= maxLength,
            message: ''
        }

        if (!result.isValid) {
            result.message = !pageCode ? messages.requiredErrorMessage :
                (hasRestrictedCharacter ?
                    'Special characters are not allowed' :
                    messages.maxLengthErrorMessage);
        }

        return result;
    }

    _validatePageName() {
        const { pageName } = this.state;

        const result = {
            isValid: !!pageName && pageName.length <= maxLength,
            message: ''
        }

        if (!result.isValid) {
            result.message = !pageName ? messages.requiredErrorMessage : messages.maxLengthErrorMessage;
        }

        return result;
    }

    _handleSubmit = async (isValid) => {
        this.props.toggleLoader(true);
        this.setState({ isClickedSubmit: true });

        if (isValid) {
            const { pageName, pageCode, isInsert } = this.state;
            const { selectedLandingPageId } = this.props;
            const message = isInsert ? "Add Successfully!" : "Update Successfully!";
            const landingPageInfo = Object.assign({}, { pageId: selectedLandingPageId, pageName, pageCode });
            const result = isInsert ? await this.props.insertLandingPage(landingPageInfo) : await this.props.updateLandingPage(landingPageInfo);

            if (result) {
                this._handleShowAlert(result.IsSuccessful, result.IsSuccessful ? message : result.Message);
            } else {
                this._handleShowAlert(false, isInsert ? "Add Failed!" : "Update Failed!");
            }
        }

        this.props.toggleLoader(false);
    }

    _handleKeyPress = (event, isValid) => {
        if (event.charCode === 13) {
            event.preventDefault();
            this._handleSubmit(isValid);
        }
    }

    _handleShowAlert = (isSuccess, message) => {
        this.setState({
            message,
            isSuccess,
            isShowMessage: true,
        }, () => window.setTimeout(() => this._hideMessage(isSuccess), 3000));
    }

    _hideMessage = (isSuccess) => {
        this.setState({
            isShowMessage: false
        });

        if (isSuccess) {
            this.props.fetchData();
            this._handleClosePopup();
        }
    }

    _handleClosePopup = () => {
        this.props.setSelectedLandingPageId(null);

        this.setState({ ...defaultState });
    }

    render() {
        const { pageName, pageCode, isShowMessage, isSuccess, message, isInsert, isClickedSubmit } = this.state;
        const pageNameValidation = this._validatePageName();
        const pageCodeValidation = this._validatePageCode();
        const isValid = pageNameValidation.isValid && pageCodeValidation.isValid;
        const isShowPopup = this.props.selectedLandingPageId !== null;
        const title = isInsert ? "Add Landing Page" : "Edit Landing Page";

        return (
            <Modal isOpen={isShowPopup} toggle={this._handleClosePopup} backdrop='static'>
                <ModalHeader toggle={this._handleClosePopup}><span className="iconpopup"></span>{title}</ModalHeader>
                <ModalBody>
                    <div className="modal-body-wrapper">
                        <div className="landing-page-form-container">
                            <Alert color={isSuccess ? 'success' : 'danger'} isOpen={isShowMessage}>
                                <div dangerouslySetInnerHTML={{ __html: message }}></div>
                            </Alert>
                            <Form tag="form" className="mb-5" autoComplete="off">
                                <FormGroup row className="mb-2">
                                    <Label htmlFor="pageName" sm={7} className="required">{headerText.LandingPageName}</Label>
                                    <Col sm={5}>
                                        <Input type="text" name="pageName" className="input-required mt-2" value={pageName} onChange={this._handleChange}
                                            onKeyPress={(e) => this._handleKeyPress(e, isValid)} invalid={isClickedSubmit && !pageNameValidation.isValid} />
                                        {pageNameValidation.message &&
                                            <FormFeedback><div dangerouslySetInnerHTML={{ __html: pageNameValidation.message }} /></FormFeedback>}
                                    </Col>
                                </FormGroup>
                                <FormGroup row>
                                    <Label htmlFor="pageCode" sm={7} className="required">{headerText.LandingPageCode}</Label>
                                    <Col sm={5}>
                                        <Input type="text" name="pageCode" className="input-required mt-2" value={pageCode} onChange={this._handleChange}
                                            onKeyPress={(e) => this._handleKeyPress(e, isValid)} invalid={isClickedSubmit && !pageCodeValidation.isValid} />
                                        {pageCodeValidation.message &&
                                            <FormFeedback><div dangerouslySetInnerHTML={{ __html: pageCodeValidation.message }} /></FormFeedback>}
                                    </Col>
                                </FormGroup>
                            </Form>

                            <Button type="button" color="success" className="mr-3" disabled={isClickedSubmit ? !isValid : false} onClick={() => this._handleSubmit(isValid)} >Submit</Button>
                            <Button type="button" color="info" onClick={this._handleClosePopup}>{globalResource.Cancel}</Button>
                        </div>
                    </div>
                </ModalBody>
            </Modal>
        );
    }
}

function mapStateToProps({ selectedLandingPageId }) {
    return Object.assign({}, { selectedLandingPageId });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedLandingPageId, getLandingPageDetail, fetchData, insertLandingPage, updateLandingPage, toggleLoader }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(InsertUpdateForm);