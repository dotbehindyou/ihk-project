import React from 'react';
import { Layout, Menu } from 'antd';
import {
  UsergroupAddOutlined,
  CodeOutlined,
} from '@ant-design/icons';

import { Link } from "react-router-dom";

const { Sider } = Layout;

class Navbar extends React.Component{
    state = {
        collapsed: false,
    };

    onCollapse = collapsed => {
        this.setState({collapsed});
    };

    getSelectKey() {
      let result = [];
      let path = window.location.pathname;
      if(path === "/"){
        result = ["/"];
      }
      else{
        result = path.split('/');
      }
      return result;
    }

    render(){
      let selectKey = this.getSelectKey();
        return (<Sider
            collapsed={this.state.collapsed}
            collapsible
            onCollapse={this.onCollapse}>
          <div className="logo">
            <span className={this.state.collapsed ? "sm" : "bg"}>Service Manager</span>
          </div>
          <Menu mode="inline" defaultSelectedKeys={selectKey}>
            <Menu.Item key="/" icon={<UsergroupAddOutlined />}>
              <span>Kunden</span>
              <Link to="/" />
            </Menu.Item>
            <Menu.Item key="Services" icon={<CodeOutlined />}>
              <span>Dienste</span>
              <Link to="/Services" />
            </Menu.Item>
          </Menu>
        </Sider>);
    }
}

export default Navbar;