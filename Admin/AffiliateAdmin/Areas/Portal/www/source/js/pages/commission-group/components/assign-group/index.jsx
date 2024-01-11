import React, { Component } from 'react';
import ReactDOM from 'react-dom';

import AppLayout from "../../../../layouts/AppLayout";
import AssignGroup from './AssignGroup';

export default class App extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <AppLayout>
                <AssignGroup />
            </AppLayout>
        );
    }
}

ReactDOM.render(<App />, document.getElementById("root"));