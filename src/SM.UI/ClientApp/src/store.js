import alert from './reducers/alert'
import { createStore } from 'redux';

const store = createStore(alert);

export default store;