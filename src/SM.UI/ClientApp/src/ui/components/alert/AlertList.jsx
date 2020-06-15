import React from "react";
import { connect } from "react-redux";
import { AddAlert, RemoveAlert } from "../../../store/alerts/alerts.actions";
import { Alert } from "antd";

const mapStateToProps = (state) => ({
  alerts: state.alerts,
});

const mapDispatchToProps = (dispatch) => ({
  removeAlert: (id) => dispatch(RemoveAlert(id)),
  addAlert: (text, type, icon) => dispatch(AddAlert(text, icon, type)),
});

const connector = connect(mapStateToProps, mapDispatchToProps);

class AlertList extends React.Component {
  state = {
    deleterId: 0,
    toDelete: [],
  };

  constructor(props) {
    super(props);

    this.removeAlert = this.removeAlert.bind(this);
  }

  componentDidMount() {}

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.alerts !== this.props.alerts) {
      this.resetAlert(this.props.alerts);
    }
  }

  resetAlert(alerts) {
    let self = this;
    alerts.forEach((alert) => {
      if (self.state.toDelete.filter((x) => x.id === alert.id).length < 1) {
        self.setState({ toDelete: [...self.state.toDelete, alert] });
        setTimeout((e) => {
          self.props.removeAlert(alert.id);
        }, 5000);
      }
    });
  }

  removeAlert(id) {
    console.log(id);
    this.props.removeAlert(id);
  }

  render() {
    return (
      <div
        style={{
          position: "fixed",
          top: 5,
          right: 0,
          width: 400,
          zIndex: 1000,
        }}
      >
        {this.props.alerts.map((a) => (
          <Alert
            key={a.id}
            style={{ marginRight: 5, marginBottom: 5 }}
            message={a.text}
            description={a.description}
            type={a.typ}
            showIcon={a.icon}
            closable
            onClose={(e) => {
              this.removeAlert(a.id);
            }}
          />
        ))}
      </div>
    );
  }
}

export default connector(AlertList);
