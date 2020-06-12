import React from "react";
import __api_helper from "../../helper/__api_helper";
import { Row, Col, DatePicker, Input, Button } from "antd";

import ConfigEditor from "./ConfigEditor";

class VersionView extends React.Component {
  helper = new __api_helper.API_Version();

  state = {
    module_ID: "",
    moduleName: "",
    releaseDate: "",
    name: "",
    status: "",
    validationToken: "",
    version: "",
    file: null,
    config: {},
  };

  constructor(props) {
    super(props);

    this.state.module_ID = props.serviceId;
    this.changeValue = this.changeValue.bind(this);
    this.save = this.save.bind(this);
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if (prevState.version === nextProps.version) return null;
    return {
      ...nextProps.version,
    };
  }

  componentDidMount() {
    this.loadVersionData();
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.version !== this.props.version) {
      this.loadVersionData();
    }
  }

  async loadVersionData() {
    let service;
    if (this.props.version === "" || this.props.version == null) return;
    if (this.props.kdnr) {
      console.log(this.props);
      service = await this.helper.getVersionFromCustomer(
        this.props.serviceId,
        this.props.version,
        this.props.kdnr
      );
    } else {
      if (this.props.isNew) {
        // TODO Create Service
        //customer = await this.helper.initCustomer(this.props.kdnr);
      } else {
        service = await this.helper.getVersion(
          this.props.serviceId,
          this.props.version
        );
      }
    }
    this.setState({ ...service });
  }

  changeValue(e) {
    this.setState({ releaseDate: e });
  }

  save(e) {
    this.helper.save(this.props.serviceId, this.state.version, {
      ...this.state,
    });
    if (this.state.file) {
      console.log(this.state.file);
      this.helper.uploadFile(
        this.props.serviceId,
        this.state.version,
        this.state.file
      );
    }
  }

  render() {
    if (!this.state.version) return <h2>Fehler</h2>;
    return (
      <div>
        <Row>
          <Col>
            <h3>Version: {this.state.version}</h3>
          </Col>
        </Row>
        <Row>
          <Col span={24}>
            <DatePicker
              style={{ width: "100%" }}
              format="DD.MM.YYYY"
              value={this.state.releaseDate}
              onChange={this.changeValue}
            />
          </Col>
        </Row>
        <hr />
        <Row>
          <Col span={24}>
            <h4>Versondatei (ZIP):</h4>
          </Col>
          <Col span={24}>Konfig-Datei wird überschrieben!</Col>
          <Col>
            <Input
              value={this.state.file}
              type="file"
              onChange={({ target: { value } }) =>
                this.setState({ file: value })
              }
            />
          </Col>
        </Row>
        <hr />
        <Row>
          <Col span={24}>
            <ConfigEditor
              isNew={this.props.isNew}
              serviceId={this.props.serviceId}
              version={this.props.version}
              onChange={(config) => this.setState({ config })}
            />
          </Col>
        </Row>
        <Row>
          <Col>
            <Button onClick={this.save}>Speichern</Button>
          </Col>
        </Row>
        {this.props.children}
      </div>
    );
  }
}

export default VersionView;
