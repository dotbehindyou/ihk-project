//export const __api_url = new URL("/api/", window.location.origin);
export const __api_url = new URL("https://localhost/api/");

/**  */
export const API_Service = {
  service_url: new URL("Modules/", __api_url),
  /** Gebe ALLE Dienste zurück */
  all: () =>
    fetch(API_Service.service_url, {
      method: "GET",
    }),
  /** Gebe die Daten vom bestimmten Dienst zurück */
  get: (serviceId) =>
    fetch(new URL(serviceId, API_Service.service_url), {
      method: "GET",
    }),
  /** Gebe dir alle Dienste vom Kunden zurück */
  fromCustomer: (kdnr) =>
    fetch(new URL(`Customer/${kdnr}`, API_Service.service_url), {
      method: "GET",
    }),
  /** Erstelle einen Dienst */
  create: (service) =>
    fetch(API_Service.service_url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(service),
    }),
  /** Veränder die Daten eines Dienstes */
  update: (serviceId, service) =>
    fetch(new URL(serviceId, API_Service.service_url), {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(service),
    }),
  /** Lösche einen Dienst */
  remove: (serviceId) =>
    fetch(new URL(serviceId, API_Service.service_url), {
      method: "DELETE",
    }),
};
