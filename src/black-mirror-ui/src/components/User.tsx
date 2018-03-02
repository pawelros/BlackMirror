import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Account from 'material-ui/svg-icons/action/account-circle';
import { cyan500 } from 'material-ui/styles/colors';

interface UserProps {
    nestedLevel: number;
    user: string;
    secondaryText: string;
}

class User extends React.Component<UserProps, {}> {

    render() {
        return (
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.user}
                secondaryText={this.props.secondaryText}
                leftIcon={<Account color={cyan500} />}
            />
        );
    }
}
export default User;
