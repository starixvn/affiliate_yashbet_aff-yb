import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import reducer from './mainpage/reducers';
import AppContainer from './mainpage/components/AppContainer';
import { configureStore } from '../../modules/store';
import { convertListToMutilpleSelected } from '../../modules/utils';

export default class App extends React.PureComponent {
    constructor(props) {
        super(props);

        const { currencyList } = window.initialState;
        const newCurrencyList = convertListToMutilpleSelected(currencyList, "ExchangeId", "Code");

        this.store = configureStore(Object.assign({}, window.initialState, { currencyList: newCurrencyList }), reducer);
    }

    render() {
        return (
            <Provider store={this.store}>
                <AppContainer />
            </Provider>
        );
    }
}

ReactDOM.render(<App />, document.getElementById('root'));