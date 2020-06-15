import React from "react";
import { Table } from "antd";
import __api_helper from "../../../helper/__api_helper";

class CustomerTable extends React.Component {
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

  helper = new __api_helper.API_Customer();

  state = {
    customerList: [],
  };

  constructor(props) {
    super(props);

    this.rowEvent = this.rowEvent.bind(this);
  }

  async componentDidMount() {
    var customerList = await this.helper.getCustomers();
    this.setState({ customerList });
  }

  rowEvent(record, rowIndex) {
    return {
      onClick: (eve) => {
        this.props.onOpenCustomer({ ...record });
        record.isRegisterd = true;
      },
    };
  }

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

export default CustomerTable;
