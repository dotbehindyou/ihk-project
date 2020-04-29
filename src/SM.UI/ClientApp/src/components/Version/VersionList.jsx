import React from 'react';

import VersionListItem from './VersionListItem';
import { Button } from 'reactstrap';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import VersionEditorModal from './VersionEditorModal';

class VersionList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            onEdit: props.onEdit,
            module: props.module,
            items: [],
            editor: {
                isOpen: false,
                item: {}
            }
        }

        this.toggleEditor = this.toggleEditor.bind(this);
    }

    load() {
        fetch('https://localhost:44376/api/v1/' + this.state.module.module_ID + '/versions') // TODO Addresse über Config auslesen lassen
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

    render() {
        var comp;
        if (this.state.isLoaded) {
            comp = this.state.items.map((x) => <VersionListItem key={x.version} model={x} onEdit={this.toggleEditor} />);
        } else {
            comp = <tr><td>{this.state.error}</td></tr>;
        }
        var edit;
        if (this.state.editor.isOpen) {
            edit = <VersionEditorModal moduleId={this.state.module.module_ID} moduleName={this.state.module.name} isOpen={this.state.editor.isOpen} onClose={() => { this.toggleEditor(null, true); }} version={this.state.editor.item.version} />
        }
        return (
            <div>
                <table>
                    <tbody>
                        {comp}
                    </tbody>
                    <tfoot>
                        <tr>
                            <td><Button size="sm" outline onClick={() => this.toggleEditor(null) }>Version hinzufügen <FontAwesomeIcon icon={faPlus} /></Button></td>
                        </tr>
                    </tfoot>
                </table>
                { edit }
            </div>);
    }
}

export default VersionList;