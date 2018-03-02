import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { List, ListItem } from 'material-ui/List';
import Divider from 'material-ui/Divider';
import User from '../interfaces/User';
import Account from 'material-ui/svg-icons/action/account-circle';

interface UserFlatProps {
    nestedLevel: number;
    value: string;
    user: User;
    initiallyOpen: boolean;
    history?: any;
}

class UserFlat extends React.Component<UserFlatProps, any> {

    render() {
        return (
            <div>
                <List>
                    <ListItem
                        key={this.props.user.Id}
                        primaryText={this.props.user.Name}
                        secondaryText={this.props.user.Id}
                        primaryTogglesNestedList={true}
                        leftIcon={<Account />}
                        onClick={() => this.props.history.push('/user/' + this.props.user.Id)}
                    />
                </List>
                <Divider inset={true} />
            </div>
        );
    }
}

export default withRouter(UserFlat);
