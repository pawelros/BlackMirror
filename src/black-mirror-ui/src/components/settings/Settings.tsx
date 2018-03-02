import * as React from 'react';
import { Switch, Route, withRouter } from 'react-router-dom';
import Account from './account/Account';
import NewAccount from './account/NewAccount';
import Default from './Default';
import Repositories from './repositories/Repositories';
import Mirrors from './mirrors/Mirrors';
import AdminStore from '../../actions/adminStore';

import Paper from 'material-ui/Paper';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import PersonAdd from 'material-ui/svg-icons/social/person-add';
import Cloud from 'material-ui/svg-icons/file/cloud';
import Divider from 'material-ui/Divider';
import Sets from 'material-ui/svg-icons/action/settings';
import Code from 'material-ui/svg-icons/action/code';
import { grey800 } from 'material-ui/styles/colors';
import { Grid, Row, Col } from 'react-flexbox-grid';

interface SettingsProps {
    history?: any;
}

class Settings extends React.Component<SettingsProps, any> {

    render() {
        return (
            <div>
                <Grid>
                    <Row top="xs">
                        <Col xs={3}>
                            <Paper>
                                <Menu>
                                    <MenuItem
                                        primaryText={'Overview'}
                                        leftIcon={<Sets />}
                                        onClick={() => this.props.history.push('/settings/')}
                                        style={this.props.history.location.pathname.match(/^\/settings\/?$/) ?
                                            { 'backgroundColor': grey800 } : undefined}
                                    />
                                    <Divider />
                                    <MenuItem
                                        primaryText={'Account'}
                                        leftIcon={<PersonAdd />}
                                        onClick={() => this.props.history.push('/settings/account')}
                                        style={this.props.history.location.pathname.match(/^\/settings\/account.*$/) ?
                                            { 'backgroundColor': grey800 } : undefined}
                                    />
                                    <MenuItem
                                        primaryText={'Repositories'}
                                        leftIcon={<Code />}
                                        onClick={() => this.props.history.push('/settings/repositories')}
                                        style={
                                            this.props.history.location.pathname.match(/^\/settings\/repositories.*$/) ?
                                                { 'backgroundColor': grey800 } : undefined}
                                    />
                                    <MenuItem
                                        primaryText={'Mirrors'}
                                        leftIcon={<Cloud />}
                                        onClick={() => this.props.history.push('/settings/mirrors')}
                                        style={this.props.history.location.pathname.match(/^\/settings\/mirrors.*$/) ?
                                            { 'backgroundColor': grey800 } : undefined}
                                    />
                                    {/* <Divider /> */}
                                    {/* <MenuItem primaryText="Download" leftIcon={<Download />} />
                      
                        <MenuItem primaryText="Remove" leftIcon={<Delete />} /> */}
                                </Menu>
                            </Paper>
                        </Col>
                        <Col xs={9}>
                            <Paper>
                                <Switch>
                                    <Route
                                        exact={true}
                                        path="/settings/"
                                        render={() => (
                                            <Default user={AdminStore.currentUser} />
                                        )}
                                    />
                                    <Route
                                        exact={true}
                                        path="/settings/account"
                                        render={() => (
                                            <Account store={AdminStore} />)}
                                    />
                                    <Route
                                        exact={true}
                                        path="/settings/account/new"
                                        render={() => (
                                            <NewAccount
                                                store={AdminStore}
                                            />)}
                                    />
                                    <Route
                                        exact={true}
                                        path="/settings/repositories/"
                                        render={() => (
                                            <Repositories
                                                repositories={AdminStore.repositories}
                                                payload={AdminStore.payloads.newRepository}
                                                users={AdminStore.users}
                                            />
                                        )}
                                    />
                                    <Route
                                        exact={true}
                                        path="/settings/mirrors/"
                                        render={() => (
                                            <Mirrors
                                                mirrors={AdminStore.mirrors}
                                                repositories={AdminStore.repositories}
                                                payload={AdminStore.payloads.newMirror}
                                            />
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

export default withRouter(Settings);