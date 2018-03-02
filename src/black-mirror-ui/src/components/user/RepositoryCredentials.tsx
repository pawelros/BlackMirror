import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Key from 'material-ui/svg-icons/action/lock';
import User from '../interfaces/User';
import Credentials from '../interfaces/Credentials';
import { green500 } from 'material-ui/styles/colors';
import Item from './RepositoryCredentialsItem';

interface RepositoryCredentialsProps {
    user: User;
    secondaryText: string;
    initiallyOpen?: boolean;
    nestedLevel: number;
    key?: string;
}

class RepositoryCredentials extends React.Component<RepositoryCredentialsProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;

    constructor(props: RepositoryCredentialsProps) {
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

        const listItems = this.props.user.RepositoryCredentials.map((c: Credentials) =>
            // tslint:disable-next-line:jsx-wrap-multiline
            <Item
                credentials={c}
                key={'credentials_' + c.Login}
                secondaryText={''}
                nestedLevel={4}
                initiallyOpen={true}
            />
        );

        return (
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={'Repository credentials'}
                secondaryText={this.props.secondaryText}
                leftIcon={<Key color={green500} />}
                onNestedListToggle={this.handleNestedListToggle}
                onClick={this.handleToggle}
                primaryTogglesNestedList={true}
                initiallyOpen={this.props.initiallyOpen}
                nestedItems={listItems}
            />
        );
    }
}
export default RepositoryCredentials;
