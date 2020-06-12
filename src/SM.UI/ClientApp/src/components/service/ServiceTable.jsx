import React from "react";
import { Table, Input, Button, Tag } from "antd";
import __api_helper from "../../helper/__api_helper";
import TableOperator from "../common/TableOperator";

class ServiceNameRow extends React.Component {
  state = {
    text: "",
  };
  constructor(props) {
    super(props);

    this.state.text = props.value;

    this.handleChange = this.handleChange.bind(this);
  }
  handleChange({ target: { value } }) {
    this.setState({ text: value });
    if (this.props.onChange) {
      this.props.onChange(value);
    }
  }
  render() {
    return <Input value={this.state.text} onChange={this.handleChange} />;
  }
}

class ServiceTable extends React.Component {
  columns = [
    {
      title: "#",
      dataIndex: "operation",
      key: "operation",
      render: (text, record) => (
        <TableOperator
          hideDelete={this.props.onlySelect}
          value={record}
          isNew={record.isNew}
          onSave={this.handleSet}
          onDelete={this.handleDelete}
          onOpen={this.handleOpen}
          onCancel={this.handleCancel}
        />
      ),
      hidden: this.props.kdnr != null,
      width: 50,
    },
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      render: (text, record) =>
        this.props.kdnr || this.props.onlySelect ? (
          <span>{text}</span>
        ) : (
          <ServiceNameRow value={text} onChange={(e) => (record.name = e)} />
        ),
    },
    {
      title: "Version",
      dataIndex: "version",
      key: "version",
      render: (text, record) => (
        <Tag color={text ? "green" : "orange"}>{text ? text : <i>Leer</i>}</Tag>
      ),
    },
  ];

  helper = new __api_helper.API_Services();

  state = {
    serviceList: [],
  };

  constructor(props) {
    super(props);

    if (props.kdnr) {
      let col = this.columns.filter((x) => x.key !== "operation");
      col.push({
        title: "Status",
        dataIndex: "status",
        key: "status",
      });
      this.columns = [...col];
    }

    this.rowEvent = this.rowEvent.bind(this);
    this.handleAdd = this.handleAdd.bind(this);
    this.handleOpen = this.handleOpen.bind(this);
    this.handleDelete = this.handleDelete.bind(this);
    this.handleSet = this.handleSet.bind(this);
    this.handleCancel = this.handleCancel.bind(this);
  }

  handleOpen(service) {
    if (this.props.onOpenService) {
      this.props.onOpenService({ ...service });
    }
  }
  handleDelete(service) {
    this.helper.deleteService(service);
    let st = this.state.serviceList.filter(
      (x) => x.module_ID !== service.module_ID
    );
    this.setState({ serviceList: [...st] });
  }
  handleAdd(e) {
    if (this.props.onAddService) {
      this.props.onAddService(e);
    } else {
      this.setState({
        serviceList: [
          ...this.state.serviceList,
          {
            name: "",
            version: "[NEU]",
            module_ID: this.state.serviceList.length,
            isNew: true,
          },
        ],
      });
    }
  }
  handleSet(service) {
    if (service.isNew === true) {
      this.helper.createService({ ...service }).then((res) => {
        let ser = this.state.serviceList;
        ser = ser.filter((val, key) => val.module_ID !== service.module_ID);
        this.setState({ serviceList: [...ser, res] });
      });
    } else {
    }
  }
  handleCancel(service) {
    let ser = this.state.serviceList;
    ser = ser.filter((val, key) => val.module_ID !== service.module_ID);
    this.setState({ serviceList: [...ser] });
  }

  async loadServiceData() {
    let serviceList;
    if (this.props.kdnr) {
      serviceList = await this.helper.getServicesFromCustomer(this.props.kdnr);
    } else {
      serviceList = await this.helper.getServices();
    }
    if (this.props.getFirstRow && serviceList != null) {
      this.props.getFirstRow(serviceList[0]);
    }
    this.setState({ serviceList });
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if (prevState.kdnr === nextProps.kdnr) return prevState;
    return {
      kdnr: nextProps.kdnr,
    };
  }

  componentDidMount() {
    this.loadServiceData();
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.kdnr !== this.state.kdnr) {
      this.loadServiceData();
    }
  }

  rowEvent(record, rowIndex) {
    return {
      onClick: (eve) => {
        if (eve.target.nodeName === "INPUT") {
          return;
        }
      },
    };
  }

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
          dataSource={this.state.serviceList}
        />
      </div>
    );
  }
}

export default ServiceTable;
