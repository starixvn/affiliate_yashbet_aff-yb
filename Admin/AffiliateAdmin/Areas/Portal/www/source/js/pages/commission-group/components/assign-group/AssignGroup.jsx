import React, { Component } from "react";
import { post } from '../../../../modules/ajaxRequest';
import { Col, FormGroup, Label, Button, Input, Alert } from 'reactstrap';
import { messages } from '../../modules/constant';
import Loader from '../../../../components/Loader';
import { popupEventNames } from '../../../common/modules/constants/commissionGroup';
import { htmlDecode } from '../../../../modules/htmlHelper';

export default class AssignGroup extends Component {
    constructor(props) {
        super(props);

        let { commGroupList, selectedGroup, affInfo } = window.initialState;
        if (selectedGroup === 0) {
            selectedGroup = commGroupList && commGroupList.length ? Number(commGroupList[0].Id) : 0
        }

        this.state = Object.assign({}, { commGroupList, selectedGroup, affInfo }, {
            isPrivateGroup: false,
            isShowLoader: false,
            isShowMessage: false,
            isSuccess: true,
            message: ''
        });
    }

    handleChange = (event) => {
        const { value } = event.target;
        this.setState({ selectedGroup: Number(value) });
    }

    handleCheck = (event) => {
        const { checked } = event.target;
        this.setState({ isPrivateGroup: checked });
    }

    handleSave = async () => {
        const { isPrivateGroup, affInfo, selectedGroup } = this.state;
        if (isPrivateGroup) {
            parent.postMessage(popupEventNames.changeAssignToPrivateGroup, window.location.origin);
        }
        else {
            if (affInfo.GroupId > 0 && affInfo.GroupId === selectedGroup) {
                this.setState({
                    isShowMessage: true,
                    isSuccess: false,
                    message: messages.choseAnotherGroup
                }, () => window.setTimeout(this.hideMessage, 3000));
                return;
            }

            this.setState({ isShowLoader: true });
            const url = 'Portal/CommGroupAssignment/AssignGroup';
            const params = {
                assignParam: {
                    groupId: selectedGroup,
                    affId: affInfo.AffId
                },
                affInfo
            }
            const response = await post(url, params);
            if (response && response.status === 200) {
                const result = response.data;

                this.setState({
                    isShowMessage: true,
                    isSuccess: result.IsSuccessful,
                    message: result.IsSuccessful ? messages.assignGroupSuccessfully : result.Message
                }, () => window.setTimeout(() => {
                    this.hideMessage();
                    result.IsSuccessful && parent.postMessage(popupEventNames.assignGroupSuccessfully, window.location.origin);
                }, 3000));
            }

            this.setState({ isShowLoader: false });
        }
    }

    hideMessage = () => {
        this.setState({
            isShowMessage: false
        });
    }

    render() {
        const { commGroupList, selectedGroup, affInfo, isPrivateGroup, isShowLoader, isShowMessage, isSuccess, message } = this.state;
        return affInfo && (
            <div className="assign-normal-group" style={{ padding: 10 }}>
                <Loader show={isShowLoader} />
                <Alert color={isSuccess ? 'success' : 'danger'} isOpen={isShowMessage}>
                    <div dangerouslySetInnerHTML={{ __html: message }}></div>
                </Alert>
                <fieldset>
                    <legend><h6 className="font-weight-bold">Assignee</h6></legend>
                    <p>{affInfo.UserName}</p>
                </fieldset>
                <fieldset>
                    <legend><h6 className="font-weight-bold">Revenue Group</h6></legend>
                    <FormGroup row>
                        <Label xs="4" className="font-weight-bold">New Revenue Group: </Label>

                        <Col xs="5" className="select-comm">
                            <select name="SelectedGroup" className="form-control" onChange={this.handleChange} value={selectedGroup.toString()}>
                                {
                                    commGroupList && commGroupList.map((item, index) => {
                                        return (<option key={`commgroup-${index}`} value={item.Id}>{htmlDecode(item.Name)}</option>)
                                    })
                                }
                            </select>
                        </Col>
                    </FormGroup>
                    <FormGroup row>
                        <Label className="font-weight-bold" for="isPrivateGroup" xs="4">Use private group: </Label>
                        <Col xs="5">
                            <FormGroup check>
                                <Label check>
                                    <Input type="checkbox" id="isPrivateGroup" style={{ marginTop: -5 }}
                                        onChange={this.handleCheck} checked={isPrivateGroup} />
                                </Label>
                            </FormGroup>
                        </Col>
                    </FormGroup>
                </fieldset>
                <p className="text-danger">{messages.note}</p>
                <Button type="button" className="my-2 btn btn-success" onClick={this.handleSave}>SAVE</Button>
            </div>
        );
    }
}