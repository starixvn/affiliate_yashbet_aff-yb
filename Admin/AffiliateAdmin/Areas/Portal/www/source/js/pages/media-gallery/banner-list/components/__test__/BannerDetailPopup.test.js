import React from 'react';
import { BannerDetailPopup } from '../BannerDetailPopup';
import { shallow } from 'enzyme';

import { Modal } from 'reactstrap';

describe('banner list - components - BannerDetailPopup ', () => {

    const defaultProps = {
        bannerDetailUrl: {
            src: "CategoryMedia/Casino/Baccarat/SBNG_2.gif",
            width: 1655,
            height: 1855
        }
    };

    it('render correctly', () => {
        const bannerDetailUrl = Object.assign({}, defaultProps.bannerDetailUrl, { width: 1000, height: 1000 })
        const props = Object.assign({}, defaultProps, { setBannerDetailUrl: jest.fn(), bannerDetailUrl })
        const result = shallow(<BannerDetailPopup {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('render correctly with bannerDetailUrl is null', () => {
        const props = Object.assign({}, defaultProps, { bannerDetailUrl: null })
        const result = shallow(<BannerDetailPopup {...props} />);

        expect(result).toMatchSnapshot();
    });

    it('setBannerDetailUrl - should be called when close popup', () => {
        const bannerDetailUrl = Object.assign({}, defaultProps.bannerDetailUrl, { width: 1000 })
        const props = Object.assign({}, defaultProps, { setBannerDetailUrl: jest.fn(), bannerDetailUrl })
        const result = shallow(<BannerDetailPopup {...props} />);

        result.find(Modal).props().toggle();

        expect(props.setBannerDetailUrl).toHaveBeenCalledWith({ src: null });
    });
})