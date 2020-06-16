//export const __api_url = new URL("/api/", window.location.origin);
export const __api_url = new URL("https://localhost/api/");

/** Service API */
export const API_Service = {
    __service_url: () => new URL("Modules/", __api_url),
    /** Gebe ALLE Dienste zurück */
    all: () =>
        fetch(API_Service.__service_url(), {
            method: "GET",
        }),
    /** Gebe die Daten vom bestimmten Dienst zurück */
    get: (serviceId) =>
        fetch(new URL(serviceId, API_Service.__service_url()), {
            method: "GET",
        }),
    /** Gebe dir alle Dienste vom Kunden zurück */
    fromCustomer: (kdnr) =>
        fetch(new URL(`Customer/${kdnr}`, API_Service.__service_url()), {
            method: "GET",
        }),
    /** Erstelle einen Dienst */
    create: (service) =>
        fetch(API_Service.__service_url(), {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(service),
        }),
    /** Veränder die Daten eines Dienstes */
    update: (serviceId, service) =>
        fetch(new URL(serviceId, API_Service.__service_url()), {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(service),
        }),
    /** Lösche einen Dienst */
    remove: (serviceId) =>
        fetch(new URL(serviceId, API_Service.__service_url()), {
            method: "DELETE",
        }),
};

/** Version API */
export const API_Version = {
    __version_url: (serviceId) => new URL(`${serviceId}/Versions/`, __api_url),
    /** Liste alle Versionen vom Dienst */
    all: (serviceId) =>
        fetch(API_Version.__version_url(serviceId), {
            method: "GET",
        }),
    /** Selektiere eine einzelne Version eines Dienstes */
    get: (serviceId, versionNr) =>
        fetch(new URL(versionNr, API_Version.__version_url(serviceId)), {
            method: "GET",
        }),
    /** Selektiere die Daten einer Version vom Kunden */
    getVersionFromCustomer: (serviceId, versionNr, kdnr) =>
        fetch(new URL(`${versionNr}/${kdnr}`, API_Version.__version_url(serviceId)), {
            method: "GET",
        }),
    /** Erstelle eine neue Version für den Dienst */
    create: (serviceId, version) =>
        fetch(API_Version.__version_url(serviceId), {
            method: "POST",
        }),
    /** Aktualisiere die Daten der Version */
    update: (serviceId, versionNr, version) =>
        fetch(new URL(versionNr, API_Version.__version_url(serviceId)), {
            method: "PUT",
        }),
    /** Lade (+Überschreibe) die Versionsdateien hoch  */
    uploadFile: (serviceId, versionNr, files) =>
        fetch(new URL(versionNr, API_Version.__version_url(serviceId)), {
            method: "PUT",
        }),
    /** Lösche die Version vom Dienst */
    delete: (serviceId, versionNr) =>
        fetch(new URL(versionNr, API_Version.__version_url(serviceId)), {
            method: "DELETE",
        }),
};