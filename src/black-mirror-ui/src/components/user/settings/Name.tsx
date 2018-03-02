import * as React from 'react';
import TextField from 'material-ui/TextField';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import User from '../../interfaces/User';

interface NameProps {
    user: User;
}

@observer
class Name extends React.Component<NameProps, {}> {

    constructor(props: NameProps) {
        super(props);
    }

    @action
    onChange(event: any) {
        this.props.user.Name = event.target.value;
    }

    render() {
        return (
            <TextField
                floatingLabelText={'Name'}
                value={this.props.user.Name}
                // tslint:disable-next-line:jsx-no-bind
                onChange={this.onChange.bind(this)}
            />
        );
    }
}
export default Name;