import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { Modal, ModalHeader, ModalBody } from 'reactstrap';
import { setBannerDetailUrl } from '../actions/index';
import { resizeWidthHeight } from '../libs/utils';

const maxWidthModal = 1108;

export const BannerDetailPopup = (props) => {

    const { bannerDetailUrl } = props;

    const _handleClosePopup = () => {
        props.setBannerDetailUrl({ src: null })
    }

    const resizeResult = bannerDetailUrl && resizeWidthHeight(bannerDetailUrl.width, bannerDetailUrl.height, maxWidthModal);

    return bannerDetailUrl && bannerDetailUrl.src && (
        <Modal isOpen={!!bannerDetailUrl.src} toggle={_handleClosePopup} size="xl" backdrop='static'>
            <ModalHeader toggle={_handleClosePopup}><span className="iconpopup"></span>{bannerDetailUrl.bannerId}</ModalHeader>
            <ModalBody>
                <div className="modal-body-wrapper embed-responsive overflow-auto text-center">
                    <img src={bannerDetailUrl.src} alt="Banner" width={resizeResult.width} height={resizeResult.height} />
                </div>
            </ModalBody>
        </Modal>
    );
}

function mapStateToProps({ bannerDetailUrl }) {
    return Object.assign({}, { bannerDetailUrl });
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ setBannerDetailUrl }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(React.memo(BannerDetailPopup));