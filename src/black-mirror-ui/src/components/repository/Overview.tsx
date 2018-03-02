import * as React from 'react';

import { List } from 'material-ui/List';
import Divider from 'material-ui/Divider';

import ISvcRepository from '../interfaces/SvcRepository';
// probably this component should be located somewhere else
import Repository from '../mirror/Repository';

interface OverviewProps {
    nestedLevel: number;
    value: string;
    repository: ISvcRepository;
    initiallyOpen: boolean;
}

class Overview extends React.Component<OverviewProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;

    constructor(props: OverviewProps) {
        super(props);
        this.state = {
            open: false,
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
            <div>
                <List>
                    <Repository
                        repository={this.props.repository}
                        key={'co_za_key' + this.props.repository.Id}
                        secondaryText={this.props.repository.Type.toString()}
                        initiallyOpen={true}
                    />
                </List>
                <Divider inset={true} />
            </div>
        );
    }
}

export default Overview;
