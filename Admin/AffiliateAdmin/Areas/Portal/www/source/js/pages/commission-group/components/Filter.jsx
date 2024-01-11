import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Button, Form, FormGroup, Input, Label } from 'reactstrap';

import { getGroupSettingList, setSearchedGroupName } from '../actions';

export class Filter extends Component {
    constructor(props) {
        super(props);
    }

    handleChange = (event) => {
        this.props.setSearchedGroupName(event.target.value);
    }

    handleSearch = () => {
        const { pagingOption, searchedGroupName } = this.props;
        const param = Object.assign({}, { PageIndex: 1, PageSize: pagingOption.PageSize }, { GroupName: searchedGroupName });
        this.props.getGroupSettingList(param);
    }

    handlePressEnter = (event) => {
        if (event.charCode === 13) {
            event.preventDefault()
            this.handleSearch();
        }
    }

    render() {
        return (
            <div className="filter-container">
                <Form inline tag="div" style={{ backgroundColor: 'transparent' }}>
                    <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                        <Label className="mr-sm-2">Group Name</Label>{' '}
                        <Input type="text" className="mr-sm-2 search-box" value={this.props.searchedGroupName} onKeyPress={this.handlePressEnter} onChange={this.handleChange} />
                    </FormGroup>
                    <Button outline color="primary" type="button" onClick={this.handleSearch}><span className="oi oi-magnifying-glass"></span> Search</Button>
                </Form>
            </div>
        );
    }
}

function mapStateToProps({ pagingOption, searchedGroupName }) {
    return Object.assign({}, { pagingOption, searchedGroupName });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ getGroupSettingList, setSearchedGroupName }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Filter);
