import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import reducer from './reducers';
import AppContainer from './components/AppContainer';
import { configureStore } from '../../modules/store';
import { headerText, defaultColumns } from './libs/constants';
import { convertListToMutilpleSelected, handleChangeMultipleSelected } from '../../modules/utils';

export default class App extends Component {
    constructor(props) {
        super(props);

        const { commGroupList, languageList, groupId, param } = window.initialState;

        let newCommGroupList = convertListToMutilpleSelected(commGroupList, 'Id', 'Name');

        if (!!groupId) {
            newCommGroupList = handleChangeMultipleSelected(newCommGroupList, groupId.toString());
        }

        const language = {};
        for (let languageItem of languageList) {
            language[languageItem.LanguageCode] = languageItem.LanguageName;
        }

        const filteredColumnList = Object.keys(headerText).filter(c => !defaultColumns.includes(c))
        const columnList = filteredColumnList.map(item => {
            return {
                columnKey: item,
                columnName: headerText[item],
                checked: true
            }
        });

        this.store = configureStore({ languageList: language, commGroupList: newCommGroupList, groupId, param, columnList }, reducer);

    }

    render() {
        return (
            <Provider store={this.store}>
                <AppContainer />
            </Provider>
        );
    }
}

ReactDOM.render(<App />, document.getElementById("root"))