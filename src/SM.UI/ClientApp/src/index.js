import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";
import App from "./ui/App";
import * as serviceWorker from "./serviceWorker";

import store from "./store/index";

import "antd/dist/antd.dark.css";
import AlertList from "./ui/components/alert/AlertList";

ReactDOM.render(
  //<React.StrictMode></React.StrictMode>
  <Provider store={store}>
    <AlertList />
    <App />
  </Provider>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
