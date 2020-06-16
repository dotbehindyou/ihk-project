import { API_Version } from "../__api";

export const REQUEST_VERSION = "REQUEST_VERSION";

function requestVersion(serviceId) {
    return {
        type: REQUEST_VERSION,
        id: serviceId
    };
}
export const RECEIVE_VERSION = "RECEIVE_VERSION";

function receiveVersion(versionList) {
    return {
        type: RECEIVE_VERSION,
        data: versionList
    };
}
export const ADD_VERSION = "ADD_VERSION";
export function addVersion() {
    return {
        type: ADD_VERSION,
    }
}
export const EDIT_VERSION = "EDIT_VERSION";
export function editVersion(editVersion) {
    return {
        type: EDIT_VERSION,
    }
}

export const REMOVE_VERSION = "REMOVE_VERSION";
export function removeVersion(versionNr) {
    return {
        type: REMOVE_VERSION,
        versionNr
    }
}

export const SELECT_VERSION = "SELECT_VERSION";
export function selectVersion(versionNr) {
    return {
        type: SELECT_VERSION,
        versionNr
    }
}

export function changeService(serviceId) {
    return fetchAll(serviceId);
}

/** Lade die Daten von der API  */
export function fetchAll(serviceId) {
    return (dispatch) => {
        dispatch(requestVersion());
        return API_Version.all(serviceId)
            .then((res) => res.json())
            .then((json) => dispatch(receiveVersion(json)));
    };
}