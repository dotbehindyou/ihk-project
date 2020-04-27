import React from 'react';

import VersionListItem from './VersionListItem';
import { Button } from 'reactstrap';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

class VersionList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            onEdit: props.onEdit,
            module: props.module,
            items: []
        }
    }

    componentDidMount() {
        fetch('https://localhost:44376/api/v1/' + this.state.module + '/version') // TODO Addresse über Config auslesen lassen
            .then(res => res.json())
            .then((result) => this.setState({
                isLoaded: true,
                items: result
            }))
            .catch((ex) => this.setState({ error: ex }));
    }

    render() {
        var comp;
        if (this.state.isLoaded) {
            comp = this.state.items.map((x) => <VersionListItem key={x.version} model={x} onEdit={this.props.onEdit} />);
        } else {
            comp = <tr><td>{this.state.error}</td></tr>;
        }
        return <table>
            <tbody>
                { comp }
            </tbody>
            <tfoot>
                <tr>
                    <td><Button><FontAwesomeIcon icon={faPlus} /></Button></td>
                </tr>
            </tfoot>
        </table>;
    }
}

export default VersionList;