import thunk from "redux-thunk";
import { createLogger } from "redux-logger";
import { createStore, combineReducers, applyMiddleware } from "redux";

import alertReducer from "./alerts/alerts.reducer";
import servicesReducer from "./services/services.reducer";
import { fetchAllServiceAsync } from "./services/services.actions";

const rootReducer = combineReducers({
  alerts: alertReducer,
  services: servicesReducer,
});

const loggerMiddleware = createLogger();

const store = createStore(
  rootReducer,
  applyMiddleware(
    thunk, // lets us dispatch() functions
    loggerMiddleware // neat middleware that logs actions
  )
);

store.dispatch(fetchAllServiceAsync());

export default store;
