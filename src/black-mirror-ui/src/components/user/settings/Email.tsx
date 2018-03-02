import * as React from 'react';
import TextField from 'material-ui/TextField';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import User from '../../interfaces/User';

interface EmailProps {
    user: User;
}

@observer
class Email extends React.Component<EmailProps, {}> {

    constructor(props: EmailProps) {
        super(props);
    }

    @action
    onChange(event: any) {
        this.props.user.Email = event.target.value;
    }

    render() {
        return (
            <TextField
                floatingLabelText={'Email'}
                value={this.props.user.Email}
                // tslint:disable-next-line:jsx-no-bind
                onChange={this.onChange.bind(this)}
            />
        );
    }
}
export default Email;