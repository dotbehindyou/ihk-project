﻿import React from 'react';
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
    }

    render() {
        var renderdItem = null;
        if (this.state.select == null)
            renderdItem = <ModuleList url="https://localhost:44376/api/v1/Modules" onEdit={this.openEditor} />;
        else
            renderdItem = <Module onClose={this.closeEditor} model={this.state.select} />;

        return <div>
            <h4>Modulenverwaltung</h4>
            <hr />
            {renderdItem}
        </div>;
    }
}

export default ModulePage;