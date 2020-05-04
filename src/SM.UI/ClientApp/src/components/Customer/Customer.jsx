import React from 'react';

import { Button, Label, Input, Row, Col, InputGroup, InputGroupAddon, Tooltip } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faWindowClose, faCopy } from '@fortawesome/free-solid-svg-icons';
import ModuleList from '../Module/ModuleList';
import CustomerSelectModule from './CustomerSelectModule';
import VersionEditorModal from '../Version/VersionEditorModal';

class Customer extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            _s: {
                copyTipOpen: false,
                isModuleOpen: false,
                module: null
            },
            name: '',
            kdnr: -1,
            isRegisterd: false,
            ...props.model,
        }

        this.close = this.close.bind(this);
        this.copyAuthToken = this.copyAuthToken.bind(this);
        this.openCopyTip = this.openCopyTip.bind(this);
        this.handleChangeModule = this.handleChangeModule.bind(this);
        this.handleChangeVersion = this.handleChangeVersion.bind(this);
        this.closeModuleModal = this.closeModuleModal.bind(this);
        this.handleRemoveModule = this.handleRemoveModule.bind(this);

        this.authTokenField = null;
    }

    componentDidMount() {
        if (this.state.isRegisterd === false) {
            this.initCustomer();
        }
        this.authTokenField = document.getElementById('auth_token');
    }

    initCustomer() {
        fetch('https://localhost:44376/api/v1/Customers/' + this.state.kdnr, {
                    method: 'PUT'
                })
            .then((res) => res.json())
            .then((res) => {
                this.setState({ ...res });
            })
            .catch((ex) => console.log(ex));
    }

    close(e) {
        this.props.onClose(e);
    }

    copyAuthToken(e) {
        this.authTokenField.focus();
        this.authTokenField.select();
        document.execCommand('copy');
        e.target.focus();

        this.openCopyTip();
    }

    openCopyTip(s) {
        this.setState((prevState) => { return { _s: { ...prevState._s, copyTipOpen: true } } });

        setTimeout(() => {
            this.setState((prevState) => { return { _s: { ...prevState._s, copyTipOpen: false } } });
        }, 2000);
    }

    handleChangeModule(item) {
        this.setState((prevState) => { return { _s: { ...prevState._s, isModuleOpen: true, module: item } } });
    }

    handleChangeVersion(item) {
        this.setState((prevState) => { return { _s: { ...prevState._s, isModuleOpen: true, module: (item ? { ...item, version: null } : null )} } });
    }

    handleRemoveModule(item) {
        console.log(item);
        fetch('https://localhost:44376/api/v1/Modules/Customer/' + this.state.kdnr,
            {
                headers: {
                    "Content-Type": "application/json"
                },
                method: 'DELETE',
                body: JSON.stringify(item)
            })
            .then((res) => res.json())
            .then(res => {
            })
            .catch((ex) => console.log(ex));
    }

    closeModuleModal(e) {
        this.setState((prevState) => {
            return {
                _s: {
                    ...prevState._s,
                    isModuleOpen: false,
                    module: null
                }
            };
        });
    }

    render() {
        var model = this.state;

        var edit;
        if (this.state._s.module != null && this.state._s.module.version != null) {
            edit = <VersionEditorModal
                changeConfig={true}
                kdnr={this.state.kdnr}
                moduleId={this.state._s.module.module_ID}
                moduleName={this.state._s.module.name}
                version={this.state._s.module.version}
                isOpen={this.state._s.isModuleOpen}
                onClose={this.closeModuleModal} />;

        } else {
            edit = <CustomerSelectModule
                kdnr={this.state.kdnr}
                module={this.state._s.module}
                isOpen={this.state._s.isModuleOpen}
                onClose={this.closeModuleModal} />;
        }

        return <div>
            <Row>
                <Col>
                    <h2>{model.name} <small>({this.state.kdnr})</small></h2>
                </Col>
                <Col style={{ textAlign: 'right' }}>
                    <Button outline color="danger" size="sm" onClick={this.close}><FontAwesomeIcon icon={faWindowClose} /></Button>
                </Col>
            </Row>
            <Row>
                <Col>
                    <hr />
                    <h4>Daten:</h4>
                    <Label for="auth_token">Auth-Token für den Service:</Label>
                    <InputGroup>
                        <Input id="auth_token" type="text" readOnly value={this.state.auth_Token || ''} />
                        <InputGroupAddon addonType="append">
                            <Button onClick={this.copyAuthToken} id="s_copy">Kopieren <FontAwesomeIcon icon={faCopy} /></Button>
                            <Tooltip placement="top" target="s_copy" isOpen={this.state._s.copyTipOpen}>
                                Kopiert!
                            </Tooltip>
                        </InputGroupAddon>
                    </InputGroup>
                </Col>
                <Col sm={8}>
                    <hr />
                    <h4>Installierte Module:</h4>
                    <ModuleList url={"https://localhost:44376/api/v1/Modules/Customer/" + this.state.kdnr}
                        onEdit={this.handleChangeModule}
                        onDelete={this.handleRemoveModule}
                        onChangeVersion={this.handleChangeVersion}
                        showStatus={true} />
                </Col>
            </Row>
            {edit}
        </div>;
    }
}

export default Customer;