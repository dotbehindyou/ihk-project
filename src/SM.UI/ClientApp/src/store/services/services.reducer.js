import {
  REQUEST_SERVICE,
  RECEIVE_SERVICE,
  SELECT_SERVICE,
} from "./services.actions";

const initialState = {
  services: [],
  isFetching: true,
  error: null,
  selected: {
    index: 0,
    service: null,
  },
};

export default function servicesReducer(state = initialState, action) {
  switch (action.type) {
    case REQUEST_SERVICE:
      return { ...state, isFetching: true };
    case RECEIVE_SERVICE:
      return { ...state, services: action.data, isFetching: false };
    case SELECT_SERVICE:
      return __select_service;
    default:
      return state;
  }
}

function __select_service(state, action) {
  if (action.index == null && action.service == null) return state;

  let service, index;

  if (action.index != null) {
    service = state.services[index];
  } else {
    state.services.forEach((x, key) => {
      if (x.module_ID === action.service.module_ID) {
        service = x;
        index = key;
      }
    });
  }

  return {
    ...state,
    selected: {
      index,
      service,
    },
  };
}
