import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import ModulePage from './components/Module/ModulePage';

import 'moment/locale/de.js';
import 'rc-datepicker/lib/style.css';
import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <Layout>
            <Route exact path='/' component={ModulePage} />
        </Layout>
    );
  }
}
