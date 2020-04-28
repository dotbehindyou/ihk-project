import React from 'react';

import { DatePickerInput } from 'rc-datepicker';

import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, Label, Input, ButtonGroup, Col, Row } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faWindowClose } from '@fortawesome/free-solid-svg-icons';
// import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

class VersionEditorModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            model: props.model,
            changes: {}
        }

        this.close = this.close.bind(this);
        this.save = this.save.bind(this);
    }

    static getDerivedStateFromProps(props, state) {
        if (props.isOpen) {

            return {
                model: props.model,
                changes: {}
            };
        }

        return {
            model: null,
            changes: {}
        };
    }

    close() {

    }

    save() {

    }

    onReleaseDateChange = (jsDate, dateString) => {
        // ...
    }

    render() {
        var model = this.props.model;

        const closeBtn = <button className="close" onClick={this.close} style={{ color: '#dc3545' }}><FontAwesomeIcon icon={faWindowClose} /></button>;

        return (
            <Modal isOpen={this.props.isOpen} toggle={this.props.onToggle} size="lg">
                <ModalHeader toggle={this.props.onToggle} close={closeBtn}>{model.moduleName} ({model.version})</ModalHeader>
                <ModalBody>
                    <Form>
                        <Row form>
                            <Col>
                                <FormGroup > {/* Versions Nr. soll nach dem erstellen, nicht mehr änderbar sein! */}
                                    <Label for="version">Version: </Label>
                                    <Input type="text" name="version" id="version" placeholder="Version" defaultValue={model.version} />
                                </FormGroup>
                            </Col>
                            <Col>
                                <FormGroup >
                                    <Label for="releaseDate">Veröffentlich: </Label>
                                    <DatePickerInput
                                        onChange={this.onReleaseDateChange}
                                        value={model.releaseDate}
                                        className='my-custom-datepicker-component' />
                                </FormGroup>
                            </Col>
                        </Row>
                        <Row form>
                            <Col>
                                <FormGroup >
                                    <Label for="version">Version: </Label>
                                    <Input type="text" name="version" id="version" placeholder="Version" defaultValue={model.version} />
                                </FormGroup>
                            </Col>
                        </Row>
                        <FormGroup>
                            <Label for="version">Config: </Label>
                            <Input type="text" name="version" id="version" placeholder="Version" defaultValue={model.config} />
                        </FormGroup>
                    </Form>
                </ModalBody>
                <ModalFooter>
                    <ButtonGroup size="sm">
                        <Button outline color="danger" type="button">Abbrechen</Button>
                        <Button outline color="primary" type="submit">Speichern</Button>
                    </ButtonGroup>
                </ModalFooter>
            </Modal>
        );
    }
}

export default VersionEditorModal;