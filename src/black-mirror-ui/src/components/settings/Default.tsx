import * as React from 'react';
import { withRouter } from 'react-router-dom';
import Avatar from 'material-ui/Avatar';

import CreateAccountButton from './account/CreateAccountButton';

var UnderConstruction = require('../../images/under_construction.gif');
import { observer } from 'mobx-react';

interface DefaultProps {
    user: any;
}

@observer
class Default extends React.Component<DefaultProps, any> {

    render() {
        if (!this.props.user.Exists) {
            return (

                <div>
                    <p>{'Hi '
                        + this.props.user.Name
                        + '. It looks that you have not created account yet. Nothing to do here without an account.'}
                    </p>
                    <CreateAccountButton />
                </div>
            );
        } else {
            return (
                <div>
                    <p>{'Hi ' + this.props.user.Name}</p>
                    <div>
                        <p>In this place there should be your settings overview / landing page.</p>
                        <p>But I have not implemented that yet :(</p>
                        <p><Avatar src={UnderConstruction} size={110} /></p>
                    </div>
                </div>
            );
        }
    }
}

export default withRouter(Default);
