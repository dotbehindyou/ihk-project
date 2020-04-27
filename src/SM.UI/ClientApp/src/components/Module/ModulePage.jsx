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
        this.closeEditor = this.closeEditor.bind(this);
    }

    openEditor(model) {
        this.setState({
            select: model
        });
    }

    closeEditor() {
        this.setState({
            select: null
        });
        console.log("lo");
    }

    render() {
        var renderdItem = null;
        if (this.state.select == null)
            renderdItem = <ModuleList onEdit={this.openEditor} />;
        else
            renderdItem = <Module onClose={this.closeEditor} model={this.state.select} />;

        return <div> {renderdItem} </div>;
    }
}

export default ModulePage;