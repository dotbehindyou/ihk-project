import { REQUEST_VERSION, RECEIVE_VERSION, ADD_VERSION, REMOVE_VERSION, SELECT_VERSION } from "./versions.action";

const initialState = {
    serviceId: null,
    isEditing: false,
    isFetching: false,
    versions: [],
    selected: {
        index: -1,
        version: null
    }
};

function __get_new_version(serviceId) {
    return {
        module_ID: serviceId,
        version: '',
        name: '',
        isNew: true
    }
}

function __get_index(state, versionNr) {
    for (var i = 0; i < state.versions.length; ++i) {
        let x = state.versions[i];
        if (x.version === versionNr) {
            return i;
        }
    }
}

export default function versionsReducer(state = initialState, action) {
    switch (action.type) {
        case REQUEST_VERSION:
            return {...state, serviceId: action.id, isFetching: true };
        case RECEIVE_VERSION:
            return {...state, isFetching: false, versions: action.data };
        case ADD_VERSION:
            return {...state, versions: [...state.versions, __get_new_version(state.serviceId)] };
        case REMOVE_VERSION:
            return {...state, versions: state.versions.filter(x => x.version !== action.versionNr) }
        case SELECT_VERSION:
            let index = __get_index(state, action.versionNr);
            return {...state, selected: { index, version: state.versions[index] } };
        default:
            return state;
    }
}