import React from 'react';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit } from '@fortawesome/free-solid-svg-icons'

import { Button } from 'reactstrap';

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
        return <tr>
            <td>{model.name}</td>
            <td><small>({model.version})</small></td>
            <td><Button outline size="sm" color="success" onClick={this.openEdit}><FontAwesomeIcon icon={faEdit} /></Button></td>
        </tr>;
    }
}

export default ModuleListItem;