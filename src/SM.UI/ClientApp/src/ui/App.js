import React from 'react';
import { Layout } from 'antd';
import Navbar from './layout/Navbar';

import {
  BrowserRouter as Router,
} from "react-router-dom";

import './css/App.css'
import Body from './layout/Body';

const { Footer } = Layout;

class App extends React.Component {
  

  render() {
    return (
      <Router>
        <Layout  style={{minHeight: '100vh'}}>
          <Navbar />        
          <Layout className="site-layout">
            <Body></Body>
            <Footer style={{ textAlign: 'center' }}>Service Manager © {new Date().getFullYear()} Weiss GmbH Softwarelösungen</Footer>
          </Layout>
        </Layout>
      </Router>
    );
  }
}


export default App;