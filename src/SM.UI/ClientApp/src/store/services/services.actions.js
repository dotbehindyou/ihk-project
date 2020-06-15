import { API_Service } from "../__api";

export const REQUEST_SERVICE = "REQUEST_SERVICES";
function requestService() {
  return {
    type: REQUEST_SERVICE,
  };
}
export const RECEIVE_SERVICE = "RECEIVE_SERVICES";
function receiveService(serviceList) {
  return {
    type: RECEIVE_SERVICE,
    data: serviceList,
  };
}
export const SELECT_SERVICE = "SELECT_SERVICE";
export function selectService(index = null, service = null) {
  return {
    type: SELECT_SERVICE,
    index,
    service,
  };
}

/** Lade die Daten von der API  */
export function fetchAllServiceAsync() {
  return (dispatch) => {
    dispatch(requestService());
    return API_Service.all()
      .then((res) => res.json())
      .then((json) => dispatch(receiveService(json)));
  };
}
