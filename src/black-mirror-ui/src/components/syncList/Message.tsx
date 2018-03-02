import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Key from 'material-ui/svg-icons/communication/vpn-key';
import { yellow500 } from 'material-ui/styles/colors';

interface MessageProps {
    nestedLevel: number;
    value: string;
}

class Message extends React.Component<MessageProps, {}> {
    constructor(props: MessageProps) {
        super(props);
    }

    render() {
        return (
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.value}
                secondaryText={'Message'}
                leftIcon={<Key color={yellow500} />}
            />
        );
    }
}
export default Message;
