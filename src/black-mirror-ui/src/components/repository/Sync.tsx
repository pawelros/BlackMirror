import * as React from 'react';
import Mirror from '../interfaces/Mirror';
import Synchronization from '../interfaces/Synchronization';
import { ListItem } from 'material-ui/List';
import Restore from 'material-ui/svg-icons/action/restore';
import { blue50 } from 'material-ui/styles/colors';
import { Link } from 'react-router-dom';

interface SyncProps {
    mirror: Mirror;
    syncList: Synchronization[];
    initiallyOpen: boolean;
}

class Sync extends React.Component<SyncProps, {}> {

    constructor(props: SyncProps) {
        super(props);
        this.state = {
            open: false
        };
    }

    render() {

        var x = this.props.syncList == null ? 'loading...' : this.props.syncList.length;

        return (
            <ListItem
                nestedLevel={1}
                primaryText={'Synchronizations: ' + x}
                secondaryText={''}
                leftIcon={<Restore color={blue50} />}
                initiallyOpen={this.props.initiallyOpen}
                containerElement={<Link to={'/mirror/' + this.props.mirror.Id + '/sync'} />}
                primaryTogglesNestedList={true}
            />
        );
    }
}
export default Sync;
