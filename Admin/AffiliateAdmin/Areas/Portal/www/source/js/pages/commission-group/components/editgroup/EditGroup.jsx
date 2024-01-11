import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Alert } from 'reactstrap';

import { getSelectedCommGroup, getGroupSettingList, setSelectedGroup, updateCommissionGroup, setSelectedProductIds } from '../../actions';

import DeductionSetting from '../shared/tier-deduction';
import CommGroupInfo from '../shared/CommGroupInfo';
import { messages } from '../../modules/constant';
import { validate } from '../../modules/service';
import { toggleLoader } from '../../../../actions/loaderAction';
import P2pProductSetting from '../shared/p2p-deduction';
import SubAffCommRateSetting from '../shared/sub-aff-comm-rate';

export class EditGroup extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isShowMessage: false,
            isSuccess: true,
            message: []
        }
    }

    componentDidMount() {
        this.props.getSelectedCommGroup({ GroupId: this.props.itemId });
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

            const response = await this.props.updateCommissionGroup(selectedGroup);
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
                    message: [result.IsSuccessful ? messages.updateGroupSuccessfully : result.Message]
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

function mapStateToProp({ selectedGroup, searchedGroupName, pagingOption, p2pProductList }) {
    return { selectedGroup, searchedGroupName, pagingOption, p2pProductList };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ getSelectedCommGroup, getGroupSettingList, setSelectedGroup, updateCommissionGroup, toggleLoader, setSelectedProductIds }, dispatch);
}

export default connect(mapStateToProp, mapDispatchToProps)(EditGroup);