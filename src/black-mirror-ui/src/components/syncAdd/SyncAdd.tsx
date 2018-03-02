import * as React from 'react';
import { Card, CardTitle, CardText } from 'material-ui/Card';
import MirrorAutoComplete from './MirrorAutoComplete';
import SyncSchedule from './SyncSchedule';
import AddButton from './AddButton';
import Mirror from '../interfaces/Mirror';

interface SyncAddState {
    selectedMirror?: Mirror;
}

class SyncAdd extends React.Component<any, SyncAddState> {
    selectMirror: (mirror: any) => void;
    constructor(props: any) {
        super(props);

        this.state = {
            selectedMirror: undefined
        };

        this.selectMirror = (mirror) => {
            this.setState({ selectedMirror: mirror });
        };
    }

    render() {

        return (
            <Card>
                <CardTitle title={'Add Sync'} subtitle={''} />
                <CardText>
                    <div>
                        <div className="">
                            <MirrorAutoComplete selectMirror={this.selectMirror} />
                            <br />
                            <p>Set execution time</p>
                            <SyncSchedule />
                            <br />
                            <br />
                            <AddButton selectedMirror={this.state.selectedMirror} />
                        </div>
                    </div>
                </CardText>
            </Card>
        );
    }
}
export default SyncAdd;
