import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { Card, CardTitle, CardText, CardActions } from 'material-ui/Card';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import ContentAdd from 'material-ui/svg-icons/content/add';

import TextField from 'material-ui/TextField';
import { green500 } from 'material-ui/styles/colors';

import { RadioButton, RadioButtonGroup } from 'material-ui/RadioButton';
import { ListItem } from 'material-ui/List';
import Remove from 'material-ui/svg-icons/content/remove';
import { red500 } from 'material-ui/styles/colors';

const styles = {
    paper: {
        padding: 15
    },
    radioButton: {
        marginBottom: 16,
    },
    addButton: {
        marginRight: 20,

    }
};

import { observer } from 'mobx-react';

interface RepositoryCredentialsProps {
    credentials: any;
}

@observer
class RepositoryCredentials extends React.Component<RepositoryCredentialsProps, any> {

    constructor(props: RepositoryCredentialsProps) {
        super(props);
    }

    onTypeChange(event: any, value: any) {
        this.props.credentials.newType = value;
    }

    onLoginChange(event: any) {
        this.props.credentials.newLogin = event.target.value;
    }

    onPasswordChange(event: any) {
        this.props.credentials.newPassword = event.target.value;
    }

    onAdd() {
        this.props.credentials.add();
    }

    onRemove(event: any) {
        this.props.credentials.delete(event);
    }

    render() {

        const listItems = this.props.credentials.existing.map((creds: any, index: any) =>
            // tslint:disable-next-line:jsx-wrap-multiline
            <ListItem
                key={index}
                primaryText={creds.Login}
                secondaryText={creds.Type}
                leftIcon={
                    <Remove
                        color={red500}
                        onClick={this.onRemove.bind(this, creds)}
                    />
                }
            />
        );

        return (
            <div>
                <Card>
                    <CardTitle title={'Repository Credentials'} subtitle={''} />
                    <CardText>
                        <div>
                            <Card style={styles.paper}>
                                <CardTitle subtitle={this.props.credentials.existing.length + ' existing'} />
                                <CardText>
                                    {listItems}
                                </CardText>
                            </Card>
                            <br />
                            <Card style={styles.paper}>
                                <CardTitle subtitle={'Add new'} />
                                <CardText>
                                    <RadioButtonGroup
                                        name="repo_type"
                                        valueSelected={this.props.credentials.newType}
                                        style={styles.radioButton}
                                        // tslint:disable-next-line:jsx-no-bind
                                        onChange={this.onTypeChange.bind(this)}
                                    >
                                        <RadioButton
                                            value="svn"
                                            label="svn"
                                            style={styles.radioButton}
                                        />
                                        <RadioButton
                                            value="git"
                                            label="git"
                                            style={styles.radioButton}
                                        />
                                    </RadioButtonGroup>
                                    <TextField
                                        disabled={false}
                                        value={this.props.credentials.newLogin}
                                        floatingLabelText={'Login'}
                                        // tslint:disable-next-line:jsx-no-bind
                                        onChange={this.onLoginChange.bind(this)}
                                    />
                                    <TextField
                                        disabled={false}
                                        value={this.props.credentials.newPassword}
                                        floatingLabelText="Password"
                                        type="password"
                                        // tslint:disable-next-line:jsx-no-bind
                                        onChange={this.onPasswordChange.bind(this)}
                                    />

                                </CardText>
                                <CardActions>
                                    <FloatingActionButton
                                        mini={true}
                                        style={styles.addButton}
                                        backgroundColor={green500}
                                        // tslint:disable-next-line:jsx-no-bind
                                        onClick={this.onAdd.bind(this)}
                                        disabled={this.props.credentials.newLogin === ''
                                            || this.props.credentials.newPassword === ''}
                                    >
                                        <ContentAdd />
                                    </FloatingActionButton>
                                </CardActions>
                            </Card>
                        </div>
                    </CardText>
                </Card>
            </div>
        );
    }
}

export default withRouter(RepositoryCredentials);