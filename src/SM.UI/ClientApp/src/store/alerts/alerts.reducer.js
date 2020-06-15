import { ADD_ALERT, REM_ALERT } from "./alerts.actions";

const ALERTS_INIT_STATE = [];

const alertReducer = (state = ALERTS_INIT_STATE, action) => {
  switch (action.type) {
    case ADD_ALERT:
      return [
        ...state,
        {
          id: action.id,
          text: action.text,
          typ: action.typ,
          icon: action.icon,
          seen: false,
        },
      ];
    case REM_ALERT:
      return state.filter((x, key) => x.id !== action.id);
    default:
      return state;
  }
};

export default alertReducer;
