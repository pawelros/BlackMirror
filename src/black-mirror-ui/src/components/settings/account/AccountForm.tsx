import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { Card, CardTitle, CardText } from 'material-ui/Card';
import TextField from 'material-ui/TextField';

const focusUsernameInputField = (input: any) => {
    if (input) {
        input.focus();
    }
};

import { observer } from 'mobx-react';

interface AccountFormProps {
    account: any;
    exists: boolean;
}

@observer
class AccountForm extends React.Component<AccountFormProps, any> {

    constructor(props: AccountFormProps) {
        super(props);

        this.state = {
            validationErrors: {
                email: ''
            }
        };
    }

    validateEmail(email: string) {
        // tslint:disable-next-line:max-line-length
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    onEmailChange(event: any) {
        this.props.account.Email = event.target.Email;
        if (this.validateEmail(event.target.value)) {
            this.setState({ validationErrors: { email: '' } });
        } else {
            this.setState({ validationErrors: { email: 'Invalid email' } });
        }
    }

    onNameChange(event: any) {
        this.props.account.Name = event.target.value;
    }

    render() {
        if (this.props.exists === null) {
            return (
                <div />
            );
        } else if (!this.props.exists) {
            return (
                <div>
                    <Card>
                        <CardTitle title={'New Account'} subtitle={''} />
                        <CardText>
                            <div>
                                <div className="">
                                    <TextField
                                        disabled={true}
                                        hintText="Automaticaly generated ID. Cannot be changed."
                                        value={this.props.account.Id}
                                        floatingLabelText="ID"
                                    />
                                    <br />
                                    <TextField
                                        disabled={false}
                                        value={this.props.account.Name}
                                        floatingLabelText="Name"
                                        ref={focusUsernameInputField}
                                        // tslint:disable-next-line:jsx-no-bind
                                        onChange={this.onNameChange.bind(this)}
                                    />
                                    <br />
                                    <TextField
                                        disabled={false}
                                        hintText="Email"
                                        errorText={this.state.validationErrors.email}
                                        value={this.props.account.Email}
                                        floatingLabelText="Email"
                                        // tslint:disable-next-line:jsx-no-bind
                                        onChange={this.onEmailChange.bind(this)}
                                    />
                                </div>
                            </div>
                        </CardText>
                    </Card>
                </div>
            );
        } else {
            return (
                <div>User {this.props.account.Name} already exists.</div>
            );
        }
    }
}

export default withRouter(AccountForm);
