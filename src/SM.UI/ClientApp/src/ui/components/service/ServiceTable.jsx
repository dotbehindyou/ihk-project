import React from "react";
import { Table, Button, Input, Tag } from "antd";

import { connect } from "react-redux";
import { selectService, addService, cancelEditService } from "../../../store/services/services.actions";
import TableOperator from "../common/TableOperator";

const mapStateToProps = (state) => ({
  serviceStore: state.services,
});

const mapDispatchToProps = (dispatch) => ({
  select: (service) => dispatch(selectService(service)),
  add: () => dispatch(addService()),
  cancel: (service) => dispatch(cancelEditService(service))
});

const connector = connect(mapStateToProps, mapDispatchToProps);

export class ServiceTable extends React.Component {
  columns = [ {
    title: "#",
    dataIndex: "operation",
    key: "operation",
    render: (text, record) => (
      <TableOperator
        hideDelete={this.props.onlySelect}
        value={record}
        isEdited={record.isEdited || record.isNew}
        onSave={this.handleSave}
        onDelete={this.handleDelete}
        onOpen={this.handleOpen}
        onCancel={this.handleCancel}
      />
    ),
    width: 100,
  },{
    title: "Name",
    dataIndex: "name",
    key: "name",
  render: (text, record) => (record.isNew || record.isEditing ? 
    <Input defaultValue={record.name} onChange={({target: { value }}) => record.name = value}/> 
    : <span>{text}</span>)
  },{
    title: "Version",
    dataIndex: "version",
    key: "version",
    width: 100,
    render: (text, record) => <Tag color={text === '' || text == null ? "warning" : "green"}>{text || <i>Leer</i>}</Tag>
  },];

  constructor(props){
    super(props);

    this.handleSave = this.handleSave.bind(this);
    this.handleDelete = this.handleDelete.bind(this);
    this.handleOpen = this.handleOpen.bind(this);
    this.handleCancel = this.handleCancel.bind(this);
    this.handleAdd = this.handleAdd.bind(this);
  }
 // TODO Speicher Async
  handleSave(service){
    console.log(service);
  }

// TODO Delete Async
  handleDelete(service){
    console.log(service);

  }

  handleOpen(service){
    this.props.select(service);
  }

  handleCancel(service){
    this.props.cancel(service);

  }

  handleAdd(e){
    this.props.add();
  }

  render() {
    let store = this.props.serviceStore;
    let serviceList = this.props.serviceStore.services;
    let isEditing = this.props.serviceStore.isEditing;
    return (
      <div>
        {this.props.onlySelect ? null : (
          <Button disabled={isEditing} style={{ marginBottom: 16 }} onClick={this.handleAdd}>
            {this.props.kdnr ? null : "Neuen"} Dienst hinzufügen
          </Button>
        )}
        <Table
          loading={store.isFetching}
          bordered
          rowKey="module_ID"
          size="small"
          columns={this.columns}
          dataSource={serviceList}
        />
      </div>
    );
  }
}

export default connector(ServiceTable);
