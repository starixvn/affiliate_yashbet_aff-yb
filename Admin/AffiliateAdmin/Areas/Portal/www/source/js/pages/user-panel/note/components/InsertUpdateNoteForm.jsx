import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Alert, Col, Button, Form, FormGroup, Label, Input, FormFeedback, Modal, ModalHeader, ModalBody } from 'reactstrap';
import { setShowPopup, getNoteList, insertNote, updateNote } from '../actions';

import { htmlDecode } from '../../../../modules/htmlHelper';
import { resource, maxLength } from '../modules/constants';
import { globalResource } from '../../../../modules/globalResource';

export class InsertUpdateNoteForm extends PureComponent {
    constructor(props) {
        super(props);

        this.state = {
            isClickedSubmit: false,
            noteContent: '',
            isShowMessage: false,
            isSuccess: true,
            message: ''
        }
    }

    componentDidUpdate(prevProps) {
        const { noteInfo } = this.props;

        if (noteInfo && noteInfo !== prevProps.noteInfo) {
            this.setState({ noteContent: noteInfo.noteContent })
        }
    }

    handleChange = (event) => {
        this.setState({ [event.target.name]: event.target.value })
    }

    _validateNoteContent = () => {
        const { noteContent } = this.state;

        const result = {
            isValid: !!noteContent && noteContent.length <= maxLength,
            message: ''
        }

        if (!result.isValid) {
            result.message = noteContent ? resource.MaxLengthErrorMessage : resource.RequiredErrorMessage;
        }

        return result;
    }

    handleSave = (isValid) => {
        this.setState({ isClickedSubmit: true });

        if (isValid) {
            const newNoteInfo = Object.assign({}, this.props.noteInfo, { noteContent: this.state.noteContent });
            this.Save(newNoteInfo);
        }
    }

    async Save(noteInfo) {
        const isInsert = noteInfo.noteId === 0;

        const message = isInsert ? resource.AddNoteSuccess : resource.UpdateNoteSuccess;

        const { affId } = this.props;
        const param = { ...noteInfo, affId };
        const result = isInsert ? await this.props.insertNote(param) : await this.props.updateNote(param);

        if (result) {
            this.setState({
                isSuccess: result.IsSuccessful,
                isShowMessage: true,
                message: result.IsSuccessful ? message : result.Message
            }, () => window.setTimeout(this.hideMessage, 3000));

            if (result.IsSuccessful) {
                const { pagingOption } = this.props;
                const param = Object.assign({}, { affId, pageIndex: 1, pageSize: pagingOption.PageSize });

                window.setTimeout(() => this.props.setShowPopup(false), 3000);
                window.setTimeout(() => this.props.getNoteList(param), 3200);
            }
        } else {
            this.setState({
                isSuccess: false,
                isShowMessage: true,
                message: resource.Failed
            }, () => window.setTimeout(this.hideMessage, 3000));
        }
    }

    hideMessage = () => {
        this.setState({
            isShowMessage: false
        });
    }

    handleClosePopup = () => {
        this.props.setShowPopup(false);
        this.setState({ noteContent: '', isClickedSubmit: false });
    }

    render() {
        const { isShowMessage, isSuccess, message, isClickedSubmit } = this.state;
        const { isShowPopup, noteInfo } = this.props;

        const isInsert = noteInfo.noteId === 0;
        const popupTitle = isInsert ? resource.AddNote : resource.UpdateNote;
        const buttonTitle = isInsert ? globalResource.Add : globalResource.Update;

        const validationResult = this._validateNoteContent();

        return (
            <Modal isOpen={isShowPopup} toggle={this.handleClosePopup} backdrop='static'>
                <ModalHeader toggle={this.handleClosePopup}><span className="iconpopup"></span>{popupTitle}</ModalHeader>
                <ModalBody>
                    <div className="modal-body-wrapper">
                        <div className="note-form-container">
                            <Alert color={isSuccess ? 'success' : 'danger'} isOpen={isShowMessage}>
                                <div dangerouslySetInnerHTML={{ __html: message }}></div>
                            </Alert>
                            <Form tag="form" className="mb-5" autoComplete="off">
                                <FormGroup row className="mb-2">
                                    <Label htmlFor="noteContent" sm={3} className="d-flex justify-content-center align-items-center required">Content</Label>
                                    <Col sm={9}>
                                        <Input type="textarea" className="input-required" name="noteContent" id="noteContent" rows="8"
                                            value={htmlDecode(this.state.noteContent)} onChange={this.handleChange} invalid={isClickedSubmit && !validationResult.isValid} />
                                        {validationResult.message &&
                                            <FormFeedback><div dangerouslySetInnerHTML={{ __html: validationResult.message }} /></FormFeedback>}
                                    </Col>
                                </FormGroup>
                            </Form>
                            <Button type="button" color="success" className="mr-3" disabled={isClickedSubmit ? !validationResult.isValid : false} onClick={() => this.handleSave(validationResult.isValid)}>{buttonTitle}</Button>
                            <Button type="button" color="info" onClick={this.handleClosePopup}>{globalResource.Cancel}</Button>
                        </div>
                    </div>
                </ModalBody>
            </Modal>
        );
    }
}

function mapStateToProps({ affCommonInfo, note }) {
    const { pagingOption, noteInfo, isShowPopup } = note;
    const { affId } = affCommonInfo;

    return Object.assign({}, {
        affId,
        isShowPopup,
        pagingOption,
        noteInfo
    });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setShowPopup, getNoteList, insertNote, updateNote }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(InsertUpdateNoteForm);