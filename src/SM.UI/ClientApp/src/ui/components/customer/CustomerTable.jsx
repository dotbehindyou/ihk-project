import React from "react";
import { Table } from "antd";
import __api_helper from "../../../helper/__api_helper";
import { connect } from "react-redux";
import { fetchAll } from "../../../store/versions/versions.action";

const mapStateToProps = (state) => ({
  selected: state.services.selected,
});

const mapDispatchToProps = (dispatch) => ({
  fetchVersions: (serviceId) => dispatch(fetchAll(serviceId))
});

const connector = connect(mapStateToProps, mapDispatchToProps);

export class CustomerTable extends React.Component {
  columns = [
    {
      title: "Kunden Nr",
      dataIndex: "kdnr",
      key: "kdnr",
      width: 100,
    },
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Ist Initialisiert?",
      dataIndex: "isRegisterd",
      key: "isRegisterd",
      width: 120,
      render: (ir) => <span>{ir ? "Ja" : "Nein"}</span>,
    },
  ];

  static getSta

  render() {
    return (
      <Table
        bordered
        onRow={this.rowEvent}
        rowClassName={(record, index) => ["row"]}
        emptyText="Keine Daten vorhanden"
        rowKey="kdnr"
        size="small"
        columns={this.columns}
        dataSource={this.state.customerList}
      />
    );
  }
}

export default connector(CustomerTable);
