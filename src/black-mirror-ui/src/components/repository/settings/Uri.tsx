import * as React from 'react';
import TextField from 'material-ui/TextField';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import SvcRepository from '../../interfaces/SvcRepository';

interface UriProps {
    repository: SvcRepository;
}

@observer
class Uri extends React.Component<UriProps, {}> {

    constructor(props: UriProps) {
        super(props);
    }

    @action
    onChange(event: any) {
        this.props.repository.Uri = event.target.value;
    }

    render() {
        return (
            <TextField
                floatingLabelText={'Uri'}
                value={this.props.repository.Uri}
                // tslint:disable-next-line:jsx-no-bind
                onChange={this.onChange.bind(this)}
            />
        );
    }
}
export default Uri;