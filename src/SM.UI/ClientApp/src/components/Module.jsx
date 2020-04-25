import React from 'react';



class Module extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            error: null,
            isLoaded: false,
            items: []
        }
    }

    componentDidMount() {
        fetch('https://localhost:44376/api/v1/Module')
            .then(res => res.json())
            .then((result) => this.setState({
                isLoaded: true,
                items: result
            }));
    }

    render() {
        return <div> {this.state.items.map((x) => <p>{x.module_ID}</p>)} </div>;
    }
}


export default Module;