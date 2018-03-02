import * as React from 'react';

import { ListItem } from 'material-ui/List';
import LockOpen from 'material-ui/svg-icons/action/lock-open';
import Credentials from '../interfaces/Credentials';
import { green500 } from 'material-ui/styles/colors';
import ISvcRepositoryType from '../interfaces/SvcRepositoryType';
import GitIcon from '../../images/svg-icons/git';
import SvnIcon from '../../images/svg-icons/svn';
import Account from 'material-ui/svg-icons/action/account-circle';

interface RepositoryCredentialsItemProps {
    credentials: Credentials;
    secondaryText: string;
    initiallyOpen?: boolean;
    nestedLevel: number;
    key?: string;
}

class RepositoryCredentials extends React.Component<RepositoryCredentialsItemProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;

    constructor(props: RepositoryCredentialsItemProps) {
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
            [
                // tslint:disable-next-line:jsx-wrap-multiline

                // tslint:disable-next-line:jsx-wrap-multiline
                <ListItem
                    key={'y'}
                    nestedLevel={this.props.nestedLevel}
                    primaryText={this.props.credentials.RepositoryType}
                    secondaryText={'Repository type'}
                    leftIcon={
                        // tslint:disable-next-line:max-line-length
                        this.props.credentials.RepositoryType.toString() === ISvcRepositoryType[ISvcRepositoryType.git] ?
                            <GitIcon /> : <SvnIcon />}
                    onNestedListToggle={this.handleNestedListToggle}
                    onClick={this.handleToggle}
                    primaryTogglesNestedList={true}
                    initiallyOpen={this.props.initiallyOpen}
                    nestedItems={[<ListItem
                        key={'x'}
                        nestedLevel={this.props.nestedLevel + 1}
                        primaryText={this.props.credentials.Login}
                        secondaryText={'Login'}
                        leftIcon={<Account />}
                        onNestedListToggle={this.handleNestedListToggle}
                        onClick={this.handleToggle}
                        primaryTogglesNestedList={true}
                        initiallyOpen={this.props.initiallyOpen}
                    />, <ListItem
                        key={'z'}
                        nestedLevel={this.props.nestedLevel + 1}
                        primaryText={this.props.credentials.AllowedRepositories.join(',')}
                        secondaryText={'Allowed repositories'}
                        leftIcon={<LockOpen color={green500} />}
                        onNestedListToggle={this.handleNestedListToggle}
                        onClick={this.handleToggle}
                        primaryTogglesNestedList={true}
                        initiallyOpen={this.props.initiallyOpen}
                    />]}
                />
            ]
        );
    }
}
export default RepositoryCredentials;
