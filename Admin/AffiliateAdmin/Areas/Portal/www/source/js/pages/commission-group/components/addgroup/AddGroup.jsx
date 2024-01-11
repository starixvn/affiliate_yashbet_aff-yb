import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Alert } from 'reactstrap';

import { insertCommissionGroup, getGroupSettingList, setSelectedGroup, setSelectedProductIds } from '../../actions';
import { buildDefaultCommGroup, validate } from '../../modules/service';

import DeductionSetting from '../shared/tier-deduction';
import CommGroupInfo from '../shared/CommGroupInfo';
import { messages } from '../../modules/constant';
import { toggleLoader } from '../../../../actions/loaderAction';
import P2pProductSetting from '../shared/p2p-deduction';
import SubAffCommRateSetting from '../shared/sub-aff-comm-rate';

export class AddGroup extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isShowMessage: false,
            isSuccess: true,
            message: []
        }
    }

    componentDidMount() {
        const { deductionList, productList, p2pProductList } = this.props;
        const commGroup = buildDefaultCommGroup(deductionList, productList, p2pProductList);

        const productIds = p2pProductList.map(p => p.ProductId);
        this.props.setSelectedGroup(commGroup);
        this.props.setSelectedProductIds(productIds);
    }

    componentWillUnmount() {
        this.props.setSelectedGroup(null);
        this.props.setSelectedProductIds(null);
    }

    handleSave = async () => {
        this.props.onSave && this.props.onSave();

        const { selectedGroup } = this.props;
        const validationResult = validate(selectedGroup);

        if (!validationResult.isValid) {
            this.setState({
                isShowMessage: !validationResult.isValid,
                isSuccess: validationResult.isValid,
                message: validationResult.messageList
            });
        }
        else {
            this.props.toggleLoader(true);
            const platformFeeList = selectedGroup.PlatformFeeList.filter(item => !!item.ProductIds);

            const param = { ...selectedGroup, PlatformFeeList: platformFeeList };
            const response = await this.props.insertCommissionGroup(param);
            if (response && response.status === 200) {
                const result = response.data;
                if (result.IsSuccessful) {
                    window.setTimeout(this.props.onClose, 3000);
                    const { pagingOption } = this.props;
                    const gettingParam = Object.assign({}, { PageIndex: pagingOption.PageIndex, PageSize: pagingOption.PageSize, GroupName: this.props.searchedGroupName });
                    this.props.getGroupSettingList(gettingParam);
                }

                this.setState({
                    isShowMessage: true,
                    isSuccess: result.IsSuccessful,
                    message: [result.IsSuccessful ? messages.addGroupSuccessfully : result.Message]
                });
            }
            this.props.toggleLoader(false);
        }
    }

    render() {
        const { isShowMessage, isSuccess, message } = this.state;

        return this.props.selectedGroup && (
            <div className="comm-popup-container">
                <Alert color={isSuccess ? 'success' : 'danger'} isOpen={isShowMessage}>
                    <div dangerouslySetInnerHTML={{ __html: message.join('<br>') }}></div>
                </Alert>
                <CommGroupInfo />
                <SubAffCommRateSetting />
                <P2pProductSetting />
                <DeductionSetting />
                <p className="text-danger">{messages.note}</p>
                <button type="button" className="my-2 btn btn-success btn-save" onClick={this.handleSave}>SAVE</button>
            </div>
        )
    }
}

function mapStateToProp({ deductionList, productList, p2pProductList, selectedGroup, searchedGroupName, pagingOption }) {
    return { deductionList, productList, p2pProductList, selectedGroup, searchedGroupName, pagingOption };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ insertCommissionGroup, getGroupSettingList, setSelectedProductIds, setSelectedGroup, toggleLoader }, dispatch);
}

export default connect(mapStateToProp, mapDispatchToProps)(AddGroup);