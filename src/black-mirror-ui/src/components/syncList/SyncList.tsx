import * as React from 'react';
import { Card, CardTitle, CardText } from 'material-ui/Card';
import Sync from './Sync';
import AddSyncButton from './AddSyncButton';
import Mirror from '../interfaces/Mirror';
import Synchronization from '../interfaces/Synchronization';

interface SyncListProps {
    mirror: Mirror;
    synchronizations: Synchronization[];
}

class SyncList extends React.Component<SyncListProps, {}> {

    render() {
        const listItems = this.props.synchronizations.map((s: Synchronization) =>
            <Sync sync={s} key={s.Id} />
        );

        return (
            <Card>
                <CardTitle
                    // tslint:disable-next-line:jsx-alignment
                    title={this.props.mirror.Name ? this.props.mirror.Name : 'Loading' + ' synchronizations'}
                    // tslint:disable-next-line:jsx-alignment
                    subtitle={'Total: ' + (this.props.synchronizations ? this.props.synchronizations.length : 0)} />
                <CardText>
                    <div>
                        <AddSyncButton
                            mirrorId={this.props.mirror.Id}
                        />
                        <div className="">
                            {listItems}
                        </div>
                    </div>
                </CardText>
            </Card>
        );
    }
}
export default SyncList;
