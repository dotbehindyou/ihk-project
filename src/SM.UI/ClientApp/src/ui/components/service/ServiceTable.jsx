import React from "react";
import { Table, Button } from "antd";

import { connect } from "react-redux";
import { fetchAllServiceAsync } from "../../../store/services/services.actions";
import TableOperator from "../common/TableOperator";

const mapStateToProps = (state) => ({
  services: state.services.services,
});

const mapDispatchToProps = (dispatch) => ({
  fetchAll: () => dispatch(fetchAllServiceAsync()),
});

const connector = connect(mapStateToProps, mapDispatchToProps);

export class ServiceTable extends React.Component {
  columns = [
    {
      title: "#",
      dataIndex: "operation",
      key: "operation",
      render: (text, record) => (
        <TableOperator
          hideDelete={this.props.onlySelect}
          value={record}
          isEdited={record.isEdited || record.isNew}
          onSave={this.handleSet}
          onDelete={this.handleDelete}
          onOpen={this.handleOpen}
          onCancel={this.handleCancel}
        />
      ),
      width: 100,
    },
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
    },
    {
      title: "Version",
      dataIndex: "version",
      key: "version",
      width: 100,
    },
  ];
  render() {
    return (
      <div>
        {this.props.onlySelect ? null : (
          <Button style={{ marginBottom: 16 }} onClick={this.handleAdd}>
            {this.props.kdnr ? null : "Neuen"} Dienst hinzufügen
          </Button>
        )}
        <Table
          bordered
          onRow={this.rowEvent}
          rowClassName={(record, index) => ["row"]}
          emptyText="Keine Daten vorhanden"
          rowKey="module_ID"
          size="small"
          columns={this.columns}
          dataSource={this.props.services}
        />
      </div>
    );
  }
}

export default connector(ServiceTable);
