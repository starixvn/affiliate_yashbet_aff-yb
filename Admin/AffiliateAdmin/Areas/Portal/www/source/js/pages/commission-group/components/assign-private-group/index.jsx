import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";
import { configureStore } from '../../../../modules/store';
import AssignPrivateGroup from './AssignPrivateGroup';

import reducer from './reducer';

const store = configureStore(window.initialState, reducer);

export default class App extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <Provider store={store}>
        <AssignPrivateGroup />
      </Provider>
    );
  }
}

ReactDOM.render(<App />, document.getElementById("root"));
