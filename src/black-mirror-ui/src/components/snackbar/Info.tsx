import * as React from 'react';
import Snackbar from 'material-ui/Snackbar';

interface InfoProps {
    isOpen: boolean;
    text: string;
}

export default class Info extends React.Component<InfoProps> {

    constructor(props: InfoProps) {
        super(props);
        this.state = {
            open: false,
        };
    }

    handleClick = () => {
        this.setState({
            open: true,
        });
    }

    handleRequestClose = () => {
        this.setState({
            open: false,
        });
    }

    render() {
        return (

            <Snackbar
                open={this.props.isOpen}
                message={this.props.text}
                autoHideDuration={3000}
                onRequestClose={this.handleRequestClose}
            />
        );
    }
}