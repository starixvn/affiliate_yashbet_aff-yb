import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Col, Button, Form, FormGroup, Label, Input, Modal, ModalHeader, ModalBody, Alert } from 'reactstrap';
import { insertDomain, updateDomain, toggleLoader, fetchData, setSelectedDomain } from '../actions/index';

export class DomainControlPopup extends PureComponent {
    constructor(props) {
        super(props)

        this.state = {
            isInsert: true,
            isShowMessage: false,
            isSuccess: true,
            message: '',
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

        const { isInsert } = this.state;
        const { selectedDomain } = this.props;
        const message = isInsert ? "Add Successfully!" : "Update Successfully!";

        const result = isInsert ? await this.props.insertDomain(selectedDomain) : await this.props.updateDomain(selectedDomain);

        if (result) {
            this._handleShowAlert(result.IsSuccessful, result.IsSuccessful ? message : result.Message);
        } else {
            this._handleShowAlert(false, isInsert ? "Add Domain is failed!" : "Update Domain is failed!");
        }

        this.props.toggleLoader(false);
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
            this.props.fetchData(this.props.selectedMarket);
            this.props.setSelectedDomain(null);
        }
    }

    _handleClosePopup = () => {
        this.props.setSelectedDomain(null)
    }

    render() {
        const { selectedDomain, marketList, affDomainList, redirectDomainList } = this.props;
        const { isInsert, isSuccess, isShowMessage, message } = this.state;
        const isShowPopup = selectedDomain !== null;
        const title = isInsert ? "Add New Domain Control" : "Edit Domain Control";
        const buttonLabel = isInsert ? "Add" : "Submit";

        return selectedDomain && affDomainList && redirectDomainList && (
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
                                    <Label for="marketId" sm={4}>Market</Label>
                                    <Col sm={8}>
                                        <Input type="select" name="marketId" className="mt-1" value={selectedDomain.marketId} onChange={this._handleChange}>
                                            {
                                                marketList.map(item => {
                                                    return (<option key={`market-${item.LanguageCode}`} value={item.LanguageCode}>{item.LanguageName}</option>)
                                                })
                                            }
                                        </Input>
                                    </Col>
                                </FormGroup>
                                <FormGroup row className="mb-2">
                                    <Label for="affDomainId" sm={4}>Aff Domain</Label>
                                    <Col sm={8}>
                                        <Input type="select" name="affDomainId" className="mt-1" value={selectedDomain.affDomainId} onChange={this._handleChange}>
                                            {
                                                affDomainList.map(item => {
                                                    return (<option key={`affDomain-${item.DomainId}`} value={item.DomainId}>{item.DomainName}</option>)
                                                })
                                            }
                                        </Input>
                                    </Col>
                                </FormGroup>
                                <FormGroup row className="mb-2">
                                    <Label for="redirectDomainId" sm={4}>Redirect Domain</Label>
                                    <Col sm={8}>
                                        <Input type="select" name="redirectDomainId" className="mt-1" value={selectedDomain.redirectDomainId} onChange={this._handleChange}>
                                            {
                                                redirectDomainList.map(item => {
                                                    return (<option key={`redirectDomain-${item.DomainId}`} value={item.DomainId}>{item.DomainName}</option>)
                                                })
                                            }
                                        </Input>
                                    </Col>
                                </FormGroup>
                                <FormGroup row className="mb-2">
                                    <Label sm={4}>Status</Label>
                                    <Col sm={8}>
                                        <input type="checkbox" className="mt-2" name="isUsed" checked={selectedDomain.isUsed} onChange={this._handleChange} />
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

function mapStateToProps({ selectedDomain, selectedMarket, marketList, affDomainList, redirectDomainList }) {
    return Object.assign({}, { selectedDomain, selectedMarket, marketList, affDomainList, redirectDomainList });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ insertDomain, updateDomain, toggleLoader, fetchData, setSelectedDomain }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(DomainControlPopup);