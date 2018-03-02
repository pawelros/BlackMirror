import * as React from 'react';

import { Card, CardActions, CardHeader, CardText } from 'material-ui/Card';
import RaisedButton from 'material-ui/FlatButton';
import { grey800, green500 } from 'material-ui/styles/colors';
import Mirror from '../../interfaces/Mirror';
import RefSpec from './RefSpec';
import Name from './Name';
import { TextField } from 'material-ui';
import { observer } from 'mobx-react';
import RestApi from '../../../actions/restApi';
import Snackbar from 'material-ui/Snackbar';
import AdminStore from '../../../actions/adminStore';
import SourceRepository from './SourceRepository';
import TargetRepository from './TargetRepository';
import Owner from './Owner';
import { withRouter } from 'react-router';

interface SettingsProps {
    mirror: Mirror;
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
        var mirrorId = this.props.mirror.Id;
        RestApi.putMirror(mirrorId, this.props.mirror).then((response: any) => {
            this.setState({ openSnackbar: true, snackbarMessage: 'Mirror updated.' });
        }
            // tslint:disable-next-line:no-empty
            , function (error: any) {
                self.setState({ openSnackbar: true, snackbarMessage: 'Mirror NOT updated. ' + error.message });
            });

    }

    handleRemovalRequest = () => {
        this.setState({ showRemovalConfirmationInput: true });
    }

    onRemovalConfirmationChange(event: any) {
        if (this.props.mirror && event.target.value === this.props.mirror.Name) {
            this.setState({ confirmRemoval: true });
        } else {
            this.setState({ confirmRemoval: false });
        }
    }

    handleRemovalConfirmation = () => {
        var self = this;
        var mirrorId = this.props.mirror.Id;
        RestApi.deleteMirror(mirrorId).then((response: any) => {
            this.setState({ openSnackbar: true, snackbarMessage: 'Mirror removed.' });
            this.props.history.push('/');
        }
            // tslint:disable-next-line:no-empty
            , function (error: any) {
                self.setState({ openSnackbar: true, snackbarMessage: 'Mirror NOT removed. ' + error.message });
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
                        title={'Edit mirror'}
                        actAsExpander={false}
                        style={{ backgroundColor: 'gray' }}
                    />
                    <CardText>
                        <TextField
                            floatingLabelText={'Id'}
                            value={this.props.mirror.Id}
                            disabled={true}
                        />
                        <br />
                        <Name
                            mirror={this.props.mirror}
                        />
                        <br />
                        <RefSpec
                            mirror={this.props.mirror}
                        />
                        <br />
                        <SourceRepository
                            mirror={this.props.mirror}
                            repositories={AdminStore.repositories}
                        />
                        <br />
                        <TargetRepository
                            mirror={this.props.mirror}
                            repositories={AdminStore.repositories}
                        />
                        <br />
                        <Owner
                            mirror={this.props.mirror}
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
                        title={'Remove mirror'}
                        actAsExpander={false}
                        style={{ backgroundColor: 'red' }}
                    />
                    <CardText>
                        <p>This will remove this mirror and all synchronizations related with it.</p>
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
                                <div>{'Type \'' + this.props.mirror.Name + '\' to confirm.'}</div>
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