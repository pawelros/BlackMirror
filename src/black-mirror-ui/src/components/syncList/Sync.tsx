import * as React from 'react';

import { List, ListItem } from 'material-ui/List';
import Divider from 'material-ui/Divider';

import * as moment from 'moment';

import Id from '../Id';
import DateTime from '../DateTime';
import Reflections from './Reflections';
import SyncStatus from '../mirror/SyncStatus';
import Synchronization from '../interfaces/Synchronization';
import Logs from './Logs';

interface SyncProps {
    sync: Synchronization;
}

class Sync extends React.Component<SyncProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;
    constructor(props: SyncProps) {
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

        var dateTime = moment(this.props.sync.CreationTime);

        return (
            <div>
                <List>
                    <ListItem
                        key={this.props.sync.Id}
                        primaryText={dateTime.format('DD-MM-YYYY HH:mm')}
                        secondaryText={this.props.sync.Status}
                        leftIcon={<SyncStatus status={this.props.sync.Status} />}
                        initiallyOpen={false}
                        onClick={this.handleToggle}
                        primaryTogglesNestedList={true}
                        nestedItems={[
                            <Id nestedLevel={1} value={this.props.sync.Id} key={this.props.sync.Id + '_sync_id'} />,
                            <DateTime
                                value={dateTime.format('DD-MM-YYYY HH:mm')}
                                text={''}
                                key={this.props.sync.CreationTime}
                                nestedLevel={1}
                            />,
                            <Logs sync={this.props.sync} key={this.props.sync.Id + '_sync_logs'} />,
                            <Reflections syncId={this.props.sync.Id} key={this.props.sync.Id + '_sync_reflections'} />
                        ]}
                    />
                </List>
                <Divider inset={true} />
            </div>
        );
    }
}
export default Sync;
