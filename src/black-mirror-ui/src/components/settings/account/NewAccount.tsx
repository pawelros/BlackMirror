import * as React from 'react';
import { withRouter } from 'react-router-dom';

import AddAccountStepper from './AddAccountStepper';
import SaveStatus from './SaveStatus';
import { observer } from 'mobx-react';

interface NewAccountProps {
    store: any;
}

@observer
class NewAccount extends React.Component<NewAccountProps, any> {

    constructor(props: NewAccountProps) {
        super(props);
    }

    render() {
        if (this.props.store.currentUser.Exists) {
            return (
                <div>
                    {'Account \'' + this.props.store.currentUser.Id + '\' already exists.'}
                </div>);
        }
        return (
            <div>
                <AddAccountStepper store={this.props.store} />
                <SaveStatus status={this.props.store.payloads.newUser.status} />
            </div>);
    }
}

export default withRouter(NewAccount);