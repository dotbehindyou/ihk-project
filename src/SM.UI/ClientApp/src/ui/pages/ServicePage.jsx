import React from "react";
import Content from "../layout/Content";
import ServiceTable from "../components/service/ServiceTable";
import ServiceView from "../components/service/ServiceView";
import { connect } from "react-redux";

const mapStateToProps = (state) => ({
  selected: state.services.selected,
});

const mapDispatchToProps = (dispatch) => ({
  //fetchAll: () => dispatch(fetchAllServiceAsync()),
});

const connector = connect(mapStateToProps, mapDispatchToProps);

export class ServicePage extends React.Component {

  render() {
    let isSelected = this.props.selected.service != null;

    return (
      <div>
        <Content hide={!isSelected}>
          <ServiceView />
        </Content>

        <Content>
          <h2>Dienste</h2>
          <ServiceTable />
        </Content>
      </div>
    );
  }
}

export default connector(ServicePage);
