import React, { PureComponent } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { setSelectedGroup } from '../../../actions';

import SubAffCommRateItem from "./SubAffCommRateItem";

export class SubAffCommRateSetting extends PureComponent {
    constructor(props) {
        super(props);

        this.defaultItem = {
            subAffCommRate: 0.1
        }
    }

    handleChangeCommission = (subAffCommRate) => {
        this.props.setSelectedGroup({ SubAffCommRate: subAffCommRate });
    }

    render() {
        const { selectedGroup } = this.props;

        return (
            <fieldset className="p2p-product-group">
                <legend><h6 className="font-weight-bold">Sub-Affiliate Comm Rate</h6></legend>
                <SubAffCommRateItem
                    data={selectedGroup.SubAffCommRate}
                    onChangeCommission={(value) => this.handleChangeCommission(value)}
                />
            </fieldset>
        )
    }
}

function mapStateToProp({ selectedGroup }) {
    return { selectedGroup };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setSelectedGroup }, dispatch);
}

export default connect(mapStateToProp, mapDispatchToProps)(SubAffCommRateSetting);