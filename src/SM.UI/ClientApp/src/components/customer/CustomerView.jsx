import React from "react";
import __api_helper from "../../helper/__api_helper";
import { Input, Button, Row, Col } from "antd";
import ServiceTable from "../service/ServiceTable";
import CustomerRightbar from "./CustomerRightbar";

class CustomerView extends React.Component {
  helper = new __api_helper.API_Customer();

  state = {
    kdnr: 0,
    name: "",
    auth_Token: "",
    isRegisterd: false,
    selected: {},
    addService: false,
  };

  authInputRef = React.createRef();

  constructor(props) {
    super(props);

    this.state.kdnr = props.kdnr;
    this.copyAuthToken = this.copyAuthToken.bind(this);
    this.openService = this.openService.bind(this);
    this.handleAddService = this.handleAddService.bind(this);
    this.onNewServiceSelected = this.onNewServiceSelected.bind(this);
    this.handleDelete = this.handleDelete.bind(this);
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if (prevState.kdnr === nextProps.kdnr) return null;
    return {
      kdnr: nextProps.kdnr,
      selected: {},
      addService: false
    };
  }

  componentDidMount() {
    this.loadCustomerData();
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.kdnr !== this.state.kdnr) {
      this.loadCustomerData();
    }
  }

  async loadCustomerData() {
    let customer;
    if (this.props.isNew) {
      customer = await this.helper.initCustomer(this.props.kdnr);
    } else {
      customer = await this.helper.getCustomer(this.props.kdnr);
    }
    this.setState({ ...customer });
  }

  copyAuthToken(e) {
    let ir = this.authInputRef.current;
    ir.focus();
    ir.select();
    document.execCommand("copy");
    e.target.focus();
  }

  openService(service) {
    this.setState({ selected: service, addService: false });
  }

  handleAddService(e) {
    this.setState({ addService: true, selected: null });
  }
  
  handleDelete(e) {
    this.helper.removeService(this.state.kdnr, e.module_ID, e);
  }

  onNewServiceSelected(e) {
    this.setState({ addService: false, selected: e });
    console.log(e);
  }

  render() {
    if (!this.state.kdnr) {
      return <h2>Fehler</h2>;
    }

    return (
      <Row>
        <Col span={8} style={{ paddingRight: 20 }}>
          <Row>
            <Col>
              <h2>{this.state.name}</h2>
            </Col>
          </Row>
          <Row>
            <Col span={24}>
              <Row>
                <Col span={24}>
                  <label>Authentifizierungstoken</label>
                </Col>
              </Row>
              <Row>
                <Col span={24}>
                  <Input.Group
                    style={{ width: "100%" }}
                    name="auth_token"
                    label="Authentifikations Token"
                  >
                    <Input
                      ref={this.authInputRef}
                      readOnly
                      style={{ width: "70%" }}
                      value={this.state.auth_Token}
                    />
                    <Button
                      style={{ width: "30%" }}
                      onClick={this.copyAuthToken}
                    >
                      Kopieren
                    </Button>
                  </Input.Group>
                </Col>
              </Row>
            </Col>
          </Row>
          <hr />
          <Row>
            <Col span={24}>
              <ServiceTable
                kdnr={this.props.kdnr}
                onOpenService={this.openService}
                onDelete={this.handleDelete}
                onAddService={this.handleAddService}
                getFirstRow={(r) => this.setState({ selected: r })}
              />
            </Col>
          </Row>
        </Col>
        <Col
          span={15}
          style={{ borderLeft: "solid black 1px", paddingLeft: 20 }}
        >
          <CustomerRightbar
            kdnr={this.props.kdnr}
            onVersionSelected={this.onNewServiceSelected}
            selected={this.state.selected}
            addService={this.state.addService}
          />
        </Col>
      </Row>
    );
  }
}

export default CustomerView;
