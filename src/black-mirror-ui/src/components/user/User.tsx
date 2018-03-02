import * as React from 'react';
import { Switch, Route, withRouter } from 'react-router-dom';

import Paper from 'material-ui/Paper';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import Sets from 'material-ui/svg-icons/action/settings';
import Home from 'material-ui/svg-icons/action/home';
import { grey800 } from 'material-ui/styles/colors';
import RestApi from '../../actions/restApi';
import Overview from './Overview';
import Settings from './settings/Settings';
import { Grid, Row, Col } from 'react-flexbox-grid';
import { observer } from 'mobx-react';
import { observable } from 'mobx';
import IUser from '../interfaces/User';

interface UserProps {
    history?: any;
    match?: any;
}

interface UserState {
    user?: IUser;
    listInterval: any;
}

@observer
class User extends React.Component<UserProps, UserState> {

    get isOnSettingsPage() {
        return this.props.history.location.pathname.match(/^\/user\/.*\/settings\/?$/);
    }

    get isOnOverviewPage() {
        return this.props.history.location.pathname.match(/^\/user\/\w*\/?$/);
    }

    constructor(props: UserProps) {
        super(props);

        this.state = {
            listInterval: false,
        };

        this.componentDidMount = () => {
            this.loadData();
        };

        this.componentWillUnmount = () => {
            // tslint:disable-next-line:no-unused-expression
            this.state.listInterval && clearInterval(this.state.listInterval);
            this.setState({ listInterval: false });
        };

        this.setState({
            listInterval: setInterval(this.loadData.bind(this), 3 * 1000)
        });
    }

    loadData = function (this: User) {

        // Do not reload user while on settings page
        // Edit form uses the same model and reloading causes all changes
        // introduced by user to be reverted back.
        if (this.state.user !== undefined && this.isOnSettingsPage) {
            return;
        }

        RestApi.getUser(this.props.match.params.id).then((response) => {

            this.setState({ user: observable(response) });
        }
            // tslint:disable-next-line:no-empty
            , function (error: any) {
            });
    };

    render() {
        return (
            <div>
                <Grid>
                    <Row top="xs">
                        <Col xs={3} >
                            <Paper>
                                <Menu>
                                    <MenuItem
                                        primaryText={'Overview'}
                                        leftIcon={<Home />}
                                        onClick={() => this.props.history.push(
                                            '/user/' + this.props.match.params.id)}
                                        style={this.isOnOverviewPage ?
                                            { 'backgroundColor': grey800 } : undefined}
                                    />
                                    <MenuItem
                                        primaryText={'Settings'}
                                        leftIcon={<Sets />}
                                        onClick={() =>
                                            this.props.history.push(
                                                '/user/' + this.props.match.params.id + '/settings')}
                                        style={
                                            this.isOnSettingsPage ?
                                                { 'backgroundColor': grey800 } : undefined}
                                    />
                                </Menu>
                            </Paper>
                        </Col>
                        <Col xs={9} >
                            <Paper>
                                <Switch>
                                    <Route
                                        exact={true}
                                        path="/user/:id/settings"
                                        render={this.state.user !== undefined ? () => {

                                            return (
                                                <Settings
                                                    user={this.state.user as IUser}
                                                />
                                            );
                                        } : () => (
                                            <div>loading</div>
                                        )}
                                    />
                                    <Route
                                        exact={true}
                                        path="/user/:id"
                                        render={this.state.user !== undefined ? () => {

                                            return (
                                                <Overview
                                                    user={this.state.user as IUser}
                                                    nestedLevel={0}
                                                    value={(this.state.user as IUser).Id}
                                                    initiallyOpen={true}
                                                />
                                            );
                                        } : () => (
                                            <div>loading</div>
                                        )}
                                    />
                                </Switch>
                            </Paper>
                        </Col>
                    </Row>
                </Grid>
            </div>
        );
    }
}

export default withRouter(User);