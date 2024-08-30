import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Col, Button, Form, FormGroup, Label, Input, Modal, ModalHeader, ModalBody, Alert } from 'reactstrap';
import { insertDomain, updateDomain, toggleLoader, fetchData, setSelectedDomain } from '../actions/index';

export class DomainPopup extends PureComponent {
    constructor(props) {
        super(props)

        this.state = {
            isInsert: true,
            isShowMessage: false,
            isSuccess: true,
            message: ''
        }
    }

    componentDidUpdate(prevProps) {
        const { selectedDomain } = this.props;
        if (selectedDomain && prevProps.selectedDomain !== selectedDomain) {
            this.setState({ isInsert: selectedDomain.domainId === 0 });
        }
    }

    _handleChange = (event) => {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;

        const { selectedDomain } = this.props;
        const newDomain = Object.assign({}, selectedDomain, { [target.name]: value });

        this.props.setSelectedDomain(newDomain);
    }

    _handleSubmit = async () => {
        this.props.toggleLoader(true);

        const { selectedDomain, isAffDomain } = this.props;

        if (selectedDomain.domainName) {
            const { isInsert } = this.state;
            const domainInfo = Object.assign({}, selectedDomain, { isAffDomain })
            const message = isInsert ? "Add Successfully!" : "Update Successfully!";

            const result = isInsert ? await this.props.insertDomain(domainInfo) : await this.props.updateDomain(domainInfo);

            if (result) {
                this._handleShowAlert(result.IsSuccessful, result.IsSuccessful ? message : result.Message);
            } else {
                this._handleShowAlert(false, isInsert ? "Add Domain is failed!" : "Update Domain is failed!");
            }

        } else {
            this._handleShowAlert(false, "Please input domain name!")
        }

        this.props.toggleLoader(false);
    }

    _handleKeyPress = (event) => {
        if (event.charCode === 13) {
            event.preventDefault();
            this._handleSubmit();
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
            this.props.fetchData(this.props.isAffDomain);
            this.props.setSelectedDomain(null);
        }
    }

    _handleClosePopup = () => {
        this.props.setSelectedDomain(null)
    }

    render() {
        const { selectedDomain } = this.props;
        const { isInsert, isSuccess, isShowMessage, message } = this.state;
        const isShowPopup = selectedDomain !== null;
        const title = isInsert ? "Add Domain" : "Edit Domain";
        const buttonLabel = isInsert ? "Add" : "Submit";

        return selectedDomain && (
            <Modal isOpen={isShowPopup} toggle={this._handleClosePopup} backdrop='static'>
                <ModalHeader toggle={this._handleClosePopup}><span className="iconpopup"></span>{title}</ModalHeader>
                <ModalBody>
                    <div className="modal-body-wrapper">
                        <div className="domain-form-container">
                            <Alert color={isSuccess ? 'success' : 'danger'} isOpen={isShowMessage}>
                                <div dangerouslySetInnerHTML={{ __html: message }}></div>
                            </Alert>
                            <Form tag="form" className="mb-5" autoComplete="off">
                                <FormGroup row className="mb-2">
                                    <Label htmlFor="domainName" sm={4} className="required">Domain</Label>
                                    <Col sm={8}>
                                        <Input type="text" className="input-required mt-2" name="domainName"
                                            value={selectedDomain.domainName} onChange={this._handleChange} onKeyPress={this._handleKeyPress} />
                                    </Col>
                                </FormGroup>
                                <FormGroup row className="mb-2">
                                    <Label sm={4}>SSL</Label>
                                    <Col sm={8}>
                                        <input type="checkbox" className="mt-2" name="isSSL" checked={selectedDomain.isSSL} onChange={this._handleChange} />
                                    </Col>
                                </FormGroup>
                            </Form>

                            <Button type="button" color="success" className="mr-3" onClick={this._handleSubmit}>{buttonLabel}</Button>
                            <Button type="button" color="info" onClick={this._handleClosePopup}>Cancel</Button>
                        </div>
                    </div>
                </ModalBody>
            </Modal>
        );
    }
}

function mapStateToProps({ selectedDomain, isAffDomain }) {
    return Object.assign({}, { selectedDomain, isAffDomain });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ insertDomain, updateDomain, toggleLoader, fetchData, setSelectedDomain }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(DomainPopup);