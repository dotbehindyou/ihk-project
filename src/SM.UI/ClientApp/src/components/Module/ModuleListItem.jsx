import React from 'react';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit, faTrash, faWrench } from '@fortawesome/free-solid-svg-icons'

import { Button, ButtonGroup } from 'reactstrap';

class ModuleListItem extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            model: props.model
        };

        this.openEdit = this.openEdit.bind(this);
        this.handleChangeVersion = this.handleChangeVersion.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
    }

    // componentDidMount() {     }

    openEdit(e) {
        this.props.onEdit(this.props.model);
    }

    handleChangeVersion(e) {
        this.props.onChangeVersion(this.props.model);
    }

    handleDelete(e) {
        if (this.props.onDelete !== undefined)
            this.props.onDelete(this.state.model);
    }

    render() {
        var model = this.state.model;
        var btn;
        if (this.props.onEdit !== undefined) {
            btn = <ButtonGroup size="sm">
                { /*Modifie Button*/this.props.onEdit ? <Button outline color="success" onClick={this.openEdit}><FontAwesomeIcon icon={this.props.editIcon || faEdit} /></Button> : null }
                { /*Change Version Button*/this.props.onChangeVersion ? <Button outline color="primary" onClick={this.handleChangeVersion}><FontAwesomeIcon icon={faWrench} /></Button> : null }
                { /*Delete Button*/this.props.isSelect ? null : <Button size="sm" color="danger" outline onClick={this.handleDelete}><FontAwesomeIcon icon={faTrash} /></Button> }
            </ButtonGroup>
        }
        return <tr>
            <td>{btn}</td>
            <td>{model.name}</td>
            <td><small>({model.version})</small></td>
            {this.props.showStatus ? <td>{model.status}</td> : null}
        </tr>;
    }
}

export default ModuleListItem;