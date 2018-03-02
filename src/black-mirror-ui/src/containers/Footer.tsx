import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import * as React from 'react';
import Settings from 'material-ui/svg-icons/action/settings';
import Dashboard from 'material-ui/svg-icons/action/dashboard';
import Code from 'material-ui/svg-icons/action/code';
import Account from 'material-ui/svg-icons/action/account-circle';
import { BottomNavigation, BottomNavigationItem } from 'material-ui/BottomNavigation';
import Paper from 'material-ui/Paper';
import UserInterface from '../actions/interfaces/User';
import { withRouter } from 'react-router-dom';

interface FooterProps {
  // tslint:disable-next-line:no-any
  theme: any;
  user: UserInterface;
  // tslint:disable-next-line:no-any
  history: any;
}
// tslint:disable-next-line:no-any
class Footer extends React.Component<FooterProps, any> {

  get selectedIndex() {
    if (this.props.history.location.pathname.startsWith('/repository')) { return 1; }
    if (this.props.history.location.pathname.startsWith('/user')) { return 2; }
    if (this.props.history.location.pathname.startsWith('/settings')) { return 3; }
    return 0;
  }

  render() {
    return (
      <div>
        <MuiThemeProvider muiTheme={this.props.theme}>
          <Paper zDepth={1}>
            <BottomNavigation selectedIndex={this.selectedIndex}>
              <BottomNavigationItem
                label="Mirrors"
                icon={<Dashboard />}
                onClick={() => { this.props.history.push('/'); }}
              />
              <BottomNavigationItem
                label="Repositories"
                icon={<Code />}
                onClick={() => { this.props.history.push('/repository'); }}
              />
              <BottomNavigationItem
                label="Users"
                icon={<Account />}
                onClick={() => { this.props.history.push('/user'); }}
              />
              <BottomNavigationItem
                label="Settings"
                icon={<Settings />}
                onClick={() => { this.props.history.push('/settings'); }}
              />
            </BottomNavigation>
          </Paper>
        </MuiThemeProvider>
      </div>
    );
  }
}

export default withRouter(Footer);
