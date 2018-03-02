import * as React from 'react';
import { List } from 'material-ui/List';
import Divider from 'material-ui/Divider';
import IUser from '../interfaces/User';
import UserDetails from './UserDetails';

interface OverviewProps {
    nestedLevel: number;
    value: string;
    user: IUser;
    initiallyOpen: boolean;
}

class Overview extends React.Component<OverviewProps, any> {
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

    render() {

        return (
            <div>
                <List>
                    <UserDetails
                        user={this.props.user}
                        secondaryText={''}
                        initiallyOpen={true}
                    />
                </List>
                <Divider inset={true} />
            </div>
        );
    }
}

export default Overview;
