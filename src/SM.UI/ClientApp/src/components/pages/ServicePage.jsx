import React from "react";
import Content from "../../layout/Content";
import ServiceTable from "../service/ServiceTable";
import ServiceView from "../service/ServiceView";

class ServicePage extends React.Component {
  state = {
    selected: null, // { module_ID: "7ff26a94-2ecf-4773-8a68-6bbf3b91319b" }, // TODO Debug },
  };

  constructor(props) {
    super(props);

    this.openService = this.openService.bind(this);
  }

  openService(service) {
    this.setState({ selected: service });
  }

  render() {
    var selected = this.state.selected || {};
    var isSelected = this.state.selected == null;

    return (
      <div>
        <Content hide={isSelected}>
          <ServiceView
            serviceId={selected.module_ID}
            isNew={selected.version === "" || selected.version == null}
          />
        </Content>

        <Content>
          <h2>Dienste</h2>
          <ServiceTable onOpenService={this.openService} />
        </Content>
      </div>
    );
  }
}

export default ServicePage;
