import React from 'react';

class Content extends React.Component {
    
    render() {
        if(this.props.hide){
            return null;
        }
        return (
            <div className="site-layout-background">
                {this.props.children}
            </div>
        );
    }
}


export default Content;