import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Button, Form, FormGroup, Col, Label, Input, Modal, ModalHeader, ModalBody, Alert } from 'reactstrap';
import { globalResource } from '../libs/constants';
import { monthNames } from '../../../../modules/constants';
import { fetchData, getMemberDetail, updateMember, setSelectedMemberId } from '../actions';

const initialState = {
    memberName: '',
    isInclude: true,
    isRemoved: false,
    lastActionDate: '',
    isClickedSubmit: false,
    isShowMessage: false,
    isSuccess: true,
    message: ''
};

export class EditMemberPopup extends PureComponent {
    constructor(props) {
        super(props);

        this.state = initialState
        this.orignalInfo = {
            isInclude: true,
            isRemoved: false
        }
    }

    componentDidUpdate = (prevProps) => {
        const { selectedMemberId } = this.props;

        if (!!selectedMemberId && selectedMemberId !== prevProps.selectedMemberId) {
            this.getMemberInfo(selectedMemberId);
        }
    }

    getMemberInfo = async (selectedMemberId) => {
        var memberInfo = await this.props.getMemberDetail(selectedMemberId);

        if (!!memberInfo) {
            this.orignalInfo = {
                isInclude: memberInfo.IsInclude,
                isRemoved: memberInfo.IsRemoved
            }

            this.setState(prevState => Object.assign({}, prevState, {
                memberName: memberInfo.UserName,
                isInclude: memberInfo.IsInclude,
                isRemoved: memberInfo.IsRemoved,
                lastActionDate: memberInfo.LastActionDate
            }))
        }
    }

    getNoteMesssage = (today, isRemoved, isInclude) => {
        const now = new Date(today);
        const month = now.getMonth();
        const monthName = monthNames[month];

        if (isRemoved) {
            return isInclude ? `The report will include data from 01/${monthName} to ${now.getDate()}/${monthName}` : `The report will exclude data from 01/${monthName}`;
        }

        return `The report will include data from 01/${isInclude ? monthName : monthNames[month + 1]}`;
    }

    isValidData = (today, isRemoved, isInclude) => {
        if (this.orignalInfo.isRemoved == isRemoved) {
            if (this.orignalInfo.isInclude == isInclude) {
                return false;
            }

            const { lastActionDate } = this.state;
            if (!lastActionDate) {
                return true;
            }

            const lastDate = new Date(lastActionDate);
            const now = new Date(today);

            if (now.getMonth() != lastDate.getMonth() || now.getFullYear() != lastDate.getFullYear()) {
                return false;
            }
        }

        return true;
    }

    handleChangeSelectBox = (event) => {
        this.setState({ isInclude: event.target.value === "0" })
    }

    handleCheckRemoved = (isRemoved) => {
        this.setState({ isRemoved: !isRemoved })
    }

    handleSubmit = async () => {
        const { selectedMemberId } = this.props;
        const { isInclude, isRemoved } = this.state;

        const result = await this.props.updateMember({ memberId: selectedMemberId, isInclude, isRemoved });

        if (result) {
            if (result.IsSuccessful) {
                this.props.fetchData(this.props.param);

                window.setTimeout(this.handleClosePopup, 3000);
            }

            this.setState({
                isSuccess: result.IsSuccessful,
                isShowMessage: true,
                message: result.IsSuccessful ? "The selected member has been updated successfully!" : result.Message
            }, () => window.setTimeout(this.hideMessage, 3000));
        } else {
            this.setState({
                isSuccess: false,
                isShowMessage: true,
                message: "Updating the selected member is failed!"
            }, () => window.setTimeout(this.hideMessage, 3000));
        }
    }

    handleClosePopup = () => {
        this.props.setSelectedMemberId(null);
        this.setState(initialState);
    }

    hideMessage = () => {
        this.setState({
            isShowMessage: false
        });
    }

    render() {
        const { selectedMemberId, today } = this.props;
        const { memberName, isInclude, isRemoved, isShowMessage, isSuccess, message } = this.state;

        const isShowPopup = !!selectedMemberId;
        const isDisableBtnSave = !this.isValidData(today, isRemoved, isInclude);
        const noteMessage = this.getNoteMesssage(today, isRemoved, isInclude);

        return (
            <Modal isOpen={isShowPopup} toggle={this.handleClosePopup} backdrop='static'>
                <ModalHeader toggle={this.handleClosePopup}><span className="iconpopup"></span>{memberName}</ModalHeader>
                <ModalBody>
                    <div className="modal-body-wrapper">
                        <div className="edit-member-form-container">
                            <Alert color={isSuccess ? 'success' : 'danger'} isOpen={isShowMessage}>
                                <div dangerouslySetInnerHTML={{ __html: message }}></div>
                            </Alert>
                            <Form tag="form" className="mb-4" autoComplete="off">
                                <FormGroup row>
                                    <Label for="isRemoved" sm={5} className="required">Status:</Label>
                                    <Col sm={5}>
                                        <Input type="checkbox" className="mt-3 ml-0" name="isRemoved" id="isRemoved" checked={isRemoved} onClick={() => this.handleCheckRemoved(isRemoved)} />
                                    </Col>
                                </FormGroup>

                                <FormGroup row className="mb-2">
                                    <Label for="isInclude" sm={5} className="required">Current Month Report:</Label>
                                    <Col sm={5}>
                                        <Input type="select" className="mt-2" name="isInclude" id="isInclude" value={isInclude ? "0" : "1"} onChange={this.handleChangeSelectBox}>
                                            <option key="include" value="0">Include</option>
                                            <option key="exclude" value="1">Exclude</option>
                                        </Input>
                                    </Col>
                                </FormGroup>
                                <FormGroup>
                                    <Label className="font-italic">{noteMessage}</Label>
                                </FormGroup>
                            </Form>

                            <Button type="button" color="success" disabled={isDisableBtnSave} className="mr-3" onClick={this.handleSubmit}>{globalResource.Save}</Button>
                            <Button type="button" color="info" onClick={this.handleClosePopup}>{globalResource.Cancel}</Button>
                        </div>
                    </div>
                </ModalBody>
            </Modal>
        );
    }
}

function mapStateToProps({ selectedMemberId, today, param }) {
    return { selectedMemberId, today, param };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ fetchData, getMemberDetail, updateMember, setSelectedMemberId }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(EditMemberPopup);