import React, { Component } from "react";

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Modal, ModalHeader, ModalBody, Button } from 'reactstrap';
import GridData from './grouplist/GridData';
import { Pager } from '@9softstudio/react-fixed-table-header';
import AddGroup from './addgroup';
import EditGroup from './editgroup';
import { getGroupSettingList } from '../actions';

export class Content extends Component {
    constructor(props) {
        super(props);

        this.state = {
            Popup: null,
            popupTitle: ''
        }

        this.modalRef = React.createRef();
    }

    handleAddNewGroup = () => {
        this.setState({
            Popup: (<AddGroup onClose={this.handleHide} onSave={this.handleSave} />),
            popupTitle: 'Add a Revenue Group'
        });
    }

    handleEditGroup = (itemId) => {
        this.setState({
            Popup: (<EditGroup onClose={this.handleHide} itemId={itemId} onSave={this.handleSave} />),
            popupTitle: 'Edit the Revenue Group'

        });
    }

    handleHide = () => {
        this.setState({ Popup: null });
    }

    handleSave = () => {
        this.modalRef.current.scrollTo({ top: 0, behavior: "smooth" });
    }

    onPaging = (pageIndex, pageSize) => {
        const { searchedGroupName } = this.props;
        const param = Object.assign({}, { PageIndex: pageIndex, PageSize: pageSize }, { GroupName: searchedGroupName });
        this.props.getGroupSettingList(param);
    }

    render() {
        const { Popup, popupTitle } = this.state;
        const showPopup = Popup !== null;

        return this.props.gridData && (
            <div className="data-content">
                <div className="my-2">
                    <Button outline type="button" onClick={this.handleAddNewGroup}>Add New Group</Button>
                </div>

                <GridData onEdit={this.handleEditGroup} />
                <Pager pageOption={this.props.pagingOption} onPaging={this.onPaging} />

                <Modal isOpen={showPopup} toggle={this.handleHide} size="lg" backdrop='static' innerRef={this.modalRef}>
                    <ModalHeader toggle={this.handleHide}><span className="iconpopup"></span>{popupTitle}</ModalHeader>
                    <ModalBody>
                        <div className="modal-body-wrapper commission-group">
                            {Popup}
                        </div>
                    </ModalBody>
                </Modal>
            </div>
        );
    }
}

function mapStateToProps({ pagingOption, searchedGroupName, gridData }) {
    return Object.assign({}, { pagingOption, searchedGroupName, gridData });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ getGroupSettingList }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Content);