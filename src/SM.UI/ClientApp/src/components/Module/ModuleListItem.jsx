import React from 'react';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons'

import { Button, ButtonGroup } from 'reactstrap';

class ModuleListItem extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            model: props.model
        };

        this.openEdit = this.openEdit.bind(this);
    }

    // componentDidMount() {     }

    openEdit() {
        if (this.props.onEdit !== undefined) {
            this.props.onEdit(this.props.model);
        }
    }

    render() {
        var model = this.state.model;
        var btn;
        if (this.props.onEdit !== undefined) {
            btn = <ButtonGroup>
                <Button outline size="sm" color="success" onClick={this.openEdit}><FontAwesomeIcon icon={this.props.editIcon || faEdit} /></Button>
                <Button size="sm" color="danger" outline onClick={() => this.props.onDelete(this.state.model)}><FontAwesomeIcon icon={faTrash} /></Button>
            </ButtonGroup>
        }
        return <tr>
            <td>{btn}</td>
            <td>{model.name}</td>
            <td><small>({model.version})</small></td>
        </tr>;
    }
}

export default ModuleListItem;