import React from 'react';
import { Modal, Button } from 'reactstrap';

const widthHeight = { width: 70, height: 35 };

export const NotificationPopup = (props) => {

    const handleClosePopup = () => {
        props.onClose(false);
    }

    const handleConfirm = () => {
        props.onClose(true);
    }

    return (
        <Modal isOpen={!!props.message} toggle={handleClosePopup} backdrop='static'>
            <div className="notification-popup-content p-3" style={{ fontSize: 14 }}>
                <h5>{window.location.host} says</h5>
                <div style={{ whiteSpace: "pre-line" }}>{props.message}</div>
                {
                    props.isConfirmPopup ? (
                        <div className="float-right mt-4">
                            <Button style={widthHeight} color="primary mr-2" onClick={handleConfirm}><strong>OK</strong></Button>
                            <Button style={widthHeight} color="light border" onClick={handleClosePopup}><strong>Cancel</strong></Button>
                        </div>
                    ) : (<Button style={widthHeight} color="primary mt-4 float-right" onClick={handleClosePopup}><strong>OK</strong></Button>)
                }
            </div>
        </Modal>
    );
}

export default React.memo(NotificationPopup);