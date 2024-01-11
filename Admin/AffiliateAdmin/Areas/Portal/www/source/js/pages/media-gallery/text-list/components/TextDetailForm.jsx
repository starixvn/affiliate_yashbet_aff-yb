import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Col, Button, Form, FormGroup, Label, Input, Modal, ModalHeader, ModalBody, Alert } from 'reactstrap';
import { setSelectedTextId, getTextDetail, fetchData, insertText, updateText, toggleLoader, getCategoryList } from '../actions';
import { headerText, defaultState } from '../libs/constants';
import { defaultInsertId, globalResource, deviceTypes, affiliateBanner, userTypeList, UserType, playerBanner } from '../../common/libs/constants';
import { htmlDecode } from '../../../../modules/htmlHelper';

export class TextDetailForm extends PureComponent {
    constructor(props) {
        super(props)

        const { categoryList, defaultLanguage } = this.props;

        this.state = {
            ...defaultState,
            language: defaultLanguage || 'English',
            playerCategoryList: categoryList.filter(c => c.UserType === playerBanner),
            subAffiliateCategoryList: categoryList.filter(c => c.UserType === affiliateBanner)
        }
    }

    componentDidUpdate(prevProps) {
        const { selectedTextId } = this.props;
        if (prevProps.selectedTextId !== selectedTextId) {
            this.setState({ isInsert: selectedTextId === defaultInsertId });

            selectedTextId && this._handleGetTextDetail(selectedTextId);
        }
    }

    _handleGetTextDetail = async (selectedTextId) => {
        const item = await this.props.getTextDetail(selectedTextId, this.props.param.userType);

        item && this.setState({
            textContent: htmlDecode(item.TextContent),
            category: item.Category,
            language: item.Language,
            currency: item.Currency,
            deviceType: item.DeviceType,
            userType: item.UserType,
            isPlayer: item.UserType === playerBanner
        });
    }

    _handleChange = (event) => {
        const value = event.target.value;
        const name = event.target.name;

        this.setState({ [name]: value });

        if (name === UserType) {
            const isPlayer = value == playerBanner;
            this.setState({ isPlayer, category: isPlayer ? 'Basketball' : 'Sign up', currency: 'EURO' });
        }
    }

    _handleSubmit = async () => {
        this.props.toggleLoader(true);

        const { textContent, isInsert } = this.state;

        if (!!textContent) {
            const { category, language, currency, deviceType, userType, isPlayer } = this.state;
            const { selectedTextId } = this.props;
            const message = isInsert ? "Add Successfully!" : "Update Successfully!";
            const textInfo = Object.assign({}, { TextId: selectedTextId, textContent, category, language, currency: isPlayer ? currency : '', deviceType, userType });
            const result = isInsert ? await this.props.insertText(textInfo) : await this.props.updateText(textInfo);

            if (result) {
                this._handleShowAlert(result.IsSuccessful, result.IsSuccessful ? message : result.Message);
            } else {
                this._handleShowAlert(false, isInsert ? "Add Failed!" : "Update Failed!");
            }
        } else {
            this._handleShowAlert(false, isInsert ? 'Please input text to upload.' : "Please input text to update.");
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
            this.props.fetchData(this.props.param);
            this._handleClosePopup();
        }
    }

    _handleClosePopup = () => {
        this.props.setSelectedTextId(null);

        this.setState({ ...defaultState, language: this.props.defaultLanguage || 'English', });
    }

    render() {
        const { selectedTextId, languageList, currencyList } = this.props;
        const { userType, textContent, category, language, currency, deviceType, isShowMessage, isSuccess, message, isInsert, isPlayer, playerCategoryList, subAffiliateCategoryList } = this.state;
        const isShowPopup = selectedTextId !== null;
        const title = isInsert ? "Add Text" : "Update Text";
        const buttonLabel = isInsert ? "Upload" : "Update";
        const categoryList = isPlayer ? playerCategoryList : subAffiliateCategoryList;

        return (
            <Modal isOpen={isShowPopup} toggle={this._handleClosePopup} backdrop='static'>
                <ModalHeader toggle={this._handleClosePopup}><span className="iconpopup"></span>{title}</ModalHeader>
                <ModalBody>
                    <div className="modal-body-wrapper">
                        <div className="text-list-form-container">
                            <Alert color={isSuccess ? 'success' : 'danger'} isOpen={isShowMessage}>
                                <div dangerouslySetInnerHTML={{ __html: message }}></div>
                            </Alert>
                            <Form tag="form" className="mb-2 p-1 border" autoComplete="off">
                                <FormGroup row className="mb-2">
                                    <Label for={textContent} sm={2} className="d-flex align-items-center">Text</Label>
                                    <Col sm={10}>
                                        <Input type="textarea" className="text-content" name="textContent" id="textContent" rows="8"
                                            value={textContent} onChange={this._handleChange} />
                                    </Col>
                                </FormGroup>
                                {
                                    isInsert && <FormGroup row className="mb-2">
                                        <Label sm={2} for="userType">{headerText.UserType}</Label>
                                        <Col sm={5}>
                                            <Input type="select" name="userType" id="userType" value={userType} onChange={this._handleChange}>
                                                {
                                                    Object.keys(userTypeList).map(key => {
                                                        return (<option key={`userType-${key}`} value={key}>{userTypeList[key]}</option>)
                                                    })
                                                }
                                            </Input>
                                        </Col>
                                    </FormGroup>
                                }
                                <FormGroup row className="mb-2">
                                    <Label sm={2} for="category">{headerText.Category}</Label>
                                    <Col sm={5}>
                                        <Input type="select" name="category" id="category" value={category} onChange={this._handleChange}>
                                            {
                                                categoryList.map(item => {
                                                    return (<option key={`category-${item.CategoryName}`} value={item.CategoryName}>{item.CategoryName}</option>)
                                                })
                                            }
                                        </Input>
                                    </Col>
                                </FormGroup>
                                <FormGroup row className="mb-2">
                                    <Label sm={2} for="language">{headerText.Language}</Label>
                                    <Col sm={5}>
                                        <Input type="select" name="language" id="language" value={language} onChange={this._handleChange}>
                                            {
                                                languageList.map(item => {
                                                    return (<option key={`language-${item.LanguageName}`} value={item.LanguageName}>{item.LanguageName}</option>)
                                                })
                                            }
                                        </Input>
                                    </Col>
                                </FormGroup>
                                {
                                    isPlayer && (
                                        <FormGroup row className="mb-2">
                                            <Label sm={2} for="currency">{headerText.Currency}</Label>
                                            <Col sm={5}>
                                                <Input type="select" name="currency" id="currency" value={currency} onChange={this._handleChange}>
                                                    {
                                                        currencyList.map(item => {
                                                            return (<option key={`currency-${item.Name}`} value={item.Name}>{item.Name}</option>)
                                                        })
                                                    }
                                                </Input>
                                            </Col>
                                        </FormGroup>
                                    )
                                }
                                <FormGroup row className="mb-2">
                                    <Label sm={2}>Type</Label>
                                    <Label sm={5}>Text</Label>
                                </FormGroup>
                                <FormGroup row className="mb-2">
                                    <Label sm={2} for="deviceType">{headerText.Device}</Label>
                                    <Col sm={5}>
                                        <Input type="select" name="deviceType" id="deviceType" value={deviceType} onChange={this._handleChange}>
                                            {
                                                Object.keys(deviceTypes).map(key => {
                                                    return (<option key={`deviceType-${deviceTypes[key]}`} value={deviceTypes[key]}>{key}</option>)
                                                })
                                            }
                                        </Input>
                                    </Col>
                                </FormGroup>
                            </Form>

                            <Button type="button" color="success" className="mr-3" onClick={this._handleSubmit}>{buttonLabel}</Button>
                            <Button type="button" color="info" onClick={this._handleClosePopup}>{globalResource.Cancel}</Button>
                        </div>
                    </div>
                </ModalBody>
            </Modal>
        );
    }
}

function mapStateToProps({ selectedTextId, param, categoryList, languageList, currencyList, defaultLanguage }) {
    return Object.assign({}, { selectedTextId, param, categoryList, languageList, currencyList, defaultLanguage });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedTextId, getTextDetail, fetchData, insertText, updateText, toggleLoader, getCategoryList }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(TextDetailForm);