import React from 'react';
import { Layout } from 'antd';
import { Switch, Route } from 'react-router-dom';

import CustomerPage from '../components/pages/CustomerPage';
import Error404 from '../components/pages/404';
import ServicePage from '../components/pages/ServicePage';

const { Content } = Layout;

class Body extends React.Component {
    
    render() {
        return (
            <Content style={{ margin: '0 16px' }}>
                <br />
                <Switch>
                    <Route path="/" exact component={CustomerPage} />
                    <Route path="/Services" component={ServicePage} />
                    <Route component={Error404} />
                </Switch>
            </Content>
        );
    }
}

/*

import { Breadcrumb } from 'antd';
                <Breadcrumb style={{ margin: '16px 0' }}>
                    <Breadcrumb.Item>User</Breadcrumb.Item>
                    <Breadcrumb.Item>Bill</Breadcrumb.Item>
                </Breadcrumb>
*/

export default Body;