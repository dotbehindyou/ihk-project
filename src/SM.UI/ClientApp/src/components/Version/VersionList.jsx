import React from 'react';

import VersionListItem from './VersionListItem';
import { Button } from 'reactstrap';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import VersionEditorModal from './VersionEditorModal';

var debug_item = { "module_ID": "1ec96940-0eb2-4a42-9be1-5268df8afd80", "moduleName": null, "version": "3.0", "config": null, "releaseDate": "2020-04-19T00:00:00", "validationToken": "", "file": null };

class VersionList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            onEdit: props.onEdit,
            moduleId: props.moduleId,
            items: [],
            editor: { // TODO DEBUG!!! Beim Releas ändern!!
                isOpen: true,
                item: debug_item
            }
        }

        this.toggleEditor = this.toggleEditor.bind(this);
    }

    componentDidMount() {
        if (!this.state.isLoaded) {
            fetch('https://localhost:44376/api/v1/' + this.state.moduleId + '/versions') // TODO Addresse über Config auslesen lassen
                .then(res => res.json())
                .then((result) => this.setState({
                    isLoaded: true,
                    items: result
                }))
                .catch((ex) => this.setState({ error: ex, items: [] }));
        }
    }

    toggleEditor(editItem) {
        this.setState({
            editor: {
                isOpen: !this.state.editor.isOpen,
                item: editItem || { }
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
                <VersionEditorModal isOpen={this.state.editor.isOpen} onToggle={this.toggleEditor} model={this.state.editor.item} />
            </div>);
    }
}

export default VersionList;