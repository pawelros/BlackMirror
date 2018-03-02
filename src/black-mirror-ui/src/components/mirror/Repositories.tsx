import * as React from 'react';
import { ListItem } from 'material-ui/List';
import Code from 'material-ui/svg-icons/action/code';

import Repository from './Repository';
import Mirror from '../interfaces/Mirror';

interface RepositoriesProps {
    mirror: Mirror;
}

class Repositories extends React.Component<RepositoriesProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;

    constructor(props: RepositoriesProps) {
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
                nestedLevel={1}
                disabled={false}
                key={'Repositories'}
                primaryText="Repositories"
                initiallyOpen={true}
                leftIcon={<Code />}
                onNestedListToggle={this.handleNestedListToggle}
                open={this.state.open}
                onClick={this.handleToggle}
                nestedItems={[
                    <Repository
                        repository={this.props.mirror.SourceRepository}
                        secondaryText={'Source repository'}
                        key={this.props.mirror.SourceRepository.Id}
                    />,
                    <Repository
                        repository={this.props.mirror.TargetRepository}
                        secondaryText={'Target repository'}
                        key={this.props.mirror.TargetRepository.Id}
                    />
                ]}
            />
        );
    }
}
export default Repositories;
