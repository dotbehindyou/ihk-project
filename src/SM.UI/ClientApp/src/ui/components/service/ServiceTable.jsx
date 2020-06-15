import React from "react";
import { Table, Input, Button, Tag } from "antd";
import __api_helper from "../../../helper/__api_helper";
import TableOperator from "../common/TableOperator";

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
          isEdited={record.isEdited || record.isNew}
          onSave={this.handleSet}
          onDelete={this.handleDelete}
          onOpen={this.handleOpen}
          onCancel={this.handleCancel}
        />
      ),
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
          <Input
            value={text}
            onChange={({ target: { value } }) => {
              let st = this.state.serviceList.map((x) => {
                if (x.module_ID === record.module_ID) {
                  if (x.isEdited !== true) {
                    x.isEdited = true;
                    x.redoName = x.name;
                  }
                  x.name = value;
                  return { ...x };
                }
                return x;
              });
              this.setState({ serviceList: st });
            }}
          />
        ),
    },
    {
      title: "Version",
      dataIndex: "version",
      key: "version",
      render: (text, record) => (
        <Tag color={text ? "green" : "orange"}>{text ? text : <i>Leer</i>}</Tag>
      ),
      width: 50,
    },
  ];

  helper = new __api_helper.API_Services();

  state = {
    serviceList: [],
  };

  constructor(props) {
    super(props);

    if (props.kdnr) {
      let col = this.columns; //.filter((x) => x.key !== "operation");
      col.push({
        title: "Status",
        dataIndex: "status",
        key: "status",
        width: 50,
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
    if (this.props.onDelete) {
      this.props.onDelete(service);
    } else {
      this.helper.deleteService(service);
    }
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
    } else if (service.isEdited === true) {
      this.helper.setService(service.module_ID, { ...service }).then((res) => {
        let ser = this.state.serviceList.map((x) => {
          if (x.module_ID === service.module_ID) {
            x.isEdited = false;
            x.redoName = null;
            return { ...x };
          }
          return x;
        });
        this.setState({ serviceList: ser });
      });
    }
  }
  handleCancel(service) {
    let ser = this.state.serviceList;
    if (service.isEdited) {
      ser = ser.map((x) => {
        if (x.module_ID === service.module_ID) {
          x.isEdited = false;
          x.name = x.redoName;
          console.log(x);
        }
        return x;
      });
    } else {
      ser = ser.filter((val, key) => val.module_ID !== service.module_ID);
    }
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
    if (prevState.kdnr === nextProps.kdnr) return null;
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
