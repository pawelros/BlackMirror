import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { List } from 'material-ui/List';

import Paper from 'material-ui/Paper';
import {
    Table,
    TableBody,
    TableRow,
    TableRowColumn,
} from 'material-ui/Table';

import ExistingMirrors from './ExistingMirrors';
import AddNewForm from './AddNewForm';
import NewMirrorPayload from '../../../actions/interfaces/payloads/NewMirror';

import { observer } from 'mobx-react';
const styles = {
    paper: {
        padding: 15
    },
    radioButton: {
        marginBottom: 16,
    },
    addButton: {
        marginRight: 20,

    },
    table: {
        maxWidth: 1000,
        addNewColumn: {
            width: 350
        },
        listColumn: {
            maxWidth: 400
        }
    }
};

interface MirrorsProps {
    payload: NewMirrorPayload;
    mirrors: any;
    repositories: any;
}

@observer
class Mirrors extends React.Component<MirrorsProps, any> {

    constructor(props: MirrorsProps) {
        super(props);
    }

    render() {
        return (
            <Paper>
                <Table selectable={false} style={styles.table}>
                    <TableBody displayRowCheckbox={false}>
                        <TableRow selectable={false}>
                            <TableRowColumn style={styles.table.addNewColumn}>
                                <AddNewForm payload={this.props.payload} repositories={this.props.repositories} />
                            </TableRowColumn >
                            <TableRowColumn style={styles.table.listColumn}>
                                <List style={{ maxHeight: 400, overflow: 'auto' }}>
                                    <ExistingMirrors mirrors={this.props.mirrors} />
                                </List>
                            </TableRowColumn>
                        </TableRow>

                    </TableBody>
                </Table>
            </Paper >
        );
    }
}

export default withRouter(Mirrors);
