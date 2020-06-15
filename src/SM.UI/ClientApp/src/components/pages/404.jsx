import React from "react";
import Content from "../../layout/Content";

class Error404 extends React.Component {
  render() {
    return (
      <Content>
        <h2>404</h2>
        <p>
          Diese Seite wurde oder konnte nicht geöffnet werden! Bitte wenden Sie
          sich an den Administrator, wenn diese Meldung nicht kommen sollte!{" "}
          <br />
          Überprüfen Sie ob Sie sich vertippt haben!
        </p>
      </Content>
    );
  }
}

export default Error404;
