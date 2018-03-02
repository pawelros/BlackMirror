import * as React from 'react';
import { withRouter } from 'react-router-dom';
import Avatar from 'material-ui/Avatar';
var UnderConstruction = require('../../../images/under_construction.gif');

interface AccountProps {
    store: any;
    history?: any;
}

class Account extends React.Component<AccountProps, any> {

    render() {
        if (!this.props.store.currentUser.Exists) {
            this.props.history.push('/settings/account/new');
        }
        return (
            <div>
                <p>In this place there should be your account overview with ability to edit some settings</p>
                <p>But I have not implemented that yet :(</p>
                <p><Avatar src={UnderConstruction} size={110} /></p>
            </div>
        );
    }
}

export default withRouter(Account);
