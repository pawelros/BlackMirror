import * as React from 'react';
import RestApi from '../../actions/restApi';
import { Card, CardTitle, CardText } from 'material-ui/Card';
import User from '../interfaces/User';
import UserFlat from './UserFlat';

interface UserListProps {

}

interface UserListState {
    users: User[];
    usersInterval: any;
}

class UserList extends React.Component<UserListProps, UserListState> {
    constructor(props: UserListProps) {
        super(props);

        this.state = {
            users: [],
            usersInterval: false,
        };
    }

    componentDidMount() {
        var self = this;
        // run first
        RestApi.getUsers().then((response) => {

            self.setState({ users: response });

        }, function (error: any) {
            // console.error("Failed2!", error);
        });
        // set interval
        self.setState({
            usersInterval: setInterval(function () {
                RestApi.getUsers().then((response) => {

                    self.setState({ users: response });

                }, function (error: any) {
                    // console.error("Failed2!", error);
                });
            }.bind(this), 5000)
        });
    }

    componentWillUnmount() {
        // tslint:disable-next-line:no-unused-expression
        this.state.usersInterval && clearInterval(this.state.usersInterval);
        this.setState({ usersInterval: false });
    }

    render() {
        const listItems = this.state.users.map((u: User) =>
            // tslint:disable-next-line:jsx-wrap-multiline
            <UserFlat
                key={u.Id}
                user={u}
                nestedLevel={1}
                value={u.Id}
                initiallyOpen={false}
            />
        );

        return (
            <Card>
                <CardTitle title="Users" subtitle="Here you can manage users." />
                <CardText>
                    <div>
                        <div className="mirrorlist">
                            {listItems}
                        </div>
                    </div>
                </CardText>
            </Card>
        );
    }
}
export default UserList;
