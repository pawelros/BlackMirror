import * as React from 'react';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import { Switch, Route } from 'react-router-dom';
import MirrorList from '../components/mirrorList/MirrorList';
import Mirror from '../components/mirror/Mirror';
import RepositoryList from '../components/repositoryList/RepositoryList';
import Repository from '../components/repository/Repository';
import UserList from '../components/userList/UserList';
import User from '../components/user/User';
import SyncLogs from '../components/syncLogs/SyncLogs';
import SyncAdd from '../components/syncAdd/SyncAdd';
import Settings from '../components/settings/Settings';
import IUser from '../actions/interfaces/User';
import { Tab, Tabs } from 'material-ui';
import { withRouter } from 'react-router';
import Dashboard from 'material-ui/svg-icons/action/dashboard';
import Code from 'material-ui/svg-icons/action/code';
import Account from 'material-ui/svg-icons/action/account-circle';
import Sets from 'material-ui/svg-icons/action/settings';

interface MainProps {
    theme: __MaterialUI.Styles.MuiTheme;
    user: IUser;
    history?: any;
}

class Main extends React.Component<MainProps, any> {

    constructor(props: MainProps) {
        super(props);
    }

    get initialTabIndex() {
        if (this.props.history.location.pathname == null) { return 0; }
        if (this.props.history.location.pathname.startsWith('/repository')) { return 1; }
        if (this.props.history.location.pathname.startsWith('/user')) { return 2; }
        if (this.props.history.location.pathname.startsWith('/settings')) { return 3; }
        return 0;
    }

    handleActive = (tab: any) => {
        switch (tab.props['data-route']) {
            case '/repository':
                this.props.history.push('/repository');
                break;
            case '/user':
                this.props.history.push('/user');
                break;
            case '/settings':
                this.props.history.push('/settings');
                break;
            default:
                this.props.history.push('/');
                break;
        }
    }

    render() {
        return (
            <MuiThemeProvider muiTheme={this.props.theme}>
                <div>
                    <Tabs value={this.initialTabIndex}>
                        <Tab
                            label="Mirrors"
                            value={0}
                            icon={<Dashboard />}
                            data-route="/"
                            onActive={this.handleActive}
                        />
                        <Tab
                            label="Repositories"
                            value={1}
                            icon={<Code />}
                            data-route="/repository"
                            onActive={this.handleActive}
                        />
                        <Tab
                            label="Users"
                            value={2}
                            icon={<Account />}
                            data-route="/user"
                            onActive={this.handleActive}
                        />
                        <Tab
                            label="Settings"
                            value={3}
                            icon={<Sets />}
                            data-route="/settings"
                            onActive={this.handleActive}
                        />
                    </Tabs>
                </div>
                <div>
                    <Switch>
                        <Route exact={true} path="/" component={MirrorList} />
                        <Route exact={true} path="/mirror" component={MirrorList} />
                        <Route exact={false} path="/mirror/:id" component={Mirror} />
                        <Route exact={true} path="/repository" component={RepositoryList} />
                        <Route exact={false} path="/repository/:id" component={Repository} />
                        <Route exact={true} path="/user" component={UserList} />
                        <Route exact={false} path="/user/:id" component={User} />
                        <Route exact={true} path="/sync/:id/logs" component={SyncLogs} />
                        <Route exact={true} path="/sync/add" component={SyncAdd} />
                        <Route path="/settings" component={Settings} />
                    </Switch>
                </div>
            </MuiThemeProvider>
        );
    }
}
export default withRouter(Main);
