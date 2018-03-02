import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { List, ListItem } from 'material-ui/List';
import Divider from 'material-ui/Divider';

import IMirror from '../interfaces/Mirror';
import SyncStatus from '../mirror/SyncStatus';
import Synchronization from '../interfaces/Synchronization';
import SynchronizationStatus from '../interfaces/SynchronizationStatus';
import { observable } from 'mobx';

interface MirrorFlatProps {
    nestedLevel: number;
    value: string;
    mirror: IMirror;
    synchronizations: Synchronization[];
    initiallyOpen: boolean;
    history?: any;
}

class MirrorFlat extends React.Component<MirrorFlatProps, any> {
    @observable
    lastStatus: SynchronizationStatus;

    componentWillReceiveProps(nextProps: MirrorFlatProps) {
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
                        primaryTogglesNestedList={true}
                        onClick={() => this.props.history.push('/mirror/' + this.props.mirror.Id)}
                    />
                </List>
                <Divider inset={true} />
            </div>
        );
    }
}

export default withRouter(MirrorFlat);
