import React from 'react';
import { withRouter } from 'react-router-dom';
import { Card, CardText, CardTitle, CardActions } from 'material-ui/Card';
import { RadioButton, RadioButtonGroup } from 'material-ui/RadioButton';
import TextField from 'material-ui/TextField';
import { green500 } from 'material-ui/styles/colors';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import ContentAdd from 'material-ui/svg-icons/content/add';
import AutoComplete from 'material-ui/AutoComplete';
import NewRepositoryPayload from '../../../actions/interfaces/payloads/NewRepository';
import SaveStatus from './SaveStatus';

import { observer } from 'mobx-react';

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

const dataSourceConfig = {
    text: 'Name',
    value: 'Id'
};

interface AddNewFormProps {
    payload: NewRepositoryPayload;
    users: any;
}

@observer
class AddNewForm extends React.Component<AddNewFormProps, any> {

    constructor(props: AddNewFormProps) {
        super(props);

        this.state = {
            checkoutUserSearchText: '',
            pushUserSearchText: '',
        };
    }

    handleCheckoutUserUpdateInput = (searchText: string) => {
        this.setState({
            checkoutUserSearchText: searchText,
        });
    }

    handleCheckoutUserNewRequest = (chosenRequest: any) => {
        this.props.payload.CheckoutUserId = chosenRequest.Id;
        this.setState({
            checkoutUserSearchText: chosenRequest.Name.toString(),
        });
    }

    handlePushUserUpdateInput = (searchText: string) => {
        this.setState({
            pushUserSearchText: searchText,
        });
    }

    handlePushUserNewRequest = (chosenRequest: any) => {
        this.props.payload.PushUserId = chosenRequest.Id;
        this.setState({
            pushUserSearchText: chosenRequest.Name.toString(),
        });
    }

    handleFilter = (searchText: string, key: any) => {
        return key.Name.indexOf(searchText) !== -1;
    }

    onTypeChange(event: any, value: string) {
        this.props.payload.Type = value;
    }

    onNameChange(event: any) {
        this.props.payload.Name = event.target.value;
    }

    onUriChange(event: any) {
        this.props.payload.Uri = event.target.value;
    }

    onJiraChange(event: any) {
        this.props.payload.DefaultCommitMessagePrefix = event.target.value;
    }

    render() {
        return (
            <Card style={styles.paper}>
                <CardTitle title={'Add new'} />
                <Card style={styles.paper}>
                    <CardText>
                        <br />
                        <TextField
                            floatingLabelText="Name"
                            value={this.props.payload.Name}
                            // tslint:disable-next-line:jsx-no-bind
                            onChange={this.onNameChange.bind(this)}
                        />
                        <br />
                        <TextField
                            floatingLabelText="Uri"
                            value={this.props.payload.Uri}
                            // tslint:disable-next-line:jsx-no-bind
                            onChange={this.onUriChange.bind(this)}
                        />
                        <br />
                        <p>Type</p>
                        <RadioButtonGroup
                            name="repo_type"
                            valueSelected={this.props.payload.Type}
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
                            floatingLabelText="Fallback commit message prefix"
                            value={this.props.payload.DefaultCommitMessagePrefix}
                            // tslint:disable-next-line:jsx-no-bind
                            onChange={this.onJiraChange.bind(this)}
                        />
                        <br />
                        <AutoComplete
                            floatingLabelText="Checkout user"
                            searchText={this.state.checkoutUserSearchText}
                            onUpdateInput={this.handleCheckoutUserUpdateInput}
                            onNewRequest={this.handleCheckoutUserNewRequest}
                            dataSource={this.props.users}
                            filter={AutoComplete.fuzzyFilter}
                            openOnFocus={true}
                            dataSourceConfig={dataSourceConfig}
                        />
                        <br />
                        <AutoComplete
                            floatingLabelText="Fallback push user"
                            searchText={this.state.pushUserSearchText}
                            onUpdateInput={this.handlePushUserUpdateInput}
                            onNewRequest={this.handlePushUserNewRequest}
                            dataSource={this.props.users}
                            filter={AutoComplete.fuzzyFilter}
                            openOnFocus={true}
                            dataSourceConfig={dataSourceConfig}
                        />
                        <br />
                    </CardText>
                    <CardActions>
                        <FloatingActionButton
                            mini={true}
                            style={styles.addButton}
                            backgroundColor={green500}
                            disabled={this.props.payload.Name === ''
                                || this.props.payload.Type === ''
                                || this.props.payload.Uri === ''
                                || this.props.payload.DefaultCommitMessagePrefix === ''
                                || this.props.payload.CheckoutUserId === ''
                                || this.props.payload.PushUserId === ''}
                            onClick={this.props.payload.save.bind(this, this.props.payload)}
                        >
                            <ContentAdd />
                        </FloatingActionButton>
                        <SaveStatus status={this.props.payload.status} error={this.props.payload.error} />
                    </CardActions>
                </Card>
            </Card>
        );
    }
}

export default withRouter(AddNewForm);