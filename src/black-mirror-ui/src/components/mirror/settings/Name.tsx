import * as React from 'react';
import TextField from 'material-ui/TextField';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import Mirror from '../../interfaces/Mirror';

interface NameProps {
    mirror: Mirror;
}

// likely all TextFields components could be refactored to more generic one and reused.
// consider this 
@observer
class Name extends React.Component<NameProps, {}> {

    constructor(props: NameProps) {
        super(props);
    }

    @action
    onChange(event: any) {
        this.props.mirror.Name = event.target.value;
    }

    render() {
        return (
            <TextField
                floatingLabelText={'Name'}
                value={this.props.mirror.Name}
                // tslint:disable-next-line:jsx-no-bind
                onChange={this.onChange.bind(this)}
            />
        );
    }
}
export default Name;