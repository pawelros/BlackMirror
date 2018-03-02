import * as React from 'react';
import TextField from 'material-ui/TextField';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import Mirror from '../../interfaces/Mirror';

interface RefSpecProps {
    mirror: Mirror;
}

@observer
class RefSpec extends React.Component<RefSpecProps, {}> {

    constructor(props: RefSpecProps) {
        super(props);
    }

    @action
    onChange(event: any) {
        this.props.mirror.TargetRepositoryRefSpec = event.target.value;
    }

    render() {
        return (
            <TextField
                floatingLabelText={'Target repository refspec'}
                value={this.props.mirror.TargetRepositoryRefSpec}
                // tslint:disable-next-line:jsx-no-bind
                onChange={this.onChange.bind(this)}
            />
        );
    }
}
export default RefSpec;