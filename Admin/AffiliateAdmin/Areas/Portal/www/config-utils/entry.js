const { projectPath } = require("./constant");

const jsDist = projectPath.relativeOutputPath + "/js";
const styleDist = projectPath.relativeOutputPath + "/style";

const clientRoot = projectPath.clientInputPath;

module.exports = {
    [`${jsDist}/pages/commission/commission-group`]: clientRoot + "/js/pages/commission-group/index.jsx",
    [`${jsDist}/pages/commission/assign-group`]: clientRoot + "/js/pages/commission-group/components/assign-group/index.jsx",
    [`${jsDist}/pages/commission/assign-private-group`]: clientRoot + "/js/pages/commission-group/components/assign-private-group/index.jsx",
    [`${jsDist}/pages/report`]: clientRoot + "/js/pages/report/index.jsx",
    [`${jsDist}/pages/media-gallery/landing-page`]: clientRoot + "/js/pages/media-gallery/landing-page/index.jsx",
    [`${jsDist}/pages/media-gallery/banner-list`]: clientRoot + "/js/pages/media-gallery/banner-list/index.jsx",
    [`${jsDist}/pages/affiliate/affiliate-info`]: clientRoot + "/js/pages/affiliate-info/index.jsx",
    [`${jsDist}/pages/payment/payment-request`]: clientRoot + "/js/pages/payment-request/index.jsx",
    [`${jsDist}/pages/customer/customer`]: clientRoot + "/js/pages/customer/index.jsx",
    [`${jsDist}/pages/message/message`]: clientRoot + "/js/pages/message/index.jsx",
    [`${jsDist}/pages/member/member-list`]: clientRoot + "/js/pages/member/member-list/index.jsx",
    [`${jsDist}/pages/login`]: clientRoot + "/js/pages/login/index.jsx",
    [`${jsDist}/pages/under-maintenance`]: clientRoot + "/js/pages/under-maintenance/index.jsx",
    [`${jsDist}/pages/change-admin-password`]: clientRoot + "/js/pages/change-admin-password/index.jsx",
    [`${jsDist}/pages/domain/domain`]: clientRoot + "/js/pages/domain/domain/index.jsx",
    [`${jsDist}/pages/domain/domain-control`]: clientRoot + "/js/pages/domain/domain-control/index.jsx",
    [`${jsDist}/pages/domain/sub-aff-domain`]: clientRoot + "/js/pages/domain/sub-aff-domain/index.jsx",
    [`${jsDist}/pages/administrator/administrator`]: clientRoot + "/js/pages/administrator/administrator/index.jsx",
    [`${jsDist}/pages/administrator/department`]: clientRoot + "/js/pages/administrator/department/index.jsx",
    [`${jsDist}/pages/home`]: clientRoot + "/js/pages/home/index.jsx",

    [`${styleDist}/portalmain`]: clientRoot + "/style/main.scss",
    [`${styleDist}/layout`]: clientRoot + "/style/layout/main-layout.scss",
    [`${styleDist}/user-panel-layout`]: clientRoot + "/style/layout/user-panel-layout.scss",
    [`${styleDist}/pages/commission-group`]: clientRoot + "/style/pages/commission-group.scss",
    [`${styleDist}/pages/report`]: clientRoot + "/style/pages/report/index.scss",
    [`${styleDist}/pages/media-gallery/landing-page`]: clientRoot + "/style/pages/media-gallery/landing-page.scss",
    [`${styleDist}/pages/media-gallery/banner-list`]: clientRoot + "/style/pages/media-gallery/banner-list.scss",
    [`${styleDist}/pages/affiliate-info`]: clientRoot + "/style/pages/affiliate-info.scss",
    [`${styleDist}/pages/payment/payment-request`]: clientRoot + "/style/pages/payment/payment-request.scss",
    [`${styleDist}/pages/customer`]: clientRoot + "/style/pages/customer/main.scss",
    [`${styleDist}/pages/message/message`]: clientRoot + "/style/pages/message/message.scss",
    [`${styleDist}/login`]: clientRoot + "/style/pages/login.scss",
    [`${styleDist}/under-maintenance`]: clientRoot + "/style/pages/under-maintenance.scss",
    [`${styleDist}/change-admin-password`]: clientRoot + "/style/pages/change-admin-password.scss",
    [`${styleDist}/pages/member/active-member`]: clientRoot + "/style/pages/member/active-member.scss",
    [`${styleDist}/pages/member/member-list`]: clientRoot + "/style/pages/member/member-list.scss",
    [`${styleDist}/pages/domain/domain`]: clientRoot + "/style/pages/domain/domain.scss",
    [`${styleDist}/pages/domain/domain-control`]: clientRoot + "/style/pages/domain/domain-control.scss",
    [`${styleDist}/pages/administrator/administrator`]: clientRoot + "/style/pages/administrator/administrator.scss",
    [`${styleDist}/pages/administrator/department`]: clientRoot + "/style/pages/administrator/department.scss",
    [`${styleDist}/home`]: clientRoot + "/style/pages/home.scss",
    [`${styleDist}/YB`]: clientRoot + "/style/themes/YB.scss",
};