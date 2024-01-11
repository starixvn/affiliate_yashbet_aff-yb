import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { setSelectedGroup } from '../../../actions';

import PlatformFeeSetting from './PlatformFeeSetting';
import FeeSettingItem from './FeeSettingItem';

export class DeductionSetting extends Component {
    constructor(props) {
        super(props);

        this.state = {
            isInitialDepositFee: true,
            isInitialWithdrawalFee: true,
        }
    }

    handleChangeDepositFee = (value) => {
        const { DepositFee } = this.props.selectedGroup;
        this.props.setSelectedGroup({
            DepositFee: Object.assign({}, DepositFee, value)
        });
    }

    handleChangeWithdrawalFee = (value) => {
        const { WithdrawalFee } = this.props.selectedGroup;
        this.props.setSelectedGroup({
            WithdrawalFee: Object.assign({}, WithdrawalFee, value)
        });
    }

    render() {
        const { defaultCurrency } = this.props;
        const { isInitialDepositFee, isInitialWithdrawalFee } = this.state;
        const { DepositFee, WithdrawalFee } = this.props.selectedGroup;

        return (
            <fieldset>
                <legend><h6 className="font-weight-bold">Tier Rated Products Deduction</h6></legend>
                <PlatformFeeSetting />

                <p className="font-weight-bold pt-3">Banking Fee</p>
                <FeeSettingItem label="+ Deposit Fee" basedValueName="Deposit"
                    handleChange={this.handleChangeDepositFee}
                    handleUpdateState={() => this.setState({ isInitialDepositFee: false })}
                    data={DepositFee}
                    defaultCurrency={defaultCurrency}
                    isInitial={isInitialDepositFee} />
                <FeeSettingItem label="+ Withdrawal Fee" basedValueName="Withdrawal"
                    handleChange={this.handleChangeWithdrawalFee}
                    handleUpdateState={() => this.setState({ isInitialWithdrawalFee: false })}
                    data={WithdrawalFee}
                    defaultCurrency={defaultCurrency}
                    isInitial={isInitialWithdrawalFee} />
            </fieldset>
        )
    }
}

function mapStateToProp({ selectedGroup, defaultCurrency }) {
    return { selectedGroup, defaultCurrency };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedGroup }, dispatch);
}

export default connect(mapStateToProp, mapDispatchToProps)(DeductionSetting);