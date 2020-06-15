import { createStore, combineReducers } from "redux";

import alertReducer from "./reducers/alert.reducer";

const rootReducer = combineReducers({
  alerts: alertReducer,
});

const store = createStore(rootReducer);

export default store;
