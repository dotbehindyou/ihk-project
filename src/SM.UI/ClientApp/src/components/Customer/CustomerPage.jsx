import React from 'react';
import CustomerList from './CustomerList';
import Customer from './Customer';

const debug = {
    "kdnr": 787158,
    "name": "Kasam Imsale",
    "auth_Token": "X3PuDX+D/1fvtzSpjq8iiDWemq5a2mM4RfTZ+lsLFwsCqzrgubg87TeFMoSr1yKZ0HkgMfd8Ss+3+mzjk7ACAA==",
    "isRegisterd": true
};

class CustomerPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            editor: {
                isOpen: true,
                item: debug
            }
        };

        this.handleOpenEdit = this.handleOpenEdit.bind(this);
        this.handleCloseEdit = this.handleCloseEdit.bind(this);
    }

    handleOpenEdit(item) {
        this.setState({ editor: { isOpen: true, item }});
    }

    handleCloseEdit() {
        this.setState({ editor: { isOpen: false, item: null } });
    }

    render() {
        var viewComp;

        if (this.state.editor.isOpen)
            viewComp = <Customer onClose={this.handleCloseEdit} model={this.state.editor.item} />
        else
            viewComp = <CustomerList onEdit={this.handleOpenEdit} />
        return <div>
            <h4>Kundenverwaltung</h4>
            <hr/>
            {viewComp}
        </div>;
    }
}

export default CustomerPage;