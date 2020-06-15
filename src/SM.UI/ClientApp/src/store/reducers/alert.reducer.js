const alertReducer = (state = [], action) => {
  switch (action.type) {
    case "ADD_ALERT":
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
    case "REM_ALERT":
      return state.filter((x, key) => x.id !== action.id);
    default:
      return state;
  }
};

export default alertReducer;
