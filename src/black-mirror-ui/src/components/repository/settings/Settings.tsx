import * as React from 'react';
import { Card, CardActions, CardHeader, CardText } from 'material-ui/Card';
import RaisedButton from 'material-ui/FlatButton';
import { grey800, green500 } from 'material-ui/styles/colors';
import ISvcRepository from '../../interfaces/SvcRepository';
import Name from './Name';
import { TextField } from 'material-ui';
import { observer } from 'mobx-react';
import RestApi from '../../../actions/restApi';
import Snackbar from 'material-ui/Snackbar';
import AdminStore from '../../../actions/adminStore';
import CheckoutUser from './CheckoutUser';
import PushUser from './PushUser';
import Uri from './Uri';
import DefaultCommitMessagePrefix from './DefaultCommitMessagePrefix';
import { withRouter } from 'react-router';

interface SettingsProps {
    repository: ISvcRepository;
    history?: any;
}

@observer
class Settings extends React.Component<SettingsProps, any> {

    constructor(props: SettingsProps) {
        super(props);

        this.state = {
            openSnackbar: false,
            showRemovalConfirmationInput: false,
            confirmRemoval: false,
            snackbarMessage: ''
        };
    }

    handleClick = () => {
        var self = this;
        var repositoryId = this.props.repository.Id;
        RestApi.putRepository(repositoryId, this.props.repository).then((response: any) => {
            this.setState({ openSnackbar: true, snackbarMessage: 'Repository updated.' });
        }
            // tslint:disable-next-line:no-empty
            , function (error: any) {
                self.setState({ openSnackbar: true, snackbarMessage: 'Repository NOT updated. ' + error.message });
            });

    }

    handleRemovalRequest = () => {
        this.setState({ showRemovalConfirmationInput: true });
    }

    onRemovalConfirmationChange(event: any) {
        if (this.props.repository && event.target.value === this.props.repository.Name) {
            this.setState({ confirmRemoval: true });
        } else {
            this.setState({ confirmRemoval: false });
        }
    }

    handleRemovalConfirmation = () => {
        var self = this;
        var repositoryId = this.props.repository.Id;
        RestApi.deleteRepository(repositoryId).then((response: any) => {
            this.setState({ openSnackbar: true, snackbarMessage: 'Repository removed.' });
            this.props.history.push('/repository');
        }
            // tslint:disable-next-line:no-empty
            , function (error: any) {
                self.setState({ openSnackbar: true, snackbarMessage: 'Repository NOT removed. ' + error.message });
            });

    }

    handleSnackbarRequestClose = () => {
        this.setState({
            openSnackbar: false,
        });
    }

    render() {

        return (
            <div>
                <Card style={{ backgroundColor: grey800 }}>
                    <CardHeader
                        title={'Edit repository'}
                        actAsExpander={false}
                        style={{ backgroundColor: 'gray' }}
                    />
                    <CardText>
                        <TextField
                            floatingLabelText={'Id'}
                            value={this.props.repository.Id}
                            disabled={true}
                        />
                        <br />
                        <Name
                            repository={this.props.repository}
                        />
                        <br />
                        <Uri
                            repository={this.props.repository}
                        />
                        <br />
                        <DefaultCommitMessagePrefix
                            repository={this.props.repository}
                        />
                        <br />
                        <CheckoutUser
                            repository={this.props.repository}
                            users={AdminStore.users}
                        />
                        <br />
                        <PushUser
                            repository={this.props.repository}
                            users={AdminStore.users}
                        />
                    </CardText>
                    <CardActions>
                        <RaisedButton style={{ backgroundColor: green500 }} label={'Save'} onClick={this.handleClick} />
                    </CardActions>
                </Card>
                <br />
                <Card style={{ backgroundColor: grey800 }}>
                    <CardHeader
                        title={'Remove repository'}
                        actAsExpander={false}
                        style={{ backgroundColor: 'red' }}
                    />
                    <CardText>
                        <p>This will remove this repository.</p>
                    </CardText>
                    <CardActions>
                        <RaisedButton
                            style={{ backgroundColor: 'red' }}
                            label="Remove"
                            onClick={this.handleRemovalRequest}
                        />
                        {this.state.showRemovalConfirmationInput ?
                            <div>
                                <br />
                                <div>{'Type \'' + this.props.repository.Name + '\' to confirm.'}</div>
                                <br />
                                <TextField
                                    // tslint:disable-next-line:jsx-no-bind
                                    onChange={this.onRemovalConfirmationChange.bind(this)}
                                />
                                <br />
                                <RaisedButton
                                    style={{ backgroundColor: 'red' }}
                                    label="Confirm Removal"
                                    disabled={!this.state.confirmRemoval}
                                    onClick={this.handleRemovalConfirmation}
                                />
                            </div> : null}
                    </CardActions>
                </Card>
                <Snackbar
                    open={this.state.openSnackbar}
                    message={this.state.snackbarMessage}
                    autoHideDuration={3000}
                    onRequestClose={this.handleSnackbarRequestClose}
                />
            </div>
        );
    }
}

export default withRouter(Settings);