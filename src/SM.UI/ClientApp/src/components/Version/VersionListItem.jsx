import React from 'react';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit } from '@fortawesome/free-solid-svg-icons'

import { Button } from 'reactstrap';

class VersionListItem extends React.Component {
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
            <td>{model.version}</td>
            <td><small>({model.releaseDate})</small></td>
            <td><Button size="sm" color="success" onClick={this.openEdit}><FontAwesomeIcon icon={faEdit} /></Button></td>
        </tr>;
    }
}

export default VersionListItem;