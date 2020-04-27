import React from 'react';

import { Button, Form, FormGroup, Label, Input, ButtonGroup, Row, Col } from 'reactstrap';
import VersionList from '../Version/VersionList';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faWindowClose } from '@fortawesome/free-solid-svg-icons';

class Module extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            model: props.model,
        };

        this.close = this.close.bind(this);
        this.save = this.save.bind(this);
    }

    close() {
        if (this.props.onClose)
            this.props.onClose();
    }

    save() {

    }

    render() {
        var model = this.state.model;
        return <div>
            <Row>
                <Col>
                    <h2>{model.name} <small>{model.version}</small></h2>
                </Col>
                <Col style={{ textAlign: 'right' }}>
                    <Button color="danger" size="sm" onClick={this.close}><FontAwesomeIcon icon={faWindowClose} /></Button>
                </Col>
            </Row>
            <hr />
            <Form>
                <FormGroup>
                    <Label for="name">Name des Moduls</Label>
                    <Input type="name" name="name" id="name" placeholder="Name vom Modul" defaultValue={model.name} />
                </FormGroup>
                <ButtonGroup size="sm">
                    <Button color="danger" type="button">Abbrechen</Button>
                    <Button color="primary" type="submit">Speichern</Button>
                </ButtonGroup>
            </Form>
            <hr />
            <h4>Versionen: </h4>
            <VersionList module={model.module_ID} />
        </div>;
        // TODO - Render Version list
        // TODO - Edit
    }
}


export default Module;