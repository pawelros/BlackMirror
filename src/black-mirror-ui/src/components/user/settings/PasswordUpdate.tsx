import * as React from 'react';
import SelectField from 'material-ui/SelectField';
import MenuItem from 'material-ui/MenuItem';
import User from '../../interfaces/User';
import Credentials from '../../interfaces/Credentials';
import { TextField } from 'material-ui';
import { action } from 'mobx';

interface PasswordUpdateProps {
    user: User,
    canUpdate(): void;
    canNotUpdate(): void;
}

export default class PasswordUpdate extends React.Component<PasswordUpdateProps, any> {
    constructor(props: PasswordUpdateProps) {
        super(props);

        this.state = {
            value: '',
            canUpdate: false,
            password1: '',
            password2: '',
            passwordIsValid: false,
            passwordError: '',
            selectedIndex: 0,
            user: props.user,
        };
    }


    handleChange = (event: any, index: number, v: string) => {
        this.setState({ value: v, selectedIndex: index })
        if (v === '-') {
            this.props.canUpdate();
        }
        else {
            this.props.canNotUpdate();
        }
        this.setState({ password1: '', password2: '' })
    };

    @action
    handlePassword1Change = (event: any, value: string) => {
        this.setState({ password1: value });
        var u = this.state.user;
        var creds = u.RepositoryCredentials[this.state.selectedIndex - 1];
        creds.Password = value;

        this.setState({ user: u })
    };

    handlePassword2Change = (event: any, value: string) => {
        this.setState({ password2: value });

        if (this.state.password1 === value) {
            if (this.state.password1 === '') {
                this.setState({ passwordIsValid: false, passwordError: 'Password cannot be empty' });
                this.props.canNotUpdate();
            } else {
                this.setState({ passwordIsValid: true, passwordError: '' });
                this.props.canUpdate();
            }
        } else {
            this.setState({ passwordIsValid: false, passwordError: 'Passwords do not match' });
            this.props.canNotUpdate();
        }
    };

    render() {

        const listItems = this.props.user.RepositoryCredentials.map((c: Credentials) =>
            // tslint:disable-next-line:jsx-wrap-multiline
            <MenuItem value={c.RepositoryType + '_' + c.Login} primaryText={'[' + c.RepositoryType + '] ' + c.Login} />
        );

        listItems.unshift(<MenuItem value={'-'} primaryText={'Do not update any'} />)

        const passwordFields = this.state.value !== '-' && this.state.value !== '' ? <div>
            <TextField
                floatingLabelText="Password"
                type="password"
                value={this.state.password1}
                onChange={this.handlePassword1Change}
            /><br />
            <TextField
                floatingLabelText="Confirm password"
                type="password"
                value={this.state.password2}
                onChange={this.handlePassword2Change}
                errorText={this.state.passwordError}
            /><br />
        </div> : null

        return (
            <div>
                <SelectField
                    floatingLabelText="Update repository password"
                    value={this.state.value}
                    onChange={this.handleChange}
                >
                    {listItems}
                </SelectField>
                {passwordFields}
            </div>
        );
    }
}