import React from 'react';
import { Button } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit, faPlus } from '@fortawesome/free-solid-svg-icons';


class CustomerListItem extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            kdnr: null,
            name: null,
            isRegisterd: false,
            ...props.model
        };
    }

    render() {
        return <tr>
            <td><Button onClick={() => { this.props.onEdit(this.state) }} size="sm" outline color={this.state.isRegisterd ? "primary" : "success"}><FontAwesomeIcon icon={(this.state.isRegisterd ? faEdit : faPlus)} /></Button></td>
            <td>{this.state.kdnr}</td>
            <td>{this.state.name}</td>
            <td>{(this.state.isRegisterd ? "Ja" : "Nein")}</td>
        </tr>;
    }
}

export default CustomerListItem;