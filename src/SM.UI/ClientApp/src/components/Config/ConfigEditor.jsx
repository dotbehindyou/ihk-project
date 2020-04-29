import React from 'react';

import { FormGroup, Label, Input, Col, Row } from 'reactstrap';

import Editor from 'react-simple-code-editor';
import { highlight, languages } from 'prismjs';

import 'prismjs/components/prism-json';
import 'prismjs/components/prism-css';
import 'prismjs/themes/prism-dark.css';

class ConfigEditor extends React.Component {

    /*constructor(props) {
        super(props);
    }*/

    render() {
        var config = this.props.config || { };
        var highLang = config.format != undefined ? languages[config.format] : null;

        return (<>
            <Row form>
                <Col sm={8}>
                    <FormGroup >
                        <Label for="configName">Dateiname der Konfig: </Label>
                        <Input type="text" name="configName" id="configName" placeholder="Konfigurationsdatei Name" defaultValue={config.fileName} />
                    </FormGroup>
                </Col>
                <Col sm={4}>
                    <FormGroup >
                        <Label for="configName">Konfig Format: </Label>
                        <Input type="select" name="configFormat" id="configFormat" value={config.format || ''} onChange={this.handleChange}>
                            <option value="">Anderes (kein Highlighter)</option>
                            <option value="json">JSON</option>
                            <option value="xml">XML</option>
                        </Input>
                    </FormGroup>
                </Col>
            </Row>
            <Row form>
                <Col>
                    <FormGroup>
                        <Label for="configData">Konfig (Inhalt): </Label>
                        <Editor
                            className="form-control"
                            onValueChange={code => this.setState(prevState => {
                                let model = { ...prevState.model };
                                model.config = { ...model.config, data: code };
                                return { model };
                            })}
                            value={config.data || ''}
                            highlight={code => highLang === null ? code : highlight(code || '', highLang)}
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