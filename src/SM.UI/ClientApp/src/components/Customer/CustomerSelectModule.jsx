import React from 'react';

import { Button, Label, Input, Row, Col, InputGroup, InputGroupAddon, Tooltip, Modal, ModalHeader, ModalBody, ModalFooter, ButtonGroup } from 'reactstrap';

import ModuleList from '../Module/ModuleList';
import VersionList from '../Version/VersionList';
import { faCheck } from '@fortawesome/free-solid-svg-icons';

class CustomerSelectModule extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            module: null
        }

        this.selectModule = this.selectModule.bind(this);
    }

    selectModule(item) {
        this.setState({ module: item });
    }

    render() {
        var list = this.state.module;
        if (this.state.module === null) {
            list = <ModuleList url="https://localhost:44376/api/v1/Modules" editIcon={faCheck} onEdit={this.selectModule} />;
        } else {
            list = <div><VersionList module={this.state.module} /> </div>;
        }

        return <Modal isOpen={this.props.isOpen} size="lg">
            <ModalHeader>
                Modul zum Kunden zuweisen...
            </ModalHeader>
            <ModalBody>
                {list}
            </ModalBody>
            <ModalFooter>
                <ButtonGroup size="sm">
                    <Button onClick={this.props.onClose} outline color="danger" type="button">Abbrechen</Button>
                    <Button disabled={this.state.isSaving} onClick={this.save} outline color="primary" type="submit">Speichern</Button>
                </ButtonGroup>
            </ModalFooter>
        </Modal>;
    }
}

export default CustomerSelectModule;