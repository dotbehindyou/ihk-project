import React from "react";
import __api_helper from "../../helper/__api_helper";
import { Row, Col } from "antd";
import VersionTable from "../version/VersionTable";
import VersionView from "../version/VersionView";

class ServiceView extends React.Component {
  helper = new __api_helper.API_Services();

  state = {
    module_ID: "",
    name: "",
    status: "",
    validation_Token: false,
    version: null,
    config: {},
  };

  constructor(props) {
    super(props);

    this.state.module_ID = props.serviceId;
    this.openVersion = this.openVersion.bind(this);
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if (prevState.module_ID === nextProps.serviceId) return null;
    return {
      module_ID: nextProps.serviceId,
    };
  }

  async loadServiceData() {
    let service;
    service = await this.helper.getService(this.props.serviceId);
    this.setState({ ...service });
  }

  componentDidMount() {
    this.loadServiceData();
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.module_ID !== this.state.module_ID) {
      this.loadServiceData();
    }
  }

  openVersion(version) {
    this.setState({ version: version.version });
  }

  render() {
    if (!this.state.module_ID) return <h2>Fehler</h2>;
    return (
      <Row>
        <Col span={12} style={{ paddingRight: 20 }}>
          <Row>
            <Col span={24}>
              <h2>
                {this.state.name} {this.state.module_ID}
              </h2>
            </Col>
          </Row>
          <Row>
            <Col span={24}>
              <VersionTable
                serviceId={this.state.module_ID}
                onOpenVersion={this.openVersion}
              />
            </Col>
          </Row>
        </Col>
        <Col
          span={12}
          style={{ borderLeft: "solid black 1px", paddingLeft: 20 }}
        >
          {this.state.version != null ? (
            <VersionView
              serviceId={this.state.module_ID}
              version={this.state.version}
            ></VersionView>
          ) : null}
        </Col>
      </Row>
    );
  }
}

export default ServiceView;
