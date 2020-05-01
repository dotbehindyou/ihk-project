import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import ModulePage from './components/Module/ModulePage';

import 'moment/locale/de.js';
import 'rc-datepicker/lib/style.css';
import './custom.css'
import CustomerPage from './components/Customer/CustomerPage';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <Route exact path='/Modules' component={ModulePage} />
            <Route exact path='/' component={CustomerPage} />
        </Layout>
    );
  }
}
