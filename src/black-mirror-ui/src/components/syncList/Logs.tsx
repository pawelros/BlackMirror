import React from 'react';
import { ListItem } from 'material-ui/List';
import Receipt from 'material-ui/svg-icons/action/receipt';
import { grey400 } from 'material-ui/styles/colors';
import Sync from '../interfaces/Synchronization';

interface LogsProps {
    sync: Sync;
}

class Logs extends React.Component<LogsProps, {}> {
    constructor(props: LogsProps) {
        super(props);

        this.state = {
            open: false
        };
    }

    render() {

        return (
            <ListItem
                nestedLevel={1}
                primaryText={'Logs'}
                secondaryText={''}
                leftIcon={<Receipt color={grey400} />}
                initiallyOpen={false}
                containerElement={<a href={'/sync/' + this.props.sync.Id + '/logs'} />}
                primaryTogglesNestedList={true}
            />
        );
    }
}
export default Logs;
