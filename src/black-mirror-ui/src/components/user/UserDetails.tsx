import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Id from '../Id';
import Email from './Email';
import Account from 'material-ui/svg-icons/action/account-circle';
import IUser from '../interfaces/User';
import RepositoryCredentials from './RepositoryCredentials';

interface UserProps {
    user: IUser;
    secondaryText: string;
    initiallyOpen?: boolean;
    key?: string;
}

class User extends React.Component<UserProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;

    constructor(props: UserProps) {
        super(props);

        this.state = {
            open: false
        };

        this.handleToggle = () => {
            this.setState({
                open: !this.state.open,
            });
        };

        this.handleNestedListToggle = (item) => {
            this.setState({
                open: item.state.open,
            });
        };
    }

    render() {
        return (
            <ListItem
                nestedLevel={2}
                primaryText={this.props.user.Name}
                secondaryText={this.props.secondaryText}
                leftIcon={<Account />}
                onNestedListToggle={this.handleNestedListToggle}
                onClick={this.handleToggle}
                primaryTogglesNestedList={true}
                initiallyOpen={this.props.initiallyOpen}
                nestedItems={[
                    <Id
                        value={this.props.user.Id}
                        key={this.props.user.Id + 'user_id'}
                        nestedLevel={1}
                    />,
                    <Email
                        value={this.props.user.Email}
                        key={this.props.user.Email + 'user_email'}
                        nestedLevel={1}
                    />,
                    <RepositoryCredentials
                        user={this.props.user}
                        key={'key?' + this.props.user.Id}
                        secondaryText={''}
                        initiallyOpen={true}
                        nestedLevel={3}
                    />
                ]}
            />
        );
    }
}
export default User;
