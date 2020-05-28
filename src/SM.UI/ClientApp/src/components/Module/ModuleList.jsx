import React from 'react';

import ModuleListItem from './ModuleListItem';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { Button, Table } from 'reactstrap';

class ModuleList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            onEdit: props.onEdit,
            items: []
        };

        this.handleDelete = this.handleDelete.bind(this);
    }

    componentDidMount() {
        fetch(this.props.url) // TODO Addresse über Config auslesen lassen
            .then(res => res.json())
            .then((result) => {
                this.setState({
                    isLoaded: true,
                    items: result
                });
            })
            .catch((ex) => console.log(ex));
    }

//    componentWillUnmount() {
//    }

    handleDelete(item) {
        if (window.confirm("Wollen Sie das Modul " + item.name + " wirklich löschen?")) {
            if (this.props.onDelete !== undefined)
                this.props.onDelete(item);
            this.setState((prevState) => {
                return {
                    items: prevState.items.filter(x => x.module_ID !== item.module_ID)
                };
            });
        }
    }

    render() {
        var rows = this.state.items.map((x) => <ModuleListItem key={x.module_ID}
            showStatus={true}
            onChangeVersion={this.props.onChangeVersion}
            onEdit={this.props.onEdit}
            onDelete={this.handleDelete}
            model={x}
            isSelect={this.props.isSelect}
            editIcon={this.props.editIcon} />);

        var edit;
        if (this.props.onEdit != null && this.props.isSelect !== true) {
            edit = <td colSpan={this.props.showStatus ? 4 : 3}><Button size="sm" outline onClick={() => this.props.onEdit({})}>Dienst hinzufügen <FontAwesomeIcon icon={faPlus} /></Button></td>;
        }

        return <>
            <Table size="sm">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Dienst</th>
                        <th>Version</th>
                        {this.props.showStatus ? <th>Status</th> : null}
                    </tr>
                </thead>
                <tbody>
                    {rows}
                </tbody>
                <tfoot>
                    <tr>
                        {edit}
                    </tr>
                </tfoot>
            </Table>
        </>;
    }
}

export default ModuleList;