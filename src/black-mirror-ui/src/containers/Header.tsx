import * as React from 'react';
import Avatar from 'material-ui/Avatar';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import AppBar from 'material-ui/AppBar';
import User from '../components/User';
const Logo = require('../images/mirror-reflection.svg');

import UserInterface from '../actions/interfaces/User';
import { withRouter } from 'react-router-dom';

interface HeaderProps {
    // tslint:disable-next-line:no-any
    theme: any;
    user: UserInterface;
    // tslint:disable-next-line:no-any
    history: any; 
}

const style = {
    avatar: {
        cursor: 'pointer'
    }
};

class Header extends React.Component<HeaderProps, {}> {
    render() {

        return (
            <MuiThemeProvider muiTheme={this.props.theme}>
                <div id="avatar">
                    <AppBar
                        title="Black Mirror"
                        iconStyleLeft={style.avatar}
                        titleStyle={style.avatar}
                        iconElementLeft={<Avatar src={Logo} className="header-logo" size={70} />}
                        onLeftIconButtonTouchTap={() => { this.props.history.push('/'); }}
                        onTitleTouchTap={() => { this.props.history.push('/'); }}
                    >
                        <User
                            user={this.props.user.Auth.Name}
                            secondaryText={this.props.user.Auth.Pid}
                            nestedLevel={0}
                        />
                    </AppBar></div>
            </MuiThemeProvider >
        );
    }
}

export default withRouter(Header);
