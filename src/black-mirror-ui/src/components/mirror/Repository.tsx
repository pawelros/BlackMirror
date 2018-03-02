import * as React from 'react';

import { ListItem } from 'material-ui/List';
import User from '../User';
import Id from '../Id';
import Uri from './Uri';
import DefaultCommitMessagePrefix from './DefaultCommitMessagePrefix';
import SvcRepository from '../interfaces/SvcRepository';
import ISvcRepositoryType from '../interfaces/SvcRepositoryType';
import GitIcon from '../../images/svg-icons/git';
import SvnIcon from '../../images/svg-icons/svn';

interface RepositoryProps {
    repository: SvcRepository;
    secondaryText: string;
    initiallyOpen?: boolean;
    key?: string;
}

class Repository extends React.Component<RepositoryProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;

    constructor(props: RepositoryProps) {
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
                primaryText={this.props.repository.Name}
                secondaryText={this.props.secondaryText}
                leftIcon={this.props.repository.Type.toString() === ISvcRepositoryType[ISvcRepositoryType.git] ?
                    <GitIcon /> : <SvnIcon />}
                onNestedListToggle={this.handleNestedListToggle}
                onClick={this.handleToggle}
                primaryTogglesNestedList={true}
                initiallyOpen={this.props.initiallyOpen}
                nestedItems={[
                    <Id
                        value={this.props.repository.Id}
                        key={this.props.repository.Id}
                        nestedLevel={1}
                    />,
                    <Uri
                        value={this.props.repository.Uri}
                        key={this.props.repository.Uri}
                        nestedLevel={1}
                    />,
                    <DefaultCommitMessagePrefix
                        value={this.props.repository.DefaultCommitMessagePrefix}
                        key={this.props.repository.DefaultCommitMessagePrefix + this.props.repository.Id}
                        nestedLevel={1}
                    />,
                    <User
                        user={this.props.repository.CheckoutUser.Id}
                        secondaryText={'Checkout user'}
                        nestedLevel={1}
                        key={this.props.repository.CheckoutUser.Id}
                    />,
                    <User
                        user={this.props.repository.PushUser.Id}
                        secondaryText={'Default push user'}
                        nestedLevel={1}
                        key={this.props.repository.PushUser.Id}
                    />,
                ]}
            />
        );
    }
}
export default Repository;
