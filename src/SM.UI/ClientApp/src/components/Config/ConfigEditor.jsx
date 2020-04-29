import React from 'react';

import { FormGroup, Label, Input, Col, Row } from 'reactstrap';

import Editor from 'react-simple-code-editor';
import { highlight, languages } from 'prismjs';

import 'prismjs/components/prism-json';
import 'prismjs/components/prism-ini';

import 'prismjs/components/prism-css';
import 'prismjs/themes/prism-dark.css';

class ConfigEditor extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            config: {
                fileName: '',
                data: '',
                format: ''
            }
        }

        this.handleChange = this.handleChange.bind(this);
    }

    static getDerivedStateFromProps(props, state) {
        if (props.config === undefined)
            return {};
        return {
            config: props.config || {}
        }
    }

    handleChange(event) {
        this.props.onChange(event);
    }

    render() {
        var config = this.state.config;
        var highLang = languages[config.format];

        return (<>
            <Row form>
                <Col sm={8}>
                    <FormGroup >
                        <Label for="config.fileName">Dateiname der Konfig: </Label>
                        <Input type="text" name="config.fileName" id="config.fileName" placeholder="Konfigurationsdatei Name" value={config.fileName || ''} onChange={this.handleChange} />
                    </FormGroup>
                </Col>
                <Col sm={4}>
                    <FormGroup >
                        <Label for="config.format">Konfig Format: </Label>
                        <Input type="select" name="config.format" id="config.format" value={config.format || ''} onChange={this.handleChange}>
                            <option value={null}>Anderes (kein Highlighter)</option>
                            <option value="json">JSON</option>
                            <option value="xml">XML</option>
                            <option value="ini">INI</option>
                        </Input>
                    </FormGroup>
                </Col>
            </Row>
            <Row form>
                <Col>
                    <FormGroup>
                        <Label for="config.data">Konfig (Inhalt): </Label>
                        <Editor name="config.data"
                            className="form-control"
                            onChange={this.handleChange}
                            onValueChange={code=> code}
                            value={config.data || ''}
                            highlight={code => highLang === undefined ? code : highlight(code || '', highLang)}
                            padding={12}
                            style={{
                                height: 'auto',
                                minHeight: '200px',
                                fontFamily: '"Fira code", "Fira Mono", monospace',
                                fontSize: 12,
                            }}
                        />
                    </FormGroup>
                </Col>
            </Row>
            </>
        );
    }
}

export default ConfigEditor;