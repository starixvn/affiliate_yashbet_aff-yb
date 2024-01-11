import React from "react";
import TierSetting from './TierSetting';
import MultipleSelect from "../../../../../components/multiple-select";
import { Col, Form, FormGroup, Label } from 'reactstrap';
import DeductedPercentageForm from '../deduction/DeductedPercentageGroup';
import { ONLY_ONE_ITEM, GGR } from '../../../modules/constant';

export default class PlatformFeeItem extends React.Component {
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
        const { data, handleClick, btnText, isOnlyOnePlatformFeeRow, isFirst, index } = this.props;

        const { productList, commission, groupProductId } = data;
        const isValid = (isFirst && isOnlyOnePlatformFeeRow) || productList.filter(p => !p.Checked).length < productList.length;
        const isDisableAddButton = isFirst && (productList.length === ONLY_ONE_ITEM);

        return (
            <div className="mb-2">
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
                                hasInputOption={!isOnlyOnePlatformFeeRow}
                                maxDisplayItemCount={1}
                                noneSelectedLabel={isValid ? "None" : "Please Select"}
                                onChange={this.handleChange}
                                isDisable={isOnlyOnePlatformFeeRow} />
                        </div>
                    </Col>
                    <Col sm={5} className="pl-0">
                        <Form inline>
                            <Label className="font-weight-bold pr-2">Royalty Fee:</Label>
                            <DeductedPercentageForm
                                value={commission}
                                label={GGR}
                                onChange={this.handleChangeComm} />
                        </Form>
                    </Col>
                    <Col sm={3} className="pl-0">
                        <button type="button" className="btn btn-action" style={{ width: '100%', display: 'flex', justifyContent: 'space-between' }} disabled={isDisableAddButton} onClick={handleClick}>{btnText}</button>
                    </Col>
                </FormGroup>
                <TierSetting groupProductId={groupProductId} />
            </div>
        )
    }
}