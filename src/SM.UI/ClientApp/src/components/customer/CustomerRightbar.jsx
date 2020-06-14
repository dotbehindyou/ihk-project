import React from "react";
import __api_helper from "../../helper/__api_helper";
import ServiceTable from "../service/ServiceTable";
import ConfigEditor from "../version/ConfigEditor";
import VersionTable from "../version/VersionTable";
import { Button } from "antd";

class CustomerRightbar extends React.Component {
  helper = new __api_helper.API_Customer();

  state = {
    selected: this.props.selected
  };

  constructor(props){
    super(props);

    this.selectService = this.selectService.bind(this);
    this.selectVersion = this.selectVersion.bind(this);
    this.changeVersion = this.changeVersion.bind(this);
    this.save = this.save.bind(this);
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if(nextProps.selected != null){
      let sel = prevState.selected || { };
      if(nextProps.addService === false && prevState.addService !== false){
        return {
          selected: {...nextProps.selected},
          addService: false
        } 
      }
      if(sel.version !== nextProps.selected.version){
        return {
          selected: {...nextProps.selected}
        }
      }
    }
    else if(nextProps.addService === true && prevState.addService !== true){
      return {
        selected: null,
        addService: true,
      };
    } 

    return null;
  }
  componentDidMount() {}

  selectService(e){
    this.setState({selected: e});
  }

  selectVersion(e){
    let sel = this.state.selected;
    sel.version = e.version;
    if(this.props.onVersionSelected){
      this.props.onVersionSelected(e);
    }
    this.setState({selected: {...sel, isAdd: true}, addService: false});
  }

  changeVersion(e){
    let sel = this.state.selected;
    this.setState({changeVersion: true, selected: {...sel}});
  }

  save(config){
    let sel = this.state.selected;
    sel.config = config;
    if(sel.isAdd){  
      this.helper.addService(this.props.kdnr, sel.module_ID, sel);
    }else{
      this.helper.updateServiceInformation(this.props.kdnr, sel.module_ID, sel);
    }
  }

  render() {
    let selected = this.state.selected || null;
    if(this.state.addService || this.state.changeVersion){
      if(selected === null){
        return (
          <div>
            <ServiceTable onOpenService={this.selectService} onlySelect />
          </div>);
      }else{
        return (
          <div>
            <h4>{selected.name}</h4>
            <VersionTable
              onlySelect
              serviceId={selected.module_ID}
              onOpenVersion={this.selectVersion}
            />
          </div>);
      }
    }
    else if (
      selected != null &&
      selected.module_ID != null &&
      selected.version != null
    ) {
      return (
        <div>
          <h3>
            {selected.name}({selected.version})
          </h3>
          <Button onClick={this.changeVersion}>Version ändern</Button>
          <ConfigEditor
            isNew={selected.isAdd}
            onSave={this.save}
            hideFileInfo   
            kdnr={this.props.kdnr}
            serviceId={selected.module_ID}
            version={selected.version}
          ></ConfigEditor>
        </div>
      );
    }

    return <h2>Fehler</h2>; // TODO DEBUG return null;
  }
}

export default CustomerRightbar;
