import {
  REQUEST_SERVICE,
  RECEIVE_SERVICE,
  SELECT_SERVICE,
  ADD_SERVICE,
  CANCEL_EDIT_SERVICE,
  CANCEL_VIEW_SERVICE,
} from "./services.actions";
import { EmptyUUID } from "../../helper/uuid";

const initialState = {
  services: [],
  isFetching: true,
  error: null,
  isEditing: false,
  selected: {
    index: -1,
    service: null,
  },
};

function __get_new_service(){
  return {
    module_ID: EmptyUUID,
    name: '',
    isNew: true
  }
}

function __get_index(state, service){
  for(var i = 0; i < state.services.length; ++i){
    let x = state.services[i];
    if (x.module_ID === service.module_ID) {
      return i;
    }
  }
}

function __select_service(state, action) {
  if (action.service == null) return state;

  let service = action.service, 
    index = __get_index(state, action.service);

  return {
    ...state,
    selected: {
      index,
      service,
    },
  };
}

function __cancel_edit_service(state, action){
  let serList, service = action.service;
  if(service.isNew === true){
    serList = state.services.filter(x=> x.module_ID !== service.module_ID);
  }
  else{
    serList = state.services.map(x=> { x.isEditing = false; return x; });
  }
  return {
    ...state,
    services: serList,
    isEditing: false
  }
}

export default function servicesReducer(state = initialState, action) {
  switch (action.type) {
    case REQUEST_SERVICE:
      return { ...state, isFetching: true };
    case RECEIVE_SERVICE:
      return { ...state, services: action.data, isFetching: false };
    case SELECT_SERVICE:
      return __select_service(state, action);
    case ADD_SERVICE:
      return { ...state, services:[...state.services, __get_new_service()],isEditing: true };
    case CANCEL_EDIT_SERVICE:
      return __cancel_edit_service(state, action);
    case CANCEL_VIEW_SERVICE:
      return { ...state, selected: { index: -1, service: null } };
    default:
      return state;
  }
}
