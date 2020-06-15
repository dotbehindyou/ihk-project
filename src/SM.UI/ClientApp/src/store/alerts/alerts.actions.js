let alertId = 0;

export const ADD_ALERT = "ADD_ALERT";
export const REM_ALERT = "REM_ALERT";

export function AddAlert(text, icon, typ) {
  return {
    type: ADD_ALERT,
    id: alertId++,
    text,
    icon,
    typ,
  };
}

export function RemoveAlert(id) {
  return {
    type: REM_ALERT,
    id,
  };
}
