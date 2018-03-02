import * as React from 'react';

import { List, ListItem } from 'material-ui/List';
import Divider from 'material-ui/Divider';

import Repositories from './Repositories';
import User from '../User';
import Id from '../Id';
import RefSpec from '../RefSpec';
import IMirror from '../interfaces/Mirror';
import SyncStatus from './SyncStatus';
import Sync from './Sync';
import Synchronization from '../interfaces/Synchronization';
import SynchronizationStatus from '../interfaces/SynchronizationStatus';
import { observable } from 'mobx';

interface OverviewProps {
    nestedLevel: number;
    value: string;
    mirror: IMirror;
    synchronizations: Synchronization[];
    initiallyOpen: boolean;
}

class Overview extends React.Component<OverviewProps, any> {
    @observable
    lastStatus: SynchronizationStatus;
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

    componentWillReceiveProps(nextProps: OverviewProps) {
        this.lastStatus = SynchronizationStatus.Unknown;

        if (typeof (nextProps.synchronizations) !== 'undefined' &&
            typeof (nextProps.synchronizations.length) !== 'undefined' &&
            nextProps.synchronizations.length > 0) {
            this.lastStatus = nextProps.synchronizations[0].Status;
        }
    }

    render() {

        return (
            <div>
                <List>
                    <ListItem
                        key={this.props.mirror.Id}
                        primaryText={this.props.mirror.Name}
                        secondaryText={'Last sync: ' + (this.lastStatus === 0 ?
                            'Loading...' : this.lastStatus)}
                        leftIcon={<SyncStatus status={this.lastStatus} />}
                        initiallyOpen={this.props.initiallyOpen}
                        onClick={this.state.handleToggle}
                        primaryTogglesNestedList={true}
                        nestedItems={[
                            <Sync
                                mirror={this.props.mirror}
                                syncList={this.props.synchronizations}
                                key={this.props.mirror.Id + '_sync'}
                                initiallyOpen={false}
                            />,
                            <Id
                                nestedLevel={1}
                                value={this.props.mirror.Id}
                                key={this.props.mirror.Id + '_mirror_id'}
                            />,
                            <User
                                user={this.props.mirror.Owner.Name}
                                secondaryText={'Owner'}
                                nestedLevel={1}
                                key={this.props.mirror.Id + '_mirror_owner'}
                            />,
                            <Repositories
                                mirror={this.props.mirror}
                                key={this.props.mirror.Id + '_mirror_repositories'}
                            />,
                            <RefSpec
                                refspec={this.props.mirror.TargetRepositoryRefSpec}
                                nestedLevel={1}
                                key={this.props.mirror.Id + 'TargetRepositoryRefSpec'}
                            />
                        ]}
                    />
                </List>
                <Divider inset={true} />
            </div>
        );
    }
}

export default Overview;
