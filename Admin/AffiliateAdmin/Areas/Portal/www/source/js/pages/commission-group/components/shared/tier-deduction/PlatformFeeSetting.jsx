import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Input, Form, FormGroup, Label } from 'reactstrap';

import { setSelectedGroup } from '../../../actions';

import PlatformFeeItem from "./PlatformFeeItem";
import { deepClone, updateObjectInArray } from "../../../../../modules/dataHandler";
import { FIRST_ITEM_INDEX, ONLY_ONE_ITEM } from '../../../modules/constant';
import { convertProductList, createDefaultPlatformfee, getDeductionSetting } from '../../../modules/service';

export class PlatformFeeSetting extends PureComponent {
    constructor(props) {
        super(props);

        const { deductionList, productList } = this.props;
        const deductionSetting = getDeductionSetting(deductionList);
        const defaultPlatformSetting = createDefaultPlatformfee(productList, deductionSetting.platformFeeSetting);

        this.defaultItem = {
            DeductionId: defaultPlatformSetting.deductionId,
            ProductIds: '',
            DeductedPercentage: defaultPlatformSetting.commission
        }
    }

    handleAdd = () => {
        const { PlatformFeeList } = this.props.selectedGroup;
        const platformFeeList = [...PlatformFeeList, { ...this.defaultItem, GroupProductId: Date.now() }];

        this.props.setSelectedGroup({ PlatformFeeList: platformFeeList });
    }

    handleRemove = (index) => {
        const { PlatformFeeList } = this.props.selectedGroup;
        const platformFeeList = deepClone(PlatformFeeList);

        const rootProductIds = platformFeeList[0].ProductIds.split(',').filter(p => !!p);
        const checkedProductIds = platformFeeList[index].ProductIds.split(',').filter(p => !!p);

        for (let productId of checkedProductIds) {
            rootProductIds.push(productId);
        }

        platformFeeList[0].ProductIds = rootProductIds.join(',');

        platformFeeList.splice(index, 1);

        this.props.setSelectedGroup({ PlatformFeeList: platformFeeList });
    }

    handleSelect = (index, selectedItem) => {
        const { PlatformFeeList } = this.props.selectedGroup;
        const isChecked = selectedItem.Checked;
        const platformFeeList = deepClone(PlatformFeeList);
        const settingCount = platformFeeList.length;

        if (settingCount > 1) {
            if (isChecked) {
                const currentProductIds = platformFeeList[index].ProductIds.split(',').filter(p => !!p);
                currentProductIds.push(selectedItem.ProductId.toString());
                platformFeeList[index].ProductIds = currentProductIds.join(',');

                for (let i = 0; i < platformFeeList.length; i++) {
                    if (i !== index) {
                        platformFeeList[i].ProductIds = platformFeeList[i].ProductIds.split(',')
                            .filter(p => p && p !== selectedItem.ProductId.toString())
                            .join(',');
                    }
                }
            }
            else {
                if (index != FIRST_ITEM_INDEX) {
                    platformFeeList[index].ProductIds = platformFeeList[index].ProductIds.split(',')
                        .filter(p => p && p !== selectedItem.ProductId.toString())
                        .join(',');

                    const firstItemProductIds = platformFeeList[0].ProductIds.split(',').filter(p => !!p);
                    firstItemProductIds.push(selectedItem.ProductId.toString());

                    platformFeeList[0].ProductIds = firstItemProductIds.join(',');
                }
            }
        }

        this.props.setSelectedGroup({ PlatformFeeList: platformFeeList });
    }

    handleChangeCommission = (index, value) => {
        const { PlatformFeeList } = this.props.selectedGroup;
        const newCommissionSetting = Object.assign({}, PlatformFeeList[index], value);

        const platformFeeList = updateObjectInArray(PlatformFeeList, index, newCommissionSetting);

        this.props.setSelectedGroup({ PlatformFeeList: platformFeeList });
    }

    handleCheck = (isActivePlayerSum) => {
        this.props.setSelectedGroup({ IsActivePlayerSum: isActivePlayerSum });
    }

    render() {
        const { productList, selectedGroup, selectedProductIds } = this.props;
        const { PlatformFeeList, IsActivePlayerSum } = selectedGroup;
        const selectProducts = productList.filter(p => !selectedProductIds.find(i => i === p.ProductId));

        const platformFeeData = PlatformFeeList.map(item => {
            return {
                productList: convertProductList(selectProducts, item.ProductIds),
                commission: item.DeductedPercentage,
                groupProductId: item.GroupProductId
            }
        });

        const isOnlyOnePlatformFeeRow = platformFeeData.length === ONLY_ONE_ITEM;

        return (
            <div className="platformfee-group">
                <div className="d-flex">
                    <p className="font-weight-bold mr-5">Product Groups</p>
                    <Form inline>
                        <FormGroup check>
                            <Label check>
                                <Input type="checkbox" name="IsActivePlayerSum" checked={IsActivePlayerSum}
                                    onChange={() => this.handleCheck(!IsActivePlayerSum)} />Count Active Player across all products(exclude Free Bet)
                            </Label>
                        </FormGroup>
                    </Form>
                </div>
                {
                    platformFeeData.map((item, i) => {
                        const isFirst = i === FIRST_ITEM_INDEX;
                        const handleClick = isFirst ? this.handleAdd : () => this.handleRemove(i);

                        return (
                            <PlatformFeeItem data={item} key={`platformfee-${i}`}
                                isOnlyOnePlatformFeeRow={isOnlyOnePlatformFeeRow}
                                onSelectProduct={(selectedItem) => this.handleSelect(i, selectedItem)}
                                onChangeCommission={(value) => this.handleChangeCommission(i, value)}
                                handleClick={handleClick}
                                btnText={isFirst ? (<><span>Add Product Group</span><span>+</span></>) : (<><span>Remove Product Group</span><span>-</span></>)}
                                isFirst={isFirst}
                                index={i + 1} />
                        )
                    })
                }
            </div>
        )
    }
}

function mapStateToProp({ deductionList, productList, selectedGroup, selectedProductIds }) {
    return { deductionList, productList, selectedGroup, selectedProductIds };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedGroup }, dispatch);
}

export default connect(mapStateToProp, mapDispatchToProps)(PlatformFeeSetting);