import React, { useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';

import { setParam, getCategoryList, setDisableCurrency } from '../actions';
import { headerText } from '../libs/constants';
import { globalResource, deviceTypes, userTypeList, affiliateBanner, allUserType } from '../../common/libs/constants';

export const Filter = (props) => {
    const { categoryList, languageList, currencyList, isDisableCurrency } = props;
    const [category, setCategory] = useState('');
    const [language, setLanguage] = useState('');
    const [currency, setCurrency] = useState('');
    const [deviceType, setDeviceType] = useState('');
    const [userType, setUserType] = useState(allUserType);

    const _handleSearch = () => {
        const { pagingOption } = props;

        const param = Object.assign({}, {
            category,
            language,
            currency,
            deviceType,
            userType,
            PageIndex: 1,
            PageSize: pagingOption.PageSize
        })

        props.setParam(param);
    }

    const _handleChangeUserType = (e) => {
        const value = e.target.value;
        setCategory('');
        setCurrency('');
        setUserType(value);

        props.setDisableCurrency(value == affiliateBanner);
        props.getCategoryList(value);
    }

    return categoryList && languageList && currencyList && (
        <div className="filter-container my-2">
            <Form inline tag="div">
                <FormGroup className="mr-sm-3">
                    <Label for="userType" className="mr-sm-2">{headerText.UserType}</Label>
                    <Input type="select" name="userType" id="userType" value={userType} onChange={_handleChangeUserType}>
                        <option key={'userType-all'} value={allUserType}>{globalResource.All}</option>
                        {
                            Object.keys(userTypeList).map(key => {
                                return (<option key={`userType-${key}`} value={key}>{userTypeList[key]}</option>)
                            })
                        }
                    </Input>
                </FormGroup>
                <FormGroup className="mr-sm-3">
                    <Label for="category" className="mr-sm-2">{headerText.Category}</Label>
                    <Input type="select" name="category" id="category" value={category} onChange={(e) => setCategory(e.target.value)}>
                        <option key={'category-all'} value={''}>{globalResource.All}</option>
                        {
                            categoryList.map(item => {
                                return (<option key={`category-${item.CategoryName}`} value={item.CategoryName}>{item.CategoryName}</option>)
                            })
                        }
                    </Input>
                </FormGroup>
                <FormGroup className="mr-sm-3">
                    <Label for="language" className="mr-sm-2">{headerText.Language}</Label>
                    <Input type="select" name="language" id="language" value={language} onChange={(e) => setLanguage(e.target.value)}>
                        <option key={'language-all'} value={''}>{globalResource.All}</option>
                        {
                            languageList.map(item => {
                                return (<option key={`language-${item.LanguageName}`} value={item.LanguageName}>{item.LanguageName}</option>)
                            })
                        }
                    </Input>
                </FormGroup>
                <FormGroup className="mr-sm-3">
                    <Label for="currency" className="mr-sm-2">{headerText.Currency}</Label>
                    <Input type="select" name="currency" id="currency" value={currency} disabled={isDisableCurrency} onChange={(e) => setCurrency(e.target.value)}>
                        <option key={'currency-all'} value={''}>{globalResource.All}</option>
                        {
                            currencyList.map(item => {
                                return (<option key={`currency-${item.Name}`} value={item.Name}>{item.Name}</option>)
                            })
                        }
                    </Input>
                </FormGroup>
                <FormGroup className="mr-sm-3">
                    <Label for="deviceType" className="mr-sm-2">{headerText.Device}</Label>
                    <Input type="select" name="deviceType" id="deviceType" value={deviceType} onChange={(e) => setDeviceType(e.target.value)}>
                        <option key={'deviceType-all'} value="">{globalResource.All}</option>
                        {
                            Object.keys(deviceTypes).map(key => {
                                return (<option key={`deviceType-${deviceTypes[key]}`} value={deviceTypes[key]}>{key}</option>)
                            })
                        }
                    </Input>
                </FormGroup>
                <Button outline color="primary" type="button" onClick={_handleSearch}>
                    <span className="oi oi-magnifying-glass"></span> {globalResource.Search}
                </Button>
            </Form>
        </div>
    )
}

function mapStateToProps({ categoryList, languageList, currencyList, pagingOption, isDisableCurrency }) {
    return Object.assign({}, { categoryList, languageList, currencyList, pagingOption, isDisableCurrency });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setParam, getCategoryList, setDisableCurrency }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(Filter));