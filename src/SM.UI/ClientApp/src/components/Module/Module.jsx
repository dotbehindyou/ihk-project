import React from 'react';

import { Button, Form, Label, Input, Row, Col, InputGroup, InputGroupAddon } from 'reactstrap';
import VersionList from '../Version/VersionList';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faWindowClose } from '@fortawesome/free-solid-svg-icons';

class Module extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isSaving: false,
            isNew: props.model.module_ID === undefined,
            model: props.model,
        };

        this.close = this.close.bind(this);
        this.save = this.save.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleDelete = this.handleDelete.bind(this);
    }

    close() {
        if (this.props.onClose)
            this.props.onClose();
    }

    handleChange(event) {
        var model = this.state.model
        model[event.target.name] = event.target.value;

        this.setState({ model: { ...model } });
        console.log(this.state);
    }

    save() {
        this.setState({ isSaving: true });
        fetch('https://localhost:44376/api/v1/Modules/' + (this.state.isNew ? '' : this.state.model.module_ID),
            {
                headers: {
                    "Content-Type": "application/json",
                },
                method: this.state.isNew ? 'POST' : 'PUT',
                cache: 'no-cache',
                body: JSON.stringify(this.state.model)
            })
            .then(res => res.json())
            .then((result) => {
                this.setState({
                    model: { ...result },
                    isNew: false
                });
            }) // TODO Neues Element an die VersionsListe hinzufügen
            .catch((ex) => {
                console.warn(ex);
            })
            .finally(() => {
                this.setState({ isSaving: false });
            });
    }

    handleDelete(item) {
        fetch('https://localhost:44376/api/v1/' + item.module_ID + '/versions/' + item.version,
            {
                method: 'DELETE'
            })
            .then((res) => res.json())
            .then(res => {

            })
            .catch((ex) => console.log(ex));
    }

    render() {
        var model = this.state.model;

        var listCom;
        if (this.state.isNew === false)
            listCom = (<>
                <hr />
                <h4>Versionen: </h4>
                <VersionList module={model}
                    onDelete={this.handleDelete}/>
            </>);

        return <div>
            <Row>
                <Col>
                    <h2>{model.name} <small>{this.state.isNew ? 'als neues Module hinzufügen' : '('+(model.version || 'noch keine Version')+')'}</small></h2>
                </Col>
                <Col style={{ textAlign: 'right' }}>
                    <Button outline color="danger" size="sm" onClick={this.close}><FontAwesomeIcon icon={faWindowClose} /></Button>
                </Col>
            </Row>
            <hr />
            <Form>
                <Label for="name">Name des Moduls</Label>
                <InputGroup>
                    <Input type="text" name="name" id="name" placeholder="Name vom Modul" value={model.name} onChange={this.handleChange}/>
                    <InputGroupAddon addonType="append">
                        <Button outline color="primary" type="button" onClick={this.save}>{this.state.isNew ? 'Erstellen' : 'Übernehmen'}</Button>
                    </InputGroupAddon>
                </InputGroup>
            </Form>
            {listCom}
        </div>;
    }
}


export default Module;