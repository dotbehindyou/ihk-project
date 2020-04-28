import React from 'react';

import ModuleListItem from './ModuleListItem';

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

    render() {
        return <table>
            <tbody>
                {this.state.items.map((x) => <ModuleListItem key={x.module_ID} model={x} onEdit={this.props.onEdit}/>)}
            </tbody>
        </table>;
    }
}

export default ModuleList;