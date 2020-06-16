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
export function selectService(service = null) {
    return {
        type: SELECT_SERVICE,
        service,
    };
}

export const CANCEL_VIEW_SERVICE = "CANCEL_VIEW_SERVICE";
export function cancelViewService() {
    return {
        type: CANCEL_VIEW_SERVICE,
    }
}

export const ADD_SERVICE = "ADD_SERVICE";
export function addService() {
    return {
        type: ADD_SERVICE
    }
}

export const EDIT_SERVICE = "EDIT_SERVICE";
export function editService(service) {
    return {
        type: EDIT_SERVICE,
        service
    }
}

export const CANCEL_EDIT_SERVICE = "CANCEL_EDIT_SERVICE";
export function cancelEditService(service) {
    return {
        type: CANCEL_EDIT_SERVICE,
        service
    }
}

// TODO Speicher Async
export const SAVE_SERVICE = "SAVE_SERVICE";
export function saveServiceAsync(service) {
    return (dispatch) => {
        dispatch(requestService());
        return API_Service.create()
            .then((res) => res.json())
            .then((json) => dispatch(receiveService(json)));
    };
}

// TODO Delete Async
export const DELETE_SERVICE = "DELETE_SERVICE";
export function deleteServiceAsync(serviceId) {
    return (dispatch) => {
        dispatch(requestService());
        return API_Service.create()
            .then((res) => res.json())
            .then((json) => dispatch(receiveService(json)));
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