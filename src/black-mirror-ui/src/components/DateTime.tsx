import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Clock from 'material-ui/svg-icons/device/access-time';
import { yellow200 } from 'material-ui/styles/colors';

interface DateTimeProps {
    nestedLevel: number;
    text: string;
    value: string;
}

class DateTime extends React.Component<DateTimeProps, {}> {

    render() {
        return(
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.value}
                secondaryText={this.props.text ? this.props.text : 'Creation time'}
                leftIcon={<Clock color={yellow200} />}
            />
        );
    }
}
export default DateTime;