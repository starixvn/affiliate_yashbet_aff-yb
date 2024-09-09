import React, { useState } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';

import { setSelectedMarket, fetchData } from '../actions';
import { headerText, globalResource } from '../libs/constants';

export const Filter = (props) => {
    const { marketList } = props;
    const [market, setMarket] = useState('');

    const _handleSearch = () => {
        props.setSelectedMarket(market);
        props.fetchData(market);
    }

    return marketList && (
        <div className="filter-container my-2">
            <Form inline tag="div">
                <FormGroup className="mr-sm-3">
                    <Label htmlFor="market" className="mr-sm-2">{headerText.Market}</Label>
                    <Input type="select" name="market" value={market} onChange={(e) => setMarket(e.target.value)}>
                        <option key={'market-all'} value={''}>{globalResource.All}</option>
                        {
                            marketList.map(item => {
                                return (<option key={`market-${item.LanguageCode}`} value={item.LanguageCode}>{item.LanguageName}</option>)
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

function mapStateToProps({ marketList }) {
    return Object.assign({}, { marketList });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedMarket, fetchData }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(Filter));