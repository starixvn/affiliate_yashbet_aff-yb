import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { setSelectedGroup, setSelectedProductIds } from '../../../actions';

import P2pDeductionItem from "./P2pDeductionItem";
import { deepClone, updateObjectInArray } from "../../../../../modules/dataHandler";
import { FIRST_ITEM_INDEX, ONLY_ONE_ITEM } from '../../../modules/constant';
import { convertProductList, createDefaultP2pDeduction, getDeductionSetting, convertStringToArrayNumber, getSelectedProductIds, updatePlatfromFeeList } from '../../../modules/service';

export class P2pDeductionSetting extends PureComponent {
    constructor(props) {
        super(props);

        const { deductionList, p2pProductList, productList } = this.props;
        const deductionSetting = getDeductionSetting(deductionList);
        const defaultP2pDeduction = createDefaultP2pDeduction(p2pProductList, deductionSetting.p2pDeductionSetting);

        this.defaultItem = {
            DeductionId: defaultP2pDeduction.deductionId,
            ProductIds: '',
            DeductedPercentage: defaultP2pDeduction.deductedPercentage,
            Commission: defaultP2pDeduction.commission
        }

        this.allProductIds = productList.map(p => p.ProductId);
    }

    handleAdd = () => {
        const { P2pDeductionList } = this.props.selectedGroup;
        const p2pDeductionList = [...P2pDeductionList, { ...this.defaultItem }];

        this.props.setSelectedGroup({ P2pDeductionList: p2pDeductionList });
    }

    handleRemove = (index) => {
        const { P2pDeductionList } = this.props.selectedGroup;
        const p2pDeductionList = deepClone(P2pDeductionList);

        const rootProductIds = p2pDeductionList[0].ProductIds.split(',').filter(p => !!p);
        const checkedProductIds = p2pDeductionList[index].ProductIds.split(',').filter(p => !!p);

        for (let productId of checkedProductIds) {
            rootProductIds.push(productId);
        }

        p2pDeductionList[0].ProductIds = rootProductIds.join(',');

        p2pDeductionList.splice(index, 1);

        this.props.setSelectedGroup({ P2pDeductionList: p2pDeductionList });
    }

    handleSelect = (index, selectedItem) => {
        const { P2pDeductionList, PlatformFeeList } = this.props.selectedGroup;
        const isChecked = selectedItem.Checked;
        const newP2pDeductionList = deepClone(P2pDeductionList);

        if (isChecked) {
            const currentProductIds = newP2pDeductionList[index].ProductIds.split(',').filter(p => !!p);
            currentProductIds.push(selectedItem.ProductId.toString());
            newP2pDeductionList[index].ProductIds = currentProductIds.join(',');

            if (index == FIRST_ITEM_INDEX) {
                for (let i = 0; i < newP2pDeductionList.length; i++) {
                    if (i !== index) {
                        newP2pDeductionList[i].ProductIds = newP2pDeductionList[i].ProductIds.split(',')
                            .filter(p => p && p !== selectedItem.ProductId.toString())
                            .join(',');
                    }
                }
            }
        }
        else {
            newP2pDeductionList[index].ProductIds = newP2pDeductionList[index].ProductIds.split(',')
                .filter(p => p && p !== selectedItem.ProductId.toString())
                .join(',');
        }

        const selectedProductIdsOfP2p = getSelectedProductIds(newP2pDeductionList);
        const newPlatformFeeList = updatePlatfromFeeList(selectedProductIdsOfP2p, PlatformFeeList, this.allProductIds);

        this.props.setSelectedProductIds(selectedProductIdsOfP2p);
        this.props.setSelectedGroup({ P2pDeductionList: newP2pDeductionList, PlatformFeeList: newPlatformFeeList });
    }

    handleChangeCommission = (index, value) => {
        const { P2pDeductionList } = this.props.selectedGroup;
        const newCommissionSetting = Object.assign({}, P2pDeductionList[index], value);

        const p2pDeductionList = updateObjectInArray(P2pDeductionList, index, newCommissionSetting);

        this.props.setSelectedGroup({ P2pDeductionList: p2pDeductionList });
    }

    render() {
        const { productList, selectedGroup } = this.props;
        const { P2pDeductionList } = selectedGroup;
        let restProducts = productList;

        const p2pDeductionData = P2pDeductionList.map(item => {
            const newItem = {
                productList: convertProductList(restProducts, item.ProductIds),
                deductedPercentage: item.DeductedPercentage,
                commission: item.Commission
            }

            const productIdsOfItem = convertStringToArrayNumber(item.ProductIds)
            restProducts = restProducts.filter(p => !productIdsOfItem.find(i => i === p.ProductId));

            return newItem;
        });

        const isOnlyOneP2pDeductionRow = p2pDeductionData.length === ONLY_ONE_ITEM;

        return (
            <fieldset className="p2p-product-group">
                <legend><h6 className="font-weight-bold">P2P Products Deduction</h6></legend>
                {
                    p2pDeductionData.map((item, i) => {
                        const isFirst = i === FIRST_ITEM_INDEX;
                        const handleClick = isFirst ? this.handleAdd : () => this.handleRemove(i);

                        return (
                            <P2pDeductionItem data={item} key={`p2p-${i}`}
                                isOnlyOneP2pDeductionRow={isOnlyOneP2pDeductionRow}
                                onSelectProduct={(selectedItem) => this.handleSelect(i, selectedItem)}
                                onChangeCommission={(value) => this.handleChangeCommission(i, value)}
                                handleClick={handleClick}
                                btnText={isFirst ? '+' : '-'}
                                isFirst={isFirst}
                                index={i + 1}
                            />
                        )
                    })
                }
            </fieldset>
        )
    }
}

function mapStateToProp({ deductionList, productList, p2pProductList, selectedGroup }) {
    return { deductionList, productList, p2pProductList, selectedGroup };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedGroup, setSelectedProductIds }, dispatch);
}

export default connect(mapStateToProp, mapDispatchToProps)(P2pDeductionSetting);