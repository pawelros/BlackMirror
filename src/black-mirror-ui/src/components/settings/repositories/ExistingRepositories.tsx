import * as React from 'react';
import { withRouter } from 'react-router-dom';
import Repository from '../../mirror/Repository';
import { ListItem } from 'material-ui/List';
import Code from 'material-ui/svg-icons/action/code';
import SvcRepository from '../../interfaces/SvcRepository';
import { observer } from 'mobx-react';

interface ExistingRepositoriesProps {
    repositories: SvcRepository[];
}

@observer
class ExistingRepositories extends React.Component<ExistingRepositoriesProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;

    constructor(props: ExistingRepositoriesProps) {
        super(props);

        this.state = {
            open: true
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
        const listItems = this.props.repositories.map((repo: SvcRepository) =>
            // tslint:disable-next-line:jsx-wrap-multiline
            <Repository
                repository={repo}
                secondaryText={repo.Type.toString()}
                key={repo.Id}
            />
        );

        return (
            <ListItem
                nestedLevel={1}
                disabled={false}
                key={'repositories'}
                primaryText={this.props.repositories.length + ' existing repositories'}
                initiallyOpen={true}
                leftIcon={<Code />}
                onNestedListToggle={this.handleNestedListToggle}
                open={this.state.open}
                onClick={this.handleToggle}
                nestedItems={listItems}
            />
        );
    }
}

export default withRouter(ExistingRepositories);