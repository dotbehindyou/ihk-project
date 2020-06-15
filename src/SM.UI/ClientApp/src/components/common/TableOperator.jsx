import React from "react";
import { Button, Popconfirm } from "antd";
import { DeleteOutlined, SelectOutlined, SaveOutlined, CloseCircleOutlined } from '@ant-design/icons';


class TableOperator extends React.Component {
  state = {
    ...this.props.value,
  };

  static getDerivedStateFromProps(props, state) {
    if(props.isEdited !== state.isEdited) return {isEdited: props.isEdited};
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
        {this.state.isEdited === true ? (
          <Button.Group size="small">
            <Button type="primary" onClick={(e) => this.onSet()}>
              <SaveOutlined />
            </Button>
            <Button onClick={(e) => this.onCancel()}>
              <CloseCircleOutlined />
            </Button>
          </Button.Group>
        ) : (
          <Button.Group size="small">
            <Button onClick={(e) => this.onOpen()}>
              <SelectOutlined />
            </Button>
            <Popconfirm
              title="Löschen?"
              cancelText="Nein"
              onConfirm={() => this.onDelete()}
            >
              <Button hidden={this.props.hideDelete} type="danger">
               <DeleteOutlined />
              </Button>
            </Popconfirm>
          </Button.Group>
        )}
      </div>
    );
  }
}

export default TableOperator;
