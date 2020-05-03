import React from 'react';

import { DatePickerInput } from 'rc-datepicker';

import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, Label, Input, ButtonGroup, Col, Row } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faWindowClose } from '@fortawesome/free-solid-svg-icons';
import ConfigEditor from '../Config/ConfigEditor';

class VersionEditorModal extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            moduleId: props.moduleId || null,
            version: props.version,
            kdnr: props.kdnr,
            model: {
                config: {},
            },
            file: null,
            isLoaded: false,
            isSaving: false,
            isNew: props.version === undefined,
            update: props.changeConfig || props.changeVersion
        };

        this.close = this.close.bind(this);
        this.save = this.save.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.selectVersionFile = this.selectVersionFile.bind(this);
    }

    componentDidMount() {
        if (!this.state.isLoading && !this.state.isDone) {
            if (this.state.version != null) {
                this.load();
            }
        }
    }

    load() {
        return fetch('https://localhost:44376/api/v1/' + this.state.moduleId + '/versions/' + this.state.version + (this.props.changeConfig ? '/' + this.props.kdnr : '')) // TODO Addresse über Config auslesen lassen
            .then(res => res.json())
            .then((result) => {
                if (this.state.update) {
                    result.status = "UPDATE";
                }

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
        this.setState({ isSaving: true });
        if (this.props.kdnr === undefined) { // Komplett neue Version erstellen
            fetch('https://localhost:44376/api/v1/' + this.state.moduleId + '/versions/' + (this.state.isNew ? '' : this.state.version),
                {
                    headers: {
                        "Content-Type": "application/json"
                    },
                    method: this.state.isNew ? 'POST' : 'PUT',
                    cache: 'no-cache',
                    body: JSON.stringify(this.state.model)
                })
                .then(res => res.json())
                .then(async (result) => {
                    this.setState({
                        model: { ...result },
                        isNew: false
                    });

                    if (this.state.file !== null) {
                        await this.uploadFile();

                    }
                }) // TODO VersionsListe aktualliseren, VersionsList
                .catch((ex) => {
                    console.warn(ex);
                })
                .finally(() => {
                    this.setState({ isSaving: false });
                });

        } else {  // Version beim Kunden konfigurieren
            fetch('https://localhost:44376/api/v1/Modules/Customer/' + this.props.kdnr,
                {
                    headers: {
                        "Content-Type": "application/json"
                    },
                    method: this.state.update ? 'PUT' : 'POST',
                    cache: 'no-cache',
                    body: JSON.stringify(this.state.model)
                })
                .then(res => res.json())
                .then(async (result) => {
                    this.setState({
                        model: { ...result },
                        isNew: false
                    });
                    this.close();
                })
                .catch((ex) => {
                    console.warn(ex);
                })
                .finally(() => {
                    this.setState({ isSaving: false });
                });
        }
    }

    async uploadFile() {
        if (this.state.isNew)
            return false;

        this.setState({ isUploading: true });

        var formData = new FormData();
        formData.append('versionFile', this.state.file);

        return await fetch('https://localhost:44376/api/v1/' + this.state.moduleId + '/versions/' + this.state.version + '/file',
            {
                method: this.state.isNew ? 'POST' : 'PUT',
                cache: 'no-cache',
                body: formData
            })
            .then(res => res.json())
            .then(async (result) => {
                return result;
            })  // TODO VersionsListe aktualliseren, VersionsList
            .catch((ex) => {
                console.warn(ex);
            })
            .finally(() => {
                this.setState({ isUploading: false });
            });

    }

    selectVersionFile(event) {
        this.setState({
            file: event.target.files[0]
        });
    }

    handleChange(event) {
        var model = this.state.model
        if (event.target === undefined)
            return;
        if (event.target.name.includes("config")) {
            var s = event.target.name.split('.');
            model.config[s[1]] = event.target.value;
        } else {
            model[event.target.name] = event.target.value;
        }
        this.setState({ model: { ...model } });
    }

    render() {
        var model = this.state.model || {};

        var verIn;
        if (this.state.isNew) {
            verIn = <Col>
                <FormGroup > {/* Versions Nr. soll nach dem erstellen, nicht mehr änderbar sein! */}
                    <Label for="version">Version: </Label>
                    <Input type="text" name="version" id="version" placeholder="Version" value={model.version} onChange={this.handleChange} />
                </FormGroup>
            </Col>
        }

        const closeBtn = <button className="close" onClick={this.close} style={{ color: '#dc3545' }}><FontAwesomeIcon icon={faWindowClose} /></button>;

        return (
            <Modal isOpen={this.props.isOpen} size="lg">
                <ModalHeader toggle={this.props.onToggle} close={closeBtn}>{this.props.moduleName || model.moduleName} <small>{(!this.state.isNew ? "("+model.version+")" : "neue Version hinzufügen")}</small></ModalHeader>
                <ModalBody>
                    <Form>
                        {
                            this.props.kdnr === undefined ?
                                (<>
                                <Row form>
                                    { verIn }
                                    <Col>
                                        <FormGroup >
                                            <Label for="releaseDate">Veröffentlich: </Label>
                                            <DatePickerInput
                                                name="releaseDate"
                                                onChange={(data) => { this.handleChange({ target: { name: 'releaseDate', value: data }}); }}
                                                value={model.releaseDate}
                                                className='my-custom-datepicker-component' />
                                        </FormGroup>
                                    </Col>
                                </Row>
                                <hr />
                                <Row form>
                                    <Col>
                                        <h4>Versionsdatei (ZIP):</h4>
                                        <small>In der Versionsdatei darf die Konfigdatei NICHT hinterlegt sein!</small>
                                    </Col>
                                </Row>
                                <Row form>
                                    <Col>
                                        <Input type="file" name="file" id="file" onChange={this.selectVersionFile} /> {/* TODO Dateiupload */}
                                    </Col>
                                </Row>
                                    <hr />
                                </>)
                                : null
                        }
                        <Row form>
                            <Col>
                                <h4>Konfigurationsdatei  {this.props.kdnr === undefined ? "(Muster)" : "(Kunde)"}:</h4>
                            </Col>
                        </Row>
                        <ConfigEditor isSelect={this.props.kdnr !== undefined} onChange={this.handleChange} config={model.config || {}}/>
                    </Form>
                </ModalBody>
                <ModalFooter>
                    <ButtonGroup size="sm">
                        <Button onClick={this.close} outline color="danger" type="button">Abbrechen</Button>
                        <Button disabled={this.state.isSaving} onClick={this.save} outline color="primary" type="submit">{(this.props.kdnr !== undefined && !this.props.changeConfig ? (this.props.changeVersion ? "Version ändern" : "Hinzufügen") : "Speichern")}</Button>
                    </ButtonGroup>
                </ModalFooter>
            </Modal>
        );
    }
}

export default VersionEditorModal;