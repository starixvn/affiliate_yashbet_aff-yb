import React, { PureComponent } from 'react';
import { Button } from 'reactstrap';
import NotificationPopup from './NotificationPopup';

export default class ButtonDeleteItems extends PureComponent {
    constructor(props) {
        super(props);

        this.state = {
            itemIdList: '',
            message: '',
            isConfirmPopup: false
        }
    }

    _getCheckedItemIdList = () => {
        const { checkedItems } = this.props;

        return Object.keys(checkedItems).filter(key => checkedItems[key]);
    }

    _handleOpenNotificationPopup = (message, isConfirmPopup, itemIdList) => {
        this.setState({ message, isConfirmPopup: isConfirmPopup || false, itemIdList: itemIdList || '' });
    }

    _handleCloseNotificationPopup = (confirmResult) => {
        this.setState({ message: '' });

        const { itemIdList } = this.state;
        if (confirmResult && itemIdList) {
            this._deleteExcute(itemIdList);
        }
    }

    _handleDelete = () => {
        const itemIdList = this._getCheckedItemIdList();
        itemIdList.length === 0 ? this._handleOpenNotificationPopup(this.props.messages.PleaseSelect) : this._handleOpenNotificationPopup(this.props.messages.AreYouSureDeleteItems, true, itemIdList);
    }

    _deleteExcute = async (itemIdList) => {
        this.props.toggleLoader(true);

        const result = await this.props.deleteItems(itemIdList.join(','));

        if (result) {
            if (result.IsSuccessful) {
                this.props.isOpenNotification && this._handleOpenNotificationPopup(this.props.messages.DeleteItemsSuccessfully);

                this.props.fetchData();
            }
            else {
                this._handleOpenNotificationPopup(result.Message);
            }
        }

        this.props.toggleLoader(false);
    }

    render() {
        const { message, isConfirmPopup } = this.state;

        return (
            <React.Fragment>
                {
                    !!message && <NotificationPopup
                        message={message}
                        isConfirmPopup={isConfirmPopup}
                        onClose={this._handleCloseNotificationPopup}
                    />
                }
                <Button outline type="button" color="danger" onClick={this._handleDelete}>Delete<span className={this.props.hasIcon ? "oi oi-x ml-2" : ""}></span></Button>
            </React.Fragment>
        );
    }
}