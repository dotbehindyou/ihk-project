import React from 'react';
import moment from 'moment';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit, faTrash } from '@fortawesome/free-solid-svg-icons'

import { Button, ButtonGroup } from 'reactstrap';

class VersionListItem extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            model: props.model
        };

        this.openEdit = this.openEdit.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
    }

    // componentDidMount() {     }

    openEdit() {
        if (this.props.onEdit !== undefined) {
            this.props.onEdit(this.state.model);
        }
    }

    handleDelete(e) {
        if (this.props.onDelete !== undefined)
            this.props.onDelete(this.state.model);
    }

    render() {
        var model = this.state.model;
        return <tr>
            <td>
                <ButtonGroup>
                    <Button outline size="sm" color="success" onClick={this.openEdit}><FontAwesomeIcon icon={this.props.editIcon || faEdit} /></Button>
                    {this.props.isSelect ? null : <Button size="sm" color="danger" outline onClick={this.handleDelete}><FontAwesomeIcon icon={faTrash} /></Button>}
                </ButtonGroup>
            </td>
            <td>{model.version}</td>
            <td><small>({moment(model.releaseDate).format('DD.MM.YYYY')})</small></td>
        </tr>;
    }
}

export default VersionListItem;