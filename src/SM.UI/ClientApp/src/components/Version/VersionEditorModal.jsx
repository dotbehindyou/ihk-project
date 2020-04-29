import React from 'react';

import { DatePickerInput } from 'rc-datepicker';

import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, Label, Input, ButtonGroup, Col, Row } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faWindowClose } from '@fortawesome/free-solid-svg-icons';
import { ConfigEditor } from '../Config/ConfigEditor';

class VersionEditorModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            moduleId: props.moduleId || null,
            version: props.version,
            model: {},
            isLoaded: false,
            isNew: props.version === undefined
        };

        this.close = this.close.bind(this);
        this.save = this.save.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount() {
        if (!this.state.isLoading && !this.state.isDone) {
            if (this.state.version != null) {
                this.load();
            }
        }
    }

    load() {
        return fetch('https://localhost:44376/api/v1/' + this.state.moduleId + '/versions/' + this.state.version) // TODO Addresse über Config auslesen lassen
            .then(res => res.json())
            .then((result) => {
                this.setState({
                    model: result,
                    isLoaded: true,
                });
            })
            .catch((ex) => { this.setState({ error: ex, model: {}, isLoaded: false }); });
    }

    close() {
        this.props.onClose();
    }

    save() {
        // TODO Speichern über API
        // Objekt zurückgeben für die Liste
    }

    onReleaseDateChange = (jsDate, dateString) => {
        // ...
    }

    handleChange(event) {
        console.log(event.target);
        //this.setState({ value: event.target.value });
    }

    handleSubmit(event) {
        alert('A name was submitted: ' + this.state.value);
        event.preventDefault();
    }

    render() {
        var model = this.state.model || {};

        var verIn;
        if (this.state.isNew) {
            verIn = <Col>
                <FormGroup > {/* Versions Nr. soll nach dem erstellen, nicht mehr änderbar sein! */}
                    <Label for="version">Version: </Label>
                    <Input type="text" name="version" id="version" placeholder="Version" defaultValue={model.version} />
                </FormGroup>
            </Col>
        }

        const closeBtn = <button className="close" onClick={this.close} style={{ color: '#dc3545' }}><FontAwesomeIcon icon={faWindowClose} /></button>;

        return (
            <Modal isOpen={this.props.isOpen} size="lg">
                <ModalHeader toggle={this.props.onToggle} close={closeBtn}>{this.props.moduleName || model.moduleName} <small>{(!this.state.isNew ? "("+model.version+")" : "neue Version hinzufügen")}</small></ModalHeader>
                <ModalBody>
                    <Form>
                        <Row form>
                            { verIn }
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
                        <hr />
                        <Row form>
                            <Col>
                                <h4>Konfigurationsdatei (Muster):</h4>
                            </Col>
                        </Row>
                        <ConfigEditor />
                    </Form>
                </ModalBody>
                <ModalFooter>
                    <ButtonGroup size="sm">
                        <Button onClick={this.close} outline color="danger" type="button">Abbrechen</Button>
                        <Button onClick={this.close} outline color="primary" type="submit">Speichern</Button>
                    </ButtonGroup>
                </ModalFooter>
            </Modal>
        );
    }
}

export default VersionEditorModal;