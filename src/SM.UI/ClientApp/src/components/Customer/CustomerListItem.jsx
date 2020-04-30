import React from 'react';
import { Button } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit } from '@fortawesome/free-solid-svg-icons';


class CustomerListItem extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            item: props.model
        };
    }

    render() {
        return <tr>
            <td><Button size="sm" outline color="primary"><FontAwesomeIcon icon={faEdit} /></Button></td>
            <td>{this.state.item.kdnr}</td>
            <td>{this.state.item.name}</td>
        </tr>;
    }
}

export default CustomerListItem;