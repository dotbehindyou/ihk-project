import React from "react";
import { Button, Popconfirm } from "antd";

class TableOperator extends React.Component {
  state = {
    ...this.props.value,
  };

  static getDerivedStateFromProps(props, state) {
    if (props.value.version !== state.version) return { ...props.value };
    return null;
  }

  componentDidMount() {}

  componentDidUpdate(prevProps, prevState, snapshot) {}

  onSet() {
    if (this.props.onSave) this.props.onSave({ ...this.props.value });
  }
  onDelete() {
    if (this.props.onDelete) this.props.onDelete({ ...this.props.value });
  }
  onOpen() {
    if (this.props.onOpen) this.props.onOpen({ ...this.props.value });
  }
  onCancel() {
    if (this.props.onCancel) this.props.onCancel({ ...this.props.value });
  }

  render() {
    return (
      <div>
        {this.props.isNew === true ? (
          <Button.Group>
            <Button type="primary" onClick={(e) => this.onSet()}>
              Speichern
            </Button>
            <Button onClick={(e) => this.onCancel()}>Abbrechen</Button>
          </Button.Group>
        ) : (
          <Button.Group>
            <Button onClick={(e) => this.onOpen()}>Öffnen</Button>
            <Popconfirm
              title="Löschen?"
              cancelText="Nein"
              onConfirm={() => this.onDelete()}
            >
              <Button hidden={this.props.hideDelete} type="danger">
                Löschen
              </Button>
            </Popconfirm>
          </Button.Group>
        )}
      </div>
    );
  }
}

export default TableOperator;
