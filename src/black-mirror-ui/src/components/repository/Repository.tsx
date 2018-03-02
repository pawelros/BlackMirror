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

import ISvcRepository from '../interfaces/SvcRepository';

interface RepositoryProps {
    history?: any;
    match?: any;
}

interface RepositoryState {
    repository?: ISvcRepository;
    listInterval: any;
}

@observer
class Repository extends React.Component<RepositoryProps, RepositoryState> {

    get isOnSettingsPage() {
        return this.props.history.location.pathname.match(/^\/repository\/\w*\/settings\/?$/);
    }

    get isOnOverviewPage() {
        return this.props.history.location.pathname.match(/^\/repository\/\w*\/?$/);
    }

    constructor(props: RepositoryProps) {
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

    loadData = function (this: Repository) {

        // Do not reload repository while on settings page
        // Edit form uses the same model and reloading causes all changes
        // introduced by user to be reverted back.
        if (this.state.repository !== undefined && this.isOnSettingsPage) {
            return;
        }

        RestApi.getRepository(this.props.match.params.id).then((response) => {

            this.setState({ repository: observable(response) });
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
                                            '/repository/' + this.props.match.params.id)}
                                        style={this.isOnOverviewPage ?
                                            { 'backgroundColor': grey800 } : undefined}
                                    />
                                    <MenuItem
                                        primaryText={'Settings'}
                                        leftIcon={<Sets />}
                                        onClick={() =>
                                            this.props.history.push(
                                                '/repository/' + this.props.match.params.id + '/settings')}
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
                                        path="/repository/:id/settings"
                                        render={this.state.repository !== undefined ? () => {

                                            return (
                                                <Settings
                                                    repository={this.state.repository as ISvcRepository}
                                                />
                                            );
                                        } : () => (
                                            <div>loading</div>
                                        )}
                                    />
                                    <Route
                                        exact={true}
                                        path="/repository/:id"
                                        render={this.state.repository !== undefined ? () => {

                                            return (
                                                <Overview
                                                    repository={this.state.repository as ISvcRepository}
                                                    nestedLevel={0}
                                                    value={(this.state.repository as ISvcRepository).Id}
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

export default withRouter(Repository);