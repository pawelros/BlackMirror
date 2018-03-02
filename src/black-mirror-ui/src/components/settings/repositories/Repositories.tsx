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
import ExistingRepositories from './ExistingRepositories';
import AddNewForm from './AddNewForm';
import NewRepositoryPayload from '../../../actions/interfaces/payloads/NewRepository';

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

interface RepositoriesProps {
    payload: NewRepositoryPayload;
    users: any;
    repositories: any;
}

@observer
class Repositories extends React.Component<RepositoriesProps, any> {

    constructor(props: RepositoriesProps) {
        super(props);
    }

    render() {
        return (
            <Paper>
                <Table selectable={false} style={styles.table}>
                    <TableBody displayRowCheckbox={false}>
                        <TableRow selectable={false}>
                            <TableRowColumn style={styles.table.addNewColumn}>
                                <AddNewForm payload={this.props.payload} users={this.props.users} />
                            </TableRowColumn >
                            <TableRowColumn style={styles.table.listColumn}>
                                <List style={{ maxHeight: 400, overflow: 'auto' }}>
                                    <ExistingRepositories repositories={this.props.repositories} />
                                </List>
                            </TableRowColumn>
                        </TableRow>

                    </TableBody>
                </Table>
            </Paper >
        );
    }
}

export default withRouter(Repositories);