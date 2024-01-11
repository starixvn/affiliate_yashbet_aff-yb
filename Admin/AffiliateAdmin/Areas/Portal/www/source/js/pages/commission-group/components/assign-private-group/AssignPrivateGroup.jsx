import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Alert } from 'reactstrap';

import { getSelectedCommGroup, setSelectedGroup, setSelectedProductIds } from '../../actions';
import { buildDefaultCommGroup, validateTierSettingList, validateItemMustHaveAProduct, validateDeductedFees } from '../../modules/service';
import { messages } from '../../modules/constant';

import DeductionSetting from '../shared/tier-deduction';
import Loader from '../../../../components/Loader';
import { post } from '../../../../modules/ajaxRequest';
import { popupEventNames } from '../../../common/modules/constants/commissionGroup';
import P2pProductSetting from '../shared/p2p-deduction';
import SubAffCommRateSetting from '../shared/sub-aff-comm-rate';

export class AssignPrivateGroup extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isShowLoader: false,
            isShowMessage: false,
            isSuccess: true,
            message: []
        }
    }

    componentDidMount() {
        this.setState({ isShowLoader: true });

        const { GroupId, IsUsingPrivateCommGroup } = this.props.affInfo;

        if (IsUsingPrivateCommGroup && !!GroupId) {
            this.props.getSelectedCommGroup({ GroupId });
        }
        else {
            const { deductionList, productList, p2pProductList } = this.props;
            const productIds = p2pProductList.map(p => p.ProductId);
            this.props.setSelectedProductIds(productIds);

            const commGroup = buildDefaultCommGroup(deductionList, productList, p2pProductList);
            this.props.setSelectedGroup(commGroup);
        }
        this.setState({ isShowLoader: false });
    }

    componentWillUnmount() {
        this.props.setSelectedGroup(null);
    }

    handleSave = async () => {
        window.scrollTo({ top: 0, behavior: "smooth" });

        const validationResult = this._validate();

        if (!validationResult.isValid) {
            this.setState({
                isShowMessage: !validationResult.isValid,
                isSuccess: validationResult.isValid,
                message: validationResult.messageList
            }, () => window.setTimeout(this.hideMessage, 3000));
        }
        else {
            this.setState({ isShowLoader: true });
            const { selectedGroup, affInfo } = this.props;

            const platformFeeList = selectedGroup.PlatformFeeList.filter(item => !!item.ProductIds);
            const commGroup = { ...selectedGroup, PlatformFeeList: platformFeeList };
            const param = { commGroup, affInfo };

            const response = await post('Portal/CommGroupAssignment/AssignPrivateGroup', param);
            if (response && response.status === 200) {
                const result = response.data;

                const resultMessage = result.IsSuccessful ?
                    (affInfo.GroupId > 0 ? messages.updateGroupSuccessfully : messages.assignGroupSuccessfully) :
                    result.Message;

                this.setState({
                    isShowMessage: true,
                    isSuccess: result.IsSuccessful,
                    message: [resultMessage],
                    isShowLoader: false
                }, () => window.setTimeout(() => {
                    this.hideMessage();
                    result.IsSuccessful && parent.postMessage(popupEventNames.updatePrivateGroupSuccessfully, window.location.origin);
                }, 3000));
            }
        }
    }
    hideMessage = () => {
        this.setState({
            isShowMessage: false
        });
    }

    _validate() {
        const { TierSettingList, P2pDeductionList, PlatformFeeList, DepositFee, WithdrawalFee } = this.props.selectedGroup;
        const tierSettingValidation = validateTierSettingList(TierSettingList);
        const selectedProductValidation = validateItemMustHaveAProduct(P2pDeductionList, PlatformFeeList);
        const depositFeeValidation = validateDeductedFees(DepositFee, messages.requireDepositAmount, messages.invalidDepositAmount);
        const withdrawalFeeValidation = validateDeductedFees(WithdrawalFee, messages.requireWithdrawalAmount, messages.invalidWithdrawalAmount);

        const isValid = tierSettingValidation.isValid && selectedProductValidation.isValid  && depositFeeValidation.isValid && withdrawalFeeValidation.isValid;
        const messageList = [...tierSettingValidation.messageList, ...selectedProductValidation.messageList, ...depositFeeValidation.messageList, ...withdrawalFeeValidation.messageList];

        return { isValid, messageList };
    }

    render() {
        const { isShowMessage, isSuccess, message, isShowLoader } = this.state;
        const { affInfo, selectedGroup } = this.props;

        return affInfo && selectedGroup && (
            <div className="comm-popup-container" style={{ padding: 3 }}>
                <Loader show={isShowLoader} />
                <Alert color={isSuccess ? 'success' : 'danger'} isOpen={isShowMessage}>
                    <div dangerouslySetInnerHTML={{ __html: message.join('<br>') }}></div>
                </Alert>
                <SubAffCommRateSetting />
                <P2pProductSetting />
                <DeductionSetting />
                <p className="text-danger">{messages.note}</p>
                <button type="button" className="my-2 btn btn-success btn-save" onClick={this.handleSave}>SAVE</button>
            </div>
        )
    }
}

function mapStateToProp({ deductionList, productList, p2pProductList, selectedGroup, affInfo }) {
    return { deductionList, productList, p2pProductList, selectedGroup, affInfo };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ getSelectedCommGroup, setSelectedGroup, setSelectedProductIds }, dispatch);
}

export default connect(mapStateToProp, mapDispatchToProps)(AssignPrivateGroup);