import * as React from 'react';
import TextField from 'material-ui/TextField';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import SvcRepository from '../../interfaces/SvcRepository';

interface DefaultCommitMessagePrefixProps {
    repository: SvcRepository;
}

@observer
class DefaultCommitMessagePrefix extends React.Component<DefaultCommitMessagePrefixProps, {}> {

    constructor(props: DefaultCommitMessagePrefixProps) {
        super(props);
    }

    @action
    onChange(event: any) {
        this.props.repository.DefaultCommitMessagePrefix = event.target.value;
    }

    render() {
        return (
            <TextField
                floatingLabelText={'Default commit message prefix'}
                value={this.props.repository.DefaultCommitMessagePrefix}
                // tslint:disable-next-line:jsx-no-bind
                onChange={this.onChange.bind(this)}
            />
        );
    }
}
export default DefaultCommitMessagePrefix;