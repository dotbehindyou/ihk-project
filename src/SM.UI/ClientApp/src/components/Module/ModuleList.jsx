import React from 'react';

import ModuleListItem from './ModuleListItem';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { Button } from 'reactstrap';

class ModuleList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            onEdit: props.onEdit,
            items: []
        }
    }

    componentDidMount() {
        fetch('https://localhost:44376/api/v1/Modules') // TODO Addresse über Config auslesen lassen
            .then(res => res.json())
            .then((result) => this.setState({
                isLoaded: true,
                items: result
            }));
    }

    handleDelete(item) {
        console.log(item);
    }

    render() {
        return <table>
            <tbody>
                {this.state.items.map((x) => <ModuleListItem key={x.module_ID} model={x} onEdit={this.props.onEdit} onDelete={this.handleDelete} />)}
            </tbody>
            <tfoot>
                <tr>
                    <td><Button size="sm" outline onClick={() => this.props.onEdit({})}>Modul hinzufügen <FontAwesomeIcon icon={faPlus} /></Button></td>
                </tr>
            </tfoot>
        </table>;
    }
}

export default ModuleList;