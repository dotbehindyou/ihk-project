import React from 'react';

import VersionListItem from './VersionListItem';
import { Button, Table } from 'reactstrap';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import VersionEditorModal from './VersionEditorModal';

class VersionList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            module: props.module,
            items: [],
            editor: {
                isOpen: false,
                item: {}
            }
        }

        this.toggleEditor = this.toggleEditor.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
    }

    load() {
        fetch('/api/' + this.state.module.module_ID + '/versions') // TODO Addresse über Config auslesen lassen
            .then(res => res.json())
            .then((result) => this.setState({
                isLoaded: true,
                items: result
            }))
            .catch((ex) => this.setState({ error: ex, items: [] }));
    }

    componentDidMount() {
        if (!this.state.isLoaded) {
            this.load();
        }
    }

    toggleEditor(editItem, close) {
        if (close !== true) {
            close = this.state.editor.isOpen;
        }

        this.setState({
            editor: {
                isOpen: !close,
                item: editItem || { moduleId: this.state.module.module_ID, moduleName: this.state.module.name }
            }
        })
    }

    handleDelete(item) {
        if (window.confirm("Wollen Sie die Version " + item.version + " wirklich löschen?")) {
            if (this.props.onDelete !== undefined)
                this.props.onDelete(item);
            this.setState((prevState) => {
                return {
                    items: prevState.items.filter(x => x.version !== item.version)
                };
            });
        }
    }

    render() {
        var comp;
        if (this.state.isLoaded) {
            comp = this.state.items.map((x) => <VersionListItem key={x.version}
                isSelect={this.props.kdnr !== undefined}
                editIcon={this.props.editIcon}
                model={x}
                onEdit={this.toggleEditor}
                onDelete={this.handleDelete} />);
        } else {
            comp = <tr><td>{this.state.error}</td></tr>;
        }
        var edit;
        if (this.state.editor.isOpen) {
            edit = <VersionEditorModal
                changeVersion={this.props.update === true}
                kdnr={this.props.kdnr}
                moduleId={this.state.module.module_ID}
                moduleName={this.state.module.name}
                isOpen={this.state.editor.isOpen}
                onClose={() => { this.toggleEditor(null, true); }}
                version={this.state.editor.item.version} />

        }
        return (
            <div>
                <Table size="sm">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Versions Nr.</th>
                            <th>Release Datum</th>
                        </tr>
                    </thead>
                    <tbody>
                        {comp}
                    </tbody>
                    <tfoot>
                        {
                            this.props.kdnr === undefined ?
                                <tr>
                                    <td colSpan={3}><Button size="sm" outline onClick={() => this.toggleEditor(null) }>Version hinzufügen <FontAwesomeIcon icon={faPlus} /></Button></td>
                                </tr>
                                : null
                        }
                    </tfoot>
                </Table>
                {edit}
            </div>);
    }
}

export default VersionList;