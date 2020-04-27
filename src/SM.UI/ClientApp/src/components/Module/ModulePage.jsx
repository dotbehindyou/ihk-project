import React from 'react';
import ModuleList from './ModuleList';
import Module from './Module';

class ModulePage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            error: null,
            isLoaded: false,
            items: [],
            select: null
        }

        this.openEditor = this.openEditor.bind(this);
    }

    openEditor(model) {
        this.setState({
            select: model
        });
    }

    render() {
        var renderdItem = null;
        if (this.state.select == null)
            renderdItem = <ModuleList onEdit={this.openEditor} />;
        else
            renderdItem = <Module model={this.state.select} />;

        return <div> {renderdItem} </div>;
    }
}

export default ModulePage;