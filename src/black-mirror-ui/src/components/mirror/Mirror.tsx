import * as React from 'react';
import { Switch, Route, withRouter } from 'react-router-dom';

import Paper from 'material-ui/Paper';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import Sets from 'material-ui/svg-icons/action/settings';
import Home from 'material-ui/svg-icons/action/home';
import Restore from 'material-ui/svg-icons/action/restore';
import { grey800 } from 'material-ui/styles/colors';
import RestApi from '../../actions/restApi';
import Overview from './Overview';
import SyncList from '../syncList/SyncList';
import IM from '../interfaces/Mirror';
import Synchronization from '../interfaces/Synchronization';
import Settings from './settings/Settings';
import { Grid, Row, Col } from 'react-flexbox-grid';
import { observer } from 'mobx-react';
import { observable } from 'mobx';

interface MirrorProps {
    history?: any;
    match?: any;
}

interface MirrorState {
    mirror?: IM;
    synchronizations: Synchronization[];
    listInterval: any;
}

@observer
class Mirror extends React.Component<MirrorProps, MirrorState> {

    get isOnSettingsPage() {
        return this.props.history.location.pathname.match(/^\/mirror\/\w*\/settings\/?$/);
    }

    get isOnOverviewPage() {
        return this.props.history.location.pathname.match(/^\/mirror\/\w*\/?$/);
    }

    get isOnSynchronizationsPage() {
        return this.props.history.location.pathname.match(/^\/mirror\/\w*\/sync\/?$/);
    }

    constructor(props: MirrorProps) {
        super(props);

        this.state = {
            synchronizations: [],
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

    loadData = function (this: Mirror) {

        // Do not reload mirror while on settings page
        // Edit form uses the same model and reloading causes all changes
        // introduced by user to be reverted back.
        if (this.state.mirror !== undefined && this.isOnSettingsPage) {
            return;
        }

        RestApi.getMirror(this.props.match.params.id).then((response) => {

            this.setState({ mirror: observable(response) });
            RestApi.getLastSync(response.Id).then((res) => {
                this.setState({ synchronizations: res });
            });
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
                                            '/mirror/' + this.props.match.params.id)}
                                        style={this.isOnOverviewPage ?
                                            { 'backgroundColor': grey800 } : undefined}
                                    />
                                    <MenuItem
                                        primaryText={'Synchronizations'}
                                        leftIcon={<Restore />}
                                        onClick={() =>
                                            this.props.history.push(
                                                '/mirror/' + this.props.match.params.id + '/sync')}
                                        style={this.isOnSynchronizationsPage ?
                                            { 'backgroundColor': grey800 } : undefined}
                                    />
                                    <MenuItem
                                        primaryText={'Settings'}
                                        leftIcon={<Sets />}
                                        onClick={() =>
                                            this.props.history.push(
                                                '/mirror/' + this.props.match.params.id + '/settings')}
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
                                        exact={false}
                                        path="/mirror/:id/sync"
                                        render={this.state.mirror !== undefined ? () => {

                                            return (
                                                <SyncList
                                                    mirror={this.state.mirror as IM}
                                                    synchronizations={this.state.synchronizations}
                                                />
                                            );
                                        } : () => (<div>loading</div>)}
                                    />
                                    <Route
                                        exact={true}
                                        path="/mirror/:id/settings"
                                        render={this.state.mirror !== undefined ? () => {

                                            return (
                                                <Settings
                                                    mirror={this.state.mirror as IM}
                                                />
                                            );
                                        } : () => (
                                            <div>loading</div>
                                        )}
                                    />
                                    <Route
                                        exact={true}
                                        path="/mirror/:id"
                                        render={this.state.mirror !== undefined ? () => {

                                            return (
                                                <Overview
                                                    mirror={this.state.mirror as IM}
                                                    synchronizations={this.state.synchronizations}
                                                    nestedLevel={0}
                                                    value={(this.state.mirror as IM).Id}
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

export default withRouter(Mirror);