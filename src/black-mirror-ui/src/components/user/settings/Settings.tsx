import * as React from 'react';
import { Card, CardActions, CardHeader, CardText } from 'material-ui/Card';
import RaisedButton from 'material-ui/FlatButton';
import { grey800, green500 } from 'material-ui/styles/colors';
import Name from './Name';
import Email from './Email';
import PasswordUpdate from './PasswordUpdate';
import { TextField } from 'material-ui';
import { observer } from 'mobx-react';
import RestApi from '../../../actions/restApi';
import Snackbar from 'material-ui/Snackbar';
import { withRouter } from 'react-router';
import IUser from '../../interfaces/User';

interface SettingsProps {
    user: IUser;
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
            snackbarMessage: '',
            canSave: true
        };
    }

    handleClick = () => {
        var self = this;
        var userId = this.props.user.Id;
        RestApi.putUser(userId, this.props.user).then((response: any) => {
            this.setState({ openSnackbar: true, snackbarMessage: 'User updated.' });
        }
            // tslint:disable-next-line:no-empty
            , function (error: any) {
                self.setState({ openSnackbar: true, snackbarMessage: 'User NOT updated. ' + error.message });
            });

    }

    handleRemovalRequest = () => {
        this.setState({ showRemovalConfirmationInput: true });
    }

    onRemovalConfirmationChange(event: any) {
        if (this.props.user && event.target.value === this.props.user.Name) {
            this.setState({ confirmRemoval: true });
        } else {
            this.setState({ confirmRemoval: false });
        }
    }

    handleRemovalConfirmation = () => {
        var self = this;
        var userId = this.props.user.Id;
        RestApi.deleteUser(userId).then((response: any) => {
            this.setState({ openSnackbar: true, snackbarMessage: 'User removed.' });
            this.props.history.push('/user');
        }
            // tslint:disable-next-line:no-empty
            , function (error: any) {
                self.setState({ openSnackbar: true, snackbarMessage: 'User NOT removed. ' + error.message });
            });

    }

    handleSnackbarRequestClose = () => {
        this.setState({
            openSnackbar: false,
        });
    }

    canSave = () => {
        this.setState({ canSave: true });
    }

    canNotSave = () => {
        this.setState({ canSave: false });
    }

    render() {

        return (
            <div>
                <Card style={{ backgroundColor: grey800 }}>
                    <CardHeader
                        title={'Edit user'}
                        actAsExpander={false}
                        style={{ backgroundColor: 'gray' }}
                    />
                    <CardText>
                        <TextField
                            floatingLabelText={'Id'}
                            value={this.props.user.Id}
                            disabled={true}
                        />
                        <br />
                        <Name
                            user={this.props.user}
                        />
                        <br />
                        <Email
                            user={this.props.user}
                        />
                        <br />
                        <br />
                        <PasswordUpdate
                            user={this.props.user}
                            canUpdate={this.canSave}
                            canNotUpdate={this.canNotSave}
                        />
                    </CardText>
                    <CardActions>
                        <RaisedButton
                            style={{ backgroundColor: green500 }}
                            label={'Save'}
                            onClick={this.handleClick}
                            disabled={!this.state.canSave}
                        />
                    </CardActions>
                </Card>
                <br />
                <Card style={{ backgroundColor: grey800 }}>
                    <CardHeader
                        title={'Remove user'}
                        actAsExpander={false}
                        style={{ backgroundColor: 'red' }}
                    />
                    <CardText>
                        <p>This will remove this user.</p>
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
                                <div>{'Type \'' + this.props.user.Name + '\' to confirm.'}</div>
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