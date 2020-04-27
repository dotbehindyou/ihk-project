import React from 'react';

import { Button, Form, FormGroup, Label, Input, ButtonGroup } from 'reactstrap';

class Module extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            model: props.model,
        };
    }

    close() {

    }

    save() {

    }

    render() {
        var model = this.state.model;
        return <div>
            <h2>{model.name} <small>{model.version}</small></h2>

            <Form>
                <FormGroup>
                    <Label for="name">Name des Moduls</Label>
                    <Input type="name" name="name" id="name" placeholder="Name vom Modul" defaultValue={model.name} />
                </FormGroup>
                <FormGroup>

                </FormGroup>
                <ButtonGroup>
                    <Button color="danger" type="button">Abbrechen</Button>
                    <Button color="primary" type="submit">Speichern</Button>
                </ButtonGroup>
            </Form>
        </div>;
        // TODO - Render Version list
        // TODO - Edit
    }
}


export default Module;