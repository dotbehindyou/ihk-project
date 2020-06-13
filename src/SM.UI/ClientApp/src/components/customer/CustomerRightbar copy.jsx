import React from "react";
import __api_helper from "../../helper/__api_helper";
import { Col } from "antd";
import ServiceTable from "../service/ServiceTable";
import ConfigEditor from "../version/ConfigEditor";
import VersionTable from "../version/VersionTable";

class CustomerRightbar extends React.Component {
  helper = new __api_helper.API_Customer();

  state = {
    selected: null, //Bei Version wächsel das befüllen!!!!
    addService: this.props.addService,
  };

  constructor(props) {
    super(props);

    this.selectService = this.selectService.bind(this);
    this.selectVersion = this.selectVersion.bind(this);
    this.save = this.save.bind(this);
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if (nextProps.selected != null) {
      let sele = prevState.selected || {};
      if(nextProps.selected.version !== sele.version){
        return {
          selected: nextProps.selected
        }
      }
    }
    if (prevState.addService === nextProps.addService) return null;
    return {
      addService: nextProps.addService,
      selected: nextProps.addService ? null : nextProps.selected,
    };
  }

  componentDidMount() {}

  save(config) {
    let version = {
      kdnr: this.props.kdnr,
      module_ID: this.state.selected.module_ID,
      version: this.state.selected.version,
      status: this.state.selected.status,
      config,
    };
    if (this.state.selected.isNew) {
      this.helper.addService(this.props.kdnr, version.module_ID, version);
    } else {
      this.helper.updateServiceInformation(
        this.props.kdnr,
        version.module_ID,
        version
      );
    }
  }

  selectService(o) {
    this.setState({ selected: o });
  }

  selectVersion(o) {
    let ver = this.state.selected;
    ver.version = o.version;

    this.setState({ selected: { ...ver, isNew: true }, addService: false });

    if (this.props.onVersionSelected) this.props.onVersionSelected({ ...o });
  }

  render() {
    let selected = this.state.selected || null;

    if (this.state.addService) {
      if (selected == null) {
        return (
          <div>
            <ServiceTable onOpenService={this.selectService} onlySelect />
          </div>
        );
      } else {
        return (
          <div>
            <h4>{selected.name}</h4>
            <VersionTable
              onlySelect
              serviceId={selected.module_ID}
              onOpenVersion={this.selectVersion}
            />
          </div>
        );
      }
    } else if (
      selected != null &&
      selected.module_ID != null &&
      selected.version != null
    ) {
      return (
        <div>
          <h3>
            {selected.name}({selected.version})
          </h3>
          <ConfigEditor
            onSave={this.save}
            hideFileInfo
            kdnr={this.state.kdnr}
            serviceId={selected.module_ID}
            version={selected.version}
          ></ConfigEditor>
        </div>
      );
    }
    console.log(this.props.selected);
    return <h2>Fehler</h2>; // TODO DEBUG return null;
  }
}

export default CustomerRightbar;
