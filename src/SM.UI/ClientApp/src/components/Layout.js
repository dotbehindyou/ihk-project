import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
      return (
          <div style={{ background: '#000', color: '#fff', height: '100vh', overflowY: 'auto' }}>
              <NavMenu />
              <p />
            <Container>
              {this.props.children}
            </Container>
        </div>
    );
  }
}
