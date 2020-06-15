import React from "react";
import CustomerTable from "../customer/CustomerTable";
import CustomerView from "../customer/CustomerView";
import Content from "../../layout/Content";

class CustomerPage extends React.Component {
  state = {
    selected: null,
  };

  constructor(props) {
    super(props);

    this.openCustomer = this.openCustomer.bind(this);
  }

  openCustomer(customer) {
    this.setState({ selected: customer });
  }

  render() {
    var selected = this.state.selected || {};
    var isSelected = this.state.selected == null;
    return (
      <div>
        <Content hide={isSelected}>
          <CustomerView kdnr={selected.kdnr} isNew={!selected.isRegisterd} />
        </Content>

        <Content>
          <h2>Kunden</h2>
          <CustomerTable onOpenCustomer={this.openCustomer} />
        </Content>
      </div>
    );
  }
}

export default CustomerPage;
