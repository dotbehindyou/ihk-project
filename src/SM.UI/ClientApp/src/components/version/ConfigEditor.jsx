import React from "react";
import __api_helper from "../../helper/__api_helper";
import { Row, Col, Input, Button, Select } from "antd";

import Editor from "react-simple-code-editor";
import { highlight, languages } from "prismjs";
import "prismjs/components/prism-json";
import "prismjs/components/prism-ini";

import "prismjs/themes/prism-dark.css";

class ConfigEditor extends React.Component {
  helper = new __api_helper.API_Version();
  f_highlight = (data) => {
    if (this.state.format) {
      return highlight(data, languages[this.state.format]);
    }
    return data;
  };

  state = {
    config_ID: "",
    data: "",
    fileName: "",
    format: "",
  };

  constructor(props) {
    super(props);

    this.state.module_ID = props.serviceId;
    this.changeValue = this.changeValue.bind(this);
    this.handleSave = this.handleSave.bind(this);
    this.f_highlight = this.f_highlight.bind(this);
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if (prevState.version === nextProps.version) return prevState; // TODO Was Passiert wenn NULL zurück gegeben wird, wird DidUpdate ausgelöst?
    return {
      version: nextProps.version,
    };
  }

  componentDidMount() {
    this.loadConfig();
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.version !== this.state.version) {
      this.loadConfig();
    }
  }

  async loadConfig() {
    let service;
    if(this.props.version === "")
      return null;
    if(this.props.isNew !== true && this.props.isChanged !== true && this.props.kdnr){
      service = await this.helper.getVersionFromCustomer(
        this.props.serviceId,
        this.props.version,
        this.props.kdnr
      );
    }else{      
      service = await this.helper.getVersion(
        this.props.serviceId,
        this.props.version
      );
    }
    
    this.setState({ ...service.config });
  }

  changeValue(key, value) {
    let st = {};
    st[key] = value;
    this.setState(st);

    if (this.props.onChange) {
      st = { ...this.state, ...st };
      this.props.onChange(st);
      console.log();
    }
  }

  handleSave(e) {
    if (this.props.onSave) this.props.onSave({ ...this.state });
  }

  render() {
    if (!this.state.version) return <h2>Fehler</h2>;
    return (
      <div>
        <Row>
          <Col>
            <h4>Config</h4>
          </Col>
        </Row>
        <Row hidden={this.props.hideFileInfo}>
          <Col span={16} style={{ paddingBottom: 5 }}>
            <Input
              value={this.state.fileName}
              onChange={(eve) =>
                this.changeValue("fileName", eve.currentTarget.value)
              }
            />
          </Col>
          <Col span={8} style={{ paddingLeft: 5 }}>
            <Select
              style={{ width: "100%" }}
              value={this.state.format}
              onChange={(formatData) => {
                this.changeValue("format", formatData);
              }}
            >
              <Select.Option value="json">JSON</Select.Option>
              <Select.Option value="xml">XML</Select.Option>
              <Select.Option value="ini">INI</Select.Option>
              <Select.Option value="">
                <i>Leer</i>
              </Select.Option>
            </Select>
          </Col>
        </Row>
        <Row>
          <Col span={24}>
            <Editor
              value={this.state.data}
              padding={10}
              onValueChange={(data) => this.changeValue("data", data)}
              highlight={this.f_highlight}
              className="ant-input"
            >
              {this.state.data}
            </Editor>
          </Col>
        </Row>
        <Row hidden={!this.props.onSave}>
          <Col style={{ padding: "10 5" }}>
            <Button onClick={this.handleSave}>Speichern</Button>
          </Col>
        </Row>
      </div>
    );
  }
}

export default ConfigEditor;
