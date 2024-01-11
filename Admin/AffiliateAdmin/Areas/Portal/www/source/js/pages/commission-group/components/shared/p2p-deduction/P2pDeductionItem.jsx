import React, { PureComponent } from "react";
import MultipleSelect from "../../../../../components/multiple-select";
import { Col, FormGroup, Label, Form } from 'reactstrap';
import { commissionValueList } from '../../../modules/constant';
import { format } from '../../../../../modules/numberFormat';
import DeductedPercentageGroup from '../deduction/DeductedPercentageGroup';

export default class P2pDeductionItem extends PureComponent {
    constructor(props) {
        super(props);
    }

    handleChange = (selectedItem) => {
        selectedItem && this.props.onSelectProduct(selectedItem);
    }

    handleChangeComm = (event) => {
        const { name, value } = event.target;
        this.props.onChangeCommission({ [name]: Number(value) });
    }

    render() {
        const { data, handleClick, btnText, isOnlyOneP2pDeductionRow, isFirst, index } = this.props;

        const { productList, deductedPercentage, commission } = data;
        const isValid = (isFirst && isOnlyOneP2pDeductionRow) || productList.filter(p => !p.Checked).length < productList.length;

        return (
            <FormGroup row className="align-items-center">
                <Col sm={4}>
                    <span className="group-index">{index}.</span>
                    <div className={`select-product ${isValid ? "" : "is-invalid"}`}>
                        <MultipleSelect
                            dataSource={productList}
                            keyField="ProductId"
                            valueField="ProductName"
                            statusField="Checked"
                            hasAllOption={false}
                            hasInputOption={true}
                            maxDisplayItemCount={1}
                            noneSelectedLabel={isFirst ? "None" : "Please Select"}
                            onChange={this.handleChange} />
                    </div>
                </Col>
                <Col sm={4}>
                    <Form inline>
                        <FormGroup className="mr-2">
                            <Label>Royalty Fee</Label>
                        </FormGroup>
                        <DeductedPercentageGroup
                            value={deductedPercentage}
                            label="| Revenue |"
                            onChange={this.handleChangeComm} />
                    </Form>
                </Col>
                <Col sm={3}>
                    <Form inline>
                        <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                            <Label>Fixed Rate</Label>
                        </FormGroup>
                        <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                            <select name="Commission" className="form-control"
                                value={format(commission || 0, 3)}
                                onChange={this.handleChangeComm} style={{ width: 70 }}>
                                {
                                    Object.keys(commissionValueList).map((key, i) => {
                                        return (<option key={`fixed-rate-${i}`} value={key}>{commissionValueList[key]}</option>);
                                    })
                                }
                            </select>
                        </FormGroup>
                    </Form>
                </Col>
                <Col sm={1}>
                    <button type="button" className="btn btn-action" onClick={handleClick}>{btnText}</button>
                </Col>
            </FormGroup>
        )
    }
}