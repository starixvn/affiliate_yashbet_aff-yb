import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Col, Form, FormGroup, Label, Input } from 'reactstrap';

import { capitalizeFirstChar } from '../../modules/service';
import { setUserAccountInfo } from '../../actions';
import { playerMethod, moneybookersMethod, wireMethod, minimumPaymentText } from '../../modules/constants';
import { formatDecimal } from '../../../../../modules/numberFormat';

export class Payment extends PureComponent {
    constructor(props) {
        super(props);
    }

    handleChangePaymentMethod = (event) => {
        const newUserAccountInfo = Object.assign({}, this.props.userAccountInfo, { PreferredPayment: event.target.value })
        this.props.setUserAccountInfo(newUserAccountInfo);
    }

    handleChange = (event) => {
        const target = event.target;
        const name = capitalizeFirstChar(target.name);

        const newUserAccountInfo = Object.assign({}, this.props.userAccountInfo, { [name]: target.value })
        this.props.setUserAccountInfo(newUserAccountInfo);
    }

    _renderFormByMethod(userAccountInfo) {
        return {
            [playerMethod]: this._renderPaymentForm('playerUsername', `${this.props.licenseeName} User Name`, userAccountInfo.PlayerUsername, true, this.handleChange),
            [moneybookersMethod]: this._renderPaymentForm('moneybookersEmail', 'Moneybookers Email', userAccountInfo.MoneybookersEmail, true, this.handleChange),
            [wireMethod]: this._renderWireForm(userAccountInfo)
        }
    }

    _renderPaymentForm(name, label, inputValue, isRequired, changeHandler) {
        return (
            <FormGroup row>
                <Label for={name} sm={3} xl={2} className={isRequired ? "required" : ""}>{label}</Label>
                <Col sm={5} xl={6}>
                    <Input type="text" id={name} name={name} value={inputValue} onChange={changeHandler} />
                </Col>
            </FormGroup>
        )
    }

    _renderWireForm(userAccountInfo) {
        return (
            <React.Fragment>
                {this._renderPaymentForm('bankName', 'Wire Name', userAccountInfo.BankName, true, this.handleChange)}
                {this._renderPaymentForm('bankAddress', 'Wire Address', userAccountInfo.BankAddress, true, this.handleChange)}
                {this._renderPaymentForm('bankCode', 'Wire ABA/Swift/Sort Code', userAccountInfo.BankCode, false, this.handleChange)}
                {this._renderPaymentForm('bankAccountName', 'Wire Account Name', userAccountInfo.BankAccountName, true, this.handleChange)}
                {this._renderPaymentForm('bankAccountNumber', 'Wire Account Number', userAccountInfo.BankAccountNumber, true, this.handleChange)}
            </React.Fragment>
        )
    }

    render() {
        const { userAccountInfo, paymentMethodList, minimumPayment, licenseeName } = this.props;
        const renderFormByMethod = userAccountInfo && this._renderFormByMethod(userAccountInfo);

        return userAccountInfo && (
            <Form tag="div">
                <FormGroup row>
                    <Label for="preferredPayment" sm={3} xl={2}>Preferred Payment Method</Label>
                    <Col sm={5} xl={6}>
                        <Input type="select" id="preferredPayment" name="preferredPayment" onChange={this.handleChangePaymentMethod} value={userAccountInfo.PreferredPayment}>
                            {paymentMethodList && paymentMethodList.map(item => {
                                return (<option key={item.Key} value={item.Key}>{item.Value}</option>)
                            })}
                        </Input>
                    </Col>
                    <p>{minimumPaymentText + " " + formatDecimal(minimumPayment)}</p>
                </FormGroup>
                {renderFormByMethod[userAccountInfo.PreferredPayment]}
                {/* <p className="text-danger offset-sm-3 offset-xl-2 pl-1">{`You can request for a withdrawal through Moneybookers, Wire Transfer or directly into your ${licenseeName}.com player account.`}</p> */}
            </Form>
        )
    }
}

function mapStateToProps({ edit, licenseeName }) {
    const { userAccountInfo } = edit;
    return Object.assign({}, { userAccountInfo, licenseeName });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setUserAccountInfo }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Payment);