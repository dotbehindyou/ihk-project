import React from "react";
import { Row, Col } from "antd";
import VersionTable from "../version/VersionTable";
import VersionView from "../version/VersionView";
import { CloseOutlined } from "@ant-design/icons";
import { Button } from "antd";
import { connect } from "react-redux";
import { cancelViewService } from "../../../store/services/services.actions";

const mapStateToProps = (state) => ({
  selected: state.services.selected,
});

const mapDispatchToProps = (dispatch) => ({
  closeServiceView: () => dispatch(cancelViewService())
});

const connector = connect(mapStateToProps, mapDispatchToProps);

export class ServiceView extends React.Component {
  constructor(props) {
    super(props);

    this.openVersion = this.openVersion.bind(this);
    this.handleCloseWindow = this.handleCloseWindow.bind(this);
  }

  openVersion(version) {
    
  }

  handleCloseWindow(){
    this.props.closeServiceView();
  }

  render() {
    let service = this.props.selected.service;
    return (
      <Row>
        <Col span={24}>
          <Row>
            <Col span={20}>
              <h2>{service.name}</h2>
            </Col>
            <Col span={4} style={{textAlign: "right"}}>
              <Button type="primary" onClick={this.handleCloseWindow}><CloseOutlined /></Button>
            </Col>
          </Row>
        </Col>
        <Col span={12} style={{ paddingRight: 20 }}>
          <Row>
            <Col span={24}>
              <VersionTable
                serviceId={service.module_ID}
                onOpenVersion={this.openVersion}
              />
            </Col>
          </Row>
        </Col>
        <Col
          span={12}
          style={{ borderLeft: "solid black 1px", paddingLeft: 20 }}
        >
          {service.version != null ? (
            <VersionView
              serviceId={service.module_ID}
              version={service.version}
            ></VersionView>
          ) : null}
        </Col>
      </Row>
    );
  }
}
export default connector(ServiceView);

