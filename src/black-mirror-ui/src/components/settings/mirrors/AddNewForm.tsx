import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { Card, CardText, CardTitle, CardActions } from 'material-ui/Card';
import TextField from 'material-ui/TextField';
import { green500 } from 'material-ui/styles/colors';
import FloatingActionButton from 'material-ui/FloatingActionButton';
import ContentAdd from 'material-ui/svg-icons/content/add';
import SaveStatus from './SaveStatus';
import AutoComplete from 'material-ui/AutoComplete';
import NewMirrorPayload from '../../../actions/interfaces/payloads/NewMirror';
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
    text: 'Description',
    value: 'Id'
};

interface AddNewFormProps {
    payload: NewMirrorPayload;
    repositories: any;
}

@observer
class AddNewForm extends React.Component<AddNewFormProps, any> {

    constructor(props: AddNewFormProps) {
        super(props);

        this.state = {
            sourceRepositorySearchText: '',
            targetRepositorySearchText: '',
        };
    }

    handleSourceRepositoryUpdateInput = (searchText: string) => {
        this.setState({
            sourceRepositorySearchText: searchText,
        });
    }

    handleSourceRepositoryNewRequest = (chosenRequest: any) => {
        this.props.payload.SourceRepositoryId = chosenRequest.Id;
        this.setState({
            sourceRepositorySearchText: chosenRequest.Name.toString(),
        });
    }

    handleTargetRepositoryUpdateInput = (searchText: string) => {
        this.setState({
            targetRepositorySearchText: searchText,
        });
    }

    handleTargetRepositoryNewRequest = (chosenRequest: any) => {
        this.props.payload.TargetRepositoryId = chosenRequest.Id;
        this.setState({
            targetRepositorySearchText: chosenRequest.Name.toString(),
        });
    }

    onNameChange(event: any) {
        this.props.payload.Name = event.target.value;
    }

    render() {
        return (
            <Card style={styles.paper}>
                <CardTitle title={'Add new'} />
                <Card style={styles.paper}>
                    <CardText>
                        <TextField
                            floatingLabelText="Name"
                            value={this.props.payload.Name}
                            // tslint:disable-next-line:jsx-no-bind
                            onChange={this.onNameChange.bind(this)}
                        />
                        <br />
                        <AutoComplete
                            floatingLabelText="Source repository"
                            searchText={this.state.sourceRepositorySearchText}
                            onUpdateInput={this.handleSourceRepositoryUpdateInput}
                            onNewRequest={this.handleSourceRepositoryNewRequest}
                            dataSource={this.props.repositories}
                            filter={AutoComplete.fuzzyFilter}
                            openOnFocus={true}
                            dataSourceConfig={dataSourceConfig}
                        />
                        <br />
                        <AutoComplete
                            floatingLabelText="Target repository"
                            searchText={this.state.targetRepositorySearchText}
                            onUpdateInput={this.handleTargetRepositoryUpdateInput}
                            onNewRequest={this.handleTargetRepositoryNewRequest}
                            dataSource={this.props.repositories}
                            filter={AutoComplete.fuzzyFilter}
                            openOnFocus={true}
                            dataSourceConfig={dataSourceConfig}
                        />
                        <br />
                        <TextField
                            hintText="Target repository refspec (usually branch name)"
                        />
                        <br />
                    </CardText>
                    <CardActions>
                        <FloatingActionButton
                            mini={true}
                            style={styles.addButton}
                            backgroundColor={green500}
                            disabled={this.props.payload.Name === ''
                                || this.props.payload.SourceRepositoryId === ''
                                || this.props.payload.TargetRepositoryId === ''}
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