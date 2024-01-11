import React, { useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';

import { setMessage, setShowText, fetchData, setUserNames } from '../actions';
import { headerText } from '../libs/constants';

export const Filter = (props) => {

    const [userNames, setUserNames] = useState('');

    const _handleSearch = () => {
        const userNamesTrim = userNames.replace(/ /g, '');

        if (userNamesTrim) {
            if (userNamesTrim.split(',').length < 100) {
                _handleFetchData(userNamesTrim);
            } else {
                props.setMessage('The function can only check up to 100 accounts per request!')
            }
        } else {
            props.setMessage('Please input Deposit User Name!')
        }
    }

    const _handleFetchData = async (userNames) => {
        props.setUserNames(userNames);
        await props.fetchData({ userNames });
    }

    const _handlePressEnter = (event) => {
        if (event.charCode === 13) {
            event.preventDefault()
            _handleSearch();
        }
    }

    return (
        <div className="filter-container mt-3">
            <Form inline tag="div">
                <FormGroup className="mr-sm-3">
                    <Label className="mr-2">{headerText.DepositUserName}</Label>
                    <Input type="text" className="deposit-user-name" value={userNames} maxLength="4000" onKeyPress={_handlePressEnter} onChange={(e) => setUserNames(e.target.value)} />
                </FormGroup>
                <Button outline color="primary" type="button" onClick={_handleSearch}>{headerText.CheckStatus}</Button>
            </Form>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setMessage, setShowText, fetchData, setUserNames }, dispatch);
}

export default connect(null, mapDispatchToProps)(React.memo(Filter));