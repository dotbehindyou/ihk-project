import moment from "moment";
import alertStore from "../store";
import { AddAlert } from "../actions/alert.actions";

const __url = new URL("https://localhost:44337/api/");

class __API {
  async get(url, init, isJsonResult = true) {
    return await fetch(url, init)
      .then((res) => {
        try {
          if (isJsonResult) {
            return res.json();
          }
        } catch {}
        return res.text();
      })
      .catch((ex) => {
        alertStore.dispatch(
          AddAlert("Fehler beim abrufen der API!", true, "error")
        );
        throw ex;
      });
  }
}

class API_Customer extends __API {
  api_url = new URL("Customers", __url);

  async getCustomers() {
    return super.get(this.api_url, {
      method: "GET",
    });
  }

  async getCustomer(kdnr) {
    let url = new URL("Customers/" + kdnr, this.api_url);
    return super.get(url, {
      method: "GET",
    });
  }

  async initCustomer(kdnr) {
    let url = new URL("Customers/" + kdnr, this.api_url);
    return super
      .get(url, {
        method: "PUT",
      })
      .then((res) => {
        alertStore.dispatch(
          AddAlert(
            "Kunde mit der Nr. '" + kdnr + "' wurde erfolgreich Initialisiert.",
            true,
            "success"
          )
        );
        return res;
      });
  }

  addService(kdnr, serviceId, service) {
    console.log(serviceId);
    let url = new URL("Modules/Customer/" + kdnr, __url);
    return super
      .get(
        url,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(service),
        },
        false
      )
      .then((res) => {
        alertStore.dispatch(
          AddAlert("Dienst wurde zum Kunden hinzugefügt.", true, "success")
        );
        return res;
      });
  }

  updateServiceInformation(kdnr, serviceId, service) {
    console.log(serviceId);
    let url = new URL("Modules/Customer/" + kdnr, __url);
    return super
      .get(
        url,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(service),
        },
        false
      )
      .then((res) => {
        alertStore.dispatch(
          AddAlert("Daten wurden gespeichert.", true, "success")
        );
        return res;
      });
  }
}

class API_Services extends __API {
  api_url = new URL("Modules", __url);
  getServices() {
    let url = this.api_url;
    return super.get(url, {
      method: "GET",
    });
  }
  getServicesFromCustomer(kdnr) {
    let url = new URL("Modules/Customer/" + kdnr, this.api_url);
    return super.get(url, {
      method: "GET",
    });
  }
  getService(serviceId) {
    let url = new URL("Modules/" + serviceId, this.api_url);
    return super.get(url, {
      method: "GET",
    });
  }
  createService(service) {
    let url = new URL("Modules", this.api_url);
    service.module_ID = "00000000-0000-0000-0000-000000000000";
    return super
      .get(url, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(service),
      })
      .then((res) => {
        alertStore.dispatch(
          AddAlert(
            "Service '" + res.name + "' wurde erfolgreich hinzugefügt!",
            true,
            "success"
          )
        );
        return res;
      });
  }
  deleteService(service) {
    let url = new URL("Modules/" + service.module_ID, this.api_url);
    return super
      .get(
        url,
        {
          method: "DELETE",
        },
        false
      )
      .then((res) => {
        alertStore.dispatch(
          AddAlert(
            "Service '" + service.name + "' wurde erfolgreich gelöscht!",
            true,
            "warning"
          )
        );
        return res;
      });
  }
}

//:module_ID/Versions/:versionsID
class API_Version extends __API {
  api_url = new URL("Versions", __url);

  getVersionsFromService(serviceId) {
    let url = new URL(serviceId + "/Versions", this.api_url);
    return super
      .get(url, {
        method: "GET",
      })
      .then((x) => {
        if (x == null) return null;
        if (x.forEach !== undefined) {
          x.forEach((i) => {
            i.releaseDate = moment(i.releaseDate || "");
          });
        }
        return x;
      });
  }

  getVersionFromCustomer(serviceId, version, kdnr) {
    let url = new URL(
      serviceId + "/Versions/" + version + "/" + kdnr,
      this.api_url
    );
    return super
      .get(url, {
        method: "GET",
      })
      .then((x) => {
        x.releaseDate = moment(x.releaseDate || "");
        return x;
      });
  }

  getVersion(serviceId, version) {
    let url = new URL(serviceId + "/Versions/" + version, this.api_url);
    return super
      .get(url, {
        method: "GET",
      })
      .then((x) => {
        x.releaseDate = moment(x.releaseDate || "");
        return x;
      });
  }

  create(serviceId, data) {
    data.releaseDate = data.releaseDate.format("YYYY-MM-DD");
    let url = new URL(serviceId + "/Versions/", __url);
    return super
      .get(
        url,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        },
        false
      )
      .then((res) => {
        alertStore.dispatch(
          AddAlert("Version wurde hinzugefügt", true, "success")
        );
        return res;
      });
  }

  save(serviceId, version, data) {
    data.releaseDate = data.releaseDate.format("YYYY-MM-DD");
    let url = new URL(serviceId + "/Versions/" + version, __url);
    return super
      .get(
        url,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        },
        false
      )
      .then((res) => {
        alertStore.dispatch(
          AddAlert(
            "Änderungen der Informationen wurden gespeichert.",
            true,
            "success"
          )
        );
        return res;
      });
  }

  uploadFile(serviceId, version, file) {
    let url = new URL(
      serviceId + "/versions/" + version + "/file",
      this.api_url
    );

    var formData = new FormData();
    formData.append("versionFile", file);

    return super
      .get(url, {
        method: "PUT",
        cache: "no-cache",
        body: formData,
      })
      .then((res) => {
        alertStore.dispatch(
          AddAlert(
            "Datei für die Version '" +
              version +
              "' wurde erfolgreich hochgeladen",
            true,
            "warning"
          )
        );
        return res;
      });
  }

  deleteVersion(version) {
    let url = new URL(
      version.module_ID + "/Versions/" + version.version,
      this.api_url
    );
    return super
      .get(
        url,
        {
          method: "DELETE",
        },
        false
      )
      .then((res) => {
        alertStore.dispatch(
          AddAlert(
            "Version '" + version.version + "' wurde erfolgreich gelöscht!",
            true,
            "warning"
          )
        );
        return res;
      });
  }
}

export default { API_Customer, API_Services, API_Version };
