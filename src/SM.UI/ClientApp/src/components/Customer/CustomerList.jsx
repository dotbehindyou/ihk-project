import React from 'react';
import { Table, ButtonGroup, Button, Row, Col, Input, FormGroup, Form, InputGroupAddon, InputGroup } from 'reactstrap';
import CustomerListItem from './CustomerListItem';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft, faArrowRight, faSearch } from '@fortawesome/free-solid-svg-icons';


class CustomerList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            items: [],
            search: {
                kdnr: '',
                kdnrCondition:  'same',
                name: ''
            },
            editor: {
                isOpen: false,
                item: null
            },
            page: 0,
            pageCount: 20
        }

        this.changePage = this.changePage.bind(this);
        this.getMaxPage = this.getMaxPage.bind(this);
        this.searchCustomer = this.searchCustomer.bind(this);
    }

    getMaxPage() {
        if (this.state.items.length > this.state.pageCount)
            return parseInt(this.state.items.length / this.state.pageCount) - 1;
        return 0;
    }

    componentDidMount() {
        fetch('https://localhost:44376/api/v1/Customers/')
            .then(res => res.json())
            .then((result) => {
                this.setState((prevState) => { return { items: result } });
            })
            .catch((ex) => {
                console.log(ex);
            });
    }

    searchCustomer() {
        var uri = new URL('https://localhost:44376/api/v1/Customers/Search');
        Object.keys(this.state.search).forEach(key => uri.searchParams.append(key, this.state.search[key]));
        fetch(uri)
            .then(res => res.json())
            .then((result) => {
                this.setState((prevState) => { return { items: result } });
            })
            .catch((ex) => {
                console.log(ex);
            });
    }

    changePage(pageIndex) {
        this.setState({ page: pageIndex });
    }

    render() {
        var listCom = [];
        var pageFirst = this.state.page * this.state.pageCount;
        var pageLast = (this.state.page + 1) * this.state.pageCount;
        for (var i = pageFirst; i < this.state.items.length && i < pageLast; ++i) {
            var x = this.state.items[i];
            listCom.push( < CustomerListItem key={x.kdnr} model={x} /> );
        }

        var maxPage = this.getMaxPage();

        return <>
            <Row>
                <Col>
                    <Form inline>
                        <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                            <InputGroup>
                                <InputGroupAddon addonType="prepend">Kunden Nr:</InputGroupAddon>
                                <InputGroupAddon addonType="prepend">
                                    <Input type="select" onChange={(e) => this.setState({ search: { ...this.state.search, kdnrCondition: e.target.value } })}>
                                        <option value="same">genau</option>
                                        <option value="like">ungefähr</option>
                                    </Input>
                                </InputGroupAddon>
                                <Input type="number" id="s.kdnr" value={this.state.search.kdnr} onChange={(e) => this.setState({ search: { ...this.state.search, kdnr: e.target.value } })} />
                            </InputGroup>
                        </FormGroup>
                        <FormGroup className="mb-2 mr-sm-2 mb-sm-0">
                            <InputGroup>
                                <InputGroupAddon addonType="prepend">Name:</InputGroupAddon>
                                <Input type="text" id="s.name" value={this.state.search.name} onChange={(e) => this.setState({ search: { ...this.state.search, name: e.target.value } }) } />
                            </InputGroup>
                        </FormGroup>
                        <Button outline color="success" onClick={this.searchCustomer}>Suchen <FontAwesomeIcon icon={faSearch} /></Button>
                    </Form>
                </Col>
            </Row>
            <br />
            <Row>
                <Col>
                    <Table bordered={true} size="sm">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Kunden Nr.</th>
                                <th>Name</th>
                            </tr>
                        </thead>
                        <tbody>
                            {listCom}
                        </tbody>
                    </Table>
                </Col>
            </Row>
            <Row>
                <Col>
                    <ButtonGroup size="sm">
                        <Button disabled={this.state.page === 0} onClick={() => { this.changePage(this.state.page - 1); }}><FontAwesomeIcon icon={faArrowLeft} /></Button>
                        <Button onClick={() => { this.changePage(0) }} outline>Seite 1</Button>
                        <Button onClick={() => { this.changePage(maxPage) }} outline>Seite {maxPage + 1}</Button>
                        <Button disabled={this.state.page === maxPage} onClick={() => { this.changePage(this.state.page + 1); }}><FontAwesomeIcon icon={faArrowRight} /></Button>
                    </ButtonGroup>
                </Col>
                <Col>
                    <p>Seite {this.state.page + 1}</p>
                </Col>
            </Row>
        </>;
    }
}

export default CustomerList;