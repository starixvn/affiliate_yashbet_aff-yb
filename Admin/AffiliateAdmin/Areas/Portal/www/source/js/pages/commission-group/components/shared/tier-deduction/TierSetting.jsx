import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { setSelectedGroup } from '../../../actions';

import TierSettingItem from './TierSettingItem';
import { removeTierItem } from '../../../modules/service';
import { isValidNumber } from '../../../../../modules/utils';
import { FIRST_ITEM_INDEX } from '../../../modules/constant';

export class TierSetting extends Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const { selectedGroup, groupProductId } = this.props;

        if (!selectedGroup.TierSettingList.some(t => t.GroupProductId === groupProductId)) {
            this.handleAdd();
        }
    }

    handleAdd = () => {
        const { selectedGroup, groupProductId } = this.props;
        let defaultMinAmount = 0;
        let defaultMinAp = 5;
        let defaultCommission = 0;

        const tierSettingListByGroupProduct = selectedGroup.TierSettingList.filter(t => t.GroupProductId === groupProductId);
        const itemCount = selectedGroup.TierSettingList.filter(t => t.GroupProductId === groupProductId).length;
        if (itemCount > 0) {
            const lastItem = tierSettingListByGroupProduct[itemCount - 1];
            defaultMinAmount = lastItem.MinAmount + 1;
            defaultCommission = lastItem.Commission + 0.1;
        }

        const newTierSetting = {
            TierId: itemCount + 1,
            GroupProductId: this.props.groupProductId,
            MinAmount: defaultMinAmount,
            MinAp: defaultMinAp,
            Commission: defaultCommission
        };

        this.props.setSelectedGroup({ TierSettingList: [...selectedGroup.TierSettingList, newTierSetting] });
    }

    handleRemove = (tierId, groupProductId) => {
        const { TierSettingList } = this.props.selectedGroup;
        const tierSettingList = removeTierItem(tierId, groupProductId, TierSettingList);

        this.props.setSelectedGroup({ TierSettingList: tierSettingList });
    }

    handleChangeInput = (tierId, tierItemValue) => {
        const { MinAmount, MinAp } = tierItemValue;
        if ((!MinAmount || isValidNumber(MinAmount)) &&
            (!MinAp || isValidNumber(MinAp))) {
            const { selectedGroup, groupProductId } = this.props;

            const tierSettingList = selectedGroup.TierSettingList.map(item => {
                if (item.TierId === tierId && item.GroupProductId === groupProductId) {
                    return {
                        ...item,
                        ...tierItemValue
                    };
                }

                return item;
            });

            this.props.setSelectedGroup({ TierSettingList: tierSettingList });
        }
    }

    render() {
        const { selectedGroup, groupProductId } = this.props;
        const tierSettings = selectedGroup.TierSettingList.filter(t => t.GroupProductId === groupProductId)

        return (
                <table className="table table-bordered tier-setting-table" style={{ width: '495px', marginLeft: 42 }}>
                    <thead>
                        <tr>
                            <th style={{ width: 40 }}>Tier</th>
                            <th>Min Net Win</th>
                            <th>Min Active Player</th>
                            <th style={{ width: 85 }}>Rev. Share%</th>
                            <th style={{ width: 50 }}>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            !!tierSettings.length &&
                                tierSettings.map((item, i) => {
                                    const isFirst = i === FIRST_ITEM_INDEX;
                                    const handleClick = isFirst ? this.handleAdd : () => this.handleRemove(item.TierId, groupProductId);

                                    return (<TierSettingItem data={item} key={`tier-${i}`}
                                        handleClick={handleClick}
                                        btnText={isFirst ? '+' : '-'}
                                        onChange={this.handleChangeInput} />);
                                })
                        }
                    </tbody>
                </table>
        )
    }
}

function mapStateToProp({ selectedGroup }, { groupProductId }) {
    return { selectedGroup, groupProductId };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedGroup }, dispatch);
}

export default connect(mapStateToProp, mapDispatchToProps)(TierSetting);