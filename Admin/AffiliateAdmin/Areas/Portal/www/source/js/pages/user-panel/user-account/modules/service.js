import React from 'react';
import { Col, FormGroup, Label, Input } from 'reactstrap';

export const renderInputForm = (name, label, inputValue, isStatic, isRequired, changeHandler) => {
    return (
        <FormGroup row key={name}>
            <Label for={name} sm={3} xl={2} className={isRequired ? "required" : ""}>{label}</Label>
            <Col sm={9} xl={10}>
                {
                    isStatic ? (<p className="form-control-plaintext">{inputValue}</p>) :
                        (<Input type="text" id={name} name={name} value={inputValue || ''} onChange={changeHandler} />)
                }
            </Col>
        </FormGroup>
    )
}

export const capitalizeFirstChar = (string) => {
    return string.charAt(0).toUpperCase() + string.slice(1);
}