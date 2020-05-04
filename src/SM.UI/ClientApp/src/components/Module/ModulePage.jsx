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
        this.handleDelete = this.handleDelete.bind(this);
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

    handleDelete(item) {
        fetch('/api/Modules/' + item.module_ID,
            {
                method: 'DELETE'
            })
            .then((res) => res.json())
            .then(res => {

            })
            .catch((ex) => console.log(ex));
    }

    render() {
        var renderdItem = null;
        if (this.state.select == null)
            renderdItem = <ModuleList url="/api/Modules"
                onEdit={this.openEditor}
                onDelete={this.handleDelete} />;
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