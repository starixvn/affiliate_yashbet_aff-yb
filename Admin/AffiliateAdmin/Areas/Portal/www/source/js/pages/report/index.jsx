import React, { Suspense } from 'react';
import ReactDOM from 'react-dom';
import SubMenuList from '../common/components/SubMenuList';
import { menus } from './libs/menus';
import { Permission } from '../../modules/permissions'
import { BrowserRouter as Router, Route, Switch, Redirect } from 'react-router-dom'

export default class App extends React.PureComponent {
    constructor(props) {
        super(props);
    }

    getRenderMenuItems = () => {
        const currentPermisisons = Permission.currentPermissions;
        return menus.filter(m => currentPermisisons.hasPermission(m.permissionName))
    }

    render() {
        const imgSrc = 'Areas/Portal/www/dist/imgs/i_report.png';
        const renderMenus = this.getRenderMenuItems();
        const firstMenu = renderMenus[0];
        return (
            <Router forceRefresh>
                <SubMenuList img={imgSrc}
                    menus={renderMenus.map((menu) => { return { label: menu.label, href: menu.href } })} />
                <Suspense fallback={null}>
                    <Switch>
                        {renderMenus.map((menu, index) => <Route key={`route_${index}`} path={menu.href} component={menu.component} />)}
                        {firstMenu && <Redirect from="/" to={firstMenu.href} />}
                    </Switch>
                </Suspense>
            </Router>
        )
    }
}

ReactDOM.render(<App />, document.getElementById("root"));