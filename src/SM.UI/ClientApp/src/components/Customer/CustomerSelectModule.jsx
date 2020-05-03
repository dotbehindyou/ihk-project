import React from 'react';

import { Button, Modal, ModalHeader, ModalBody, ModalFooter, ButtonGroup } from 'reactstrap';

import ModuleList from '../Module/ModuleList';
import VersionList from '../Version/VersionList';
import { faCheck } from '@fortawesome/free-solid-svg-icons';

class CustomerSelectModule extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            select: null,
            module: null
        }

        this.selectModule = this.selectModule.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleBack = this.handleBack.bind(this);
    }

    static getDerivedStateFromProps(props, state) {
        return {
            ...state,
            module: state.select || props.module
        }
    }

    selectModule(item) {
        this.setState({ select: item });
    }

    handleBack(e) {
        this.resetModule();
    }

    handleClose(e) {
        this.props.onClose(e);
        this.resetModule();
    }

    resetModule() {
        this.setState({ select: null });
    }

    render() {
        var list;
        if (this.state.module == null || this.state.module.module_ID == null) {
            list = <ModuleList isSelect={true} url="https://localhost:44376/api/v1/Modules" editIcon={faCheck} onEdit={this.selectModule} />;
        } else {
            list = <div><VersionList kdnr={this.props.kdnr} update={this.state.select === null} editIcon={faCheck} module={this.state.module} /> </div>;
        }

        return <Modal isOpen={this.props.isOpen} size="lg">
            <ModalHeader>
                {this.props.module ? "Version vom Modul ändern..." : "Modul zum Kunden zuweisen..."}
            </ModalHeader>
            <ModalBody>
                {list}
            </ModalBody>
            <ModalFooter>
                <ButtonGroup size="sm">
                    {this.state.select === null ? null : <Button onClick={this.handleBack} outline color="warning" type="button" >Zurück</Button>}
                    <Button onClick={this.handleClose} outline color="danger" type="button" >Schließen</Button>
                </ButtonGroup>
            </ModalFooter>
        </Modal>;
    }
}

export default CustomerSelectModule;