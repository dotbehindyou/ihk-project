import React from "react";
import { Table, Button, Input, DatePicker } from "antd";
import __api_helper from "../../../helper/__api_helper";
import TableOperator from "../common/TableOperator";
import moment from "moment";

class VersionRow extends React.Component {
  state = {
    text: "",
  };
  constructor(props) {
    super(props);

    this.state.text = props.value;

    this.handleChange = this.handleChange.bind(this);
  }
  handleChange(eve) {
    let value = eve ? (eve.target ? eve.target.value : eve) : null;
    this.setState({ text: value });
    if (this.props.onChange) {
      this.props.onChange(value);
    }
  }
  render() {
    if (this.props.datePicker)
      return (
        <DatePicker
          locale="de"
          format="DD.MM.YYYY"
          value={this.state.text}
          onChange={this.handleChange}
        ></DatePicker>
      );
    return <Input value={this.state.text} onChange={this.handleChange} />;
  }
}

class VersionTable extends React.Component {
  columns = [
    {
      title: "#",
      dataIndex: "operation",
      key: "operation",
      render: (text, record) => (
        <TableOperator
          hideDelete={this.props.onlySelect}
          isEdited={record.isNew}
          value={record}
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
      title: "Version",
      dataIndex: "version",
      key: "version",
      render: (text, record) =>
        record.isNew && !this.props.onlySelect ? (
          <VersionRow
            value={text}
            onChange={(e) => {
              record.version = e;
            }}
          />
        ) : (
          <span>{text}</span>
        ),
    },
    {
      width: 130,
      title: "Release Datum",
      dataIndex: "releaseDate",
      key: "releaseDate",
      render: (text, record) =>
        record.isNew && !this.props.onlySelect ? (
          <VersionRow
            datePicker
            value={text}
            onChange={(e) => {
              record.releaseDate = e;
            }}
          />
        ) : (
          <span>{text ? text.format("DD.MM.YYYY") : null}</span>
        ),
      //render: (ir) => <span>{ir ? ir.format("DD.MM.YYYY") : null}</span>,
    },
  ];

  helper = new __api_helper.API_Version();

  state = {
    serviceId: null,
    versionList: [],
  };

  constructor(props) {
    super(props);

    this.rowEvent = this.rowEvent.bind(this);
    this.handleAdd = this.handleAdd.bind(this);
    this.handleSet = this.handleSet.bind(this);
    this.handleCancel = this.handleCancel.bind(this);
    this.handleDelete = this.handleDelete.bind(this);
    this.handleOpen = this.handleOpen.bind(this);
  }

  handleAdd(e) {
    let verList = this.state.versionList;
    verList.push({
      module_ID: this.props.serviceId,
      version: "new_" + verList.length,
      isNew: true,
      releaseDate: moment(),
    });
    this.setState({ versionList: [...verList] });
  }

  handleCancel(e) {
    let verList = this.state.versionList.filter((x) => x.version !== e.version);
    this.setState({ versionList: [...verList] });
  }

  handleDelete(version) {
    this.helper.deleteVersion({ ...version });
    let ver = this.state.versionList.filter(
      (x) => x.version !== version.version
    );
    this.setState({ versionList: [...ver] });
  }

  handleOpen(e) {
    if (this.props.onOpenVersion) this.props.onOpenVersion({ ...e });
  }

  handleSet(ver) {
    if (ver.isNew) {
      this.helper.create(this.props.serviceId, { ...ver });
      let verlist = this.state.versionList.filter(
        (x) => x.version !== ver.version
      );
      console.log(verlist);
      ver.isNew = false;
      this.setState({ versionList: [...verlist, ver] });
    }
  }

  async loadVersionData() {
    let versionList = await this.helper.getVersionsFromService(
      this.props.serviceId
    );
    this.setState({ versionList });
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    if (prevState.serviceId === nextProps.serviceId) return null;
    return {
      serviceId: nextProps.serviceId,
    };
  }

  componentDidMount() {
    this.loadVersionData();
  }

  componentDidUpdate(prevProps, prevState) {
    if (prevState.serviceId !== this.state.serviceId) {
      console.log("test");
      this.loadVersionData();
    }
  }

  rowEvent(record, rowIndex) {
    return {
      onClick: (eve) => {
        //if (this.props.onOpenVersion) this.props.onOpenVersion({ ...record });
      },
    };
  }

  render() {
    return (
      <div>
        <Button
          hidden={this.props.onlySelect}
          style={{ marginBottom: 16 }}
          onClick={this.handleAdd}
        >
          Version hinzufügen
        </Button>
        <Table
          bordered
          onRow={this.rowEvent}
          rowClassName={(record, index) => ["row"]}
          emptyText="Keine Daten vorhanden"
          rowKey="version"
          size="small"
          columns={this.columns}
          dataSource={this.state.versionList}
        />
      </div>
    );
  }
}

export default VersionTable;
