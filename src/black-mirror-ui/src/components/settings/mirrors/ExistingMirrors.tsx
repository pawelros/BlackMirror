import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { ListItem } from 'material-ui/List';
import Code from 'material-ui/svg-icons/action/code';
import Cloud from 'material-ui/svg-icons/file/cloud';
import Mirror from '../../interfaces/Mirror';

import { observer } from 'mobx-react';

interface ExistingMirrorsProps {
    mirrors: ReadonlyArray<Mirror>;
    history?: any;
}

@observer
class ExistingMirrors extends React.Component<ExistingMirrorsProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;

    constructor(props: ExistingMirrorsProps) {
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

        const listItems = this.props.mirrors.map((mirror) => {
            const url = '/mirror/' + mirror.Id + '/sync';
            return (
                <ListItem
                    key={mirror.Id}
                    primaryText={mirror.Name}
                    leftIcon={<Cloud />}
                    initiallyOpen={false}
                    onClick={() => this.props.history.push(url)}
                    primaryTogglesNestedList={true}
                    nestedItems={[
                    ]}
                />);
        }
        );

        return (
            <ListItem
                nestedLevel={1}
                disabled={false}
                key={'mirrors'}
                primaryText={this.props.mirrors.length + ' existing mirrors'}
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

export default withRouter(ExistingMirrors);