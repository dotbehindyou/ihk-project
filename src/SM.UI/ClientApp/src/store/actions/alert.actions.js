let alertId = 0;

export function AddAlert(text, icon, typ){  
    return ({
        type: 'ADD_ALERT',
        id: alertId++,
        text,
        icon,
        typ
    });
}

export function RemoveAlert(id){
    return ({
        type: 'REM_ALERT',
        id
    });
}
