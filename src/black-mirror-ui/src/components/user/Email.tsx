import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Icon from 'material-ui/svg-icons/communication/email';
import { yellow200 } from 'material-ui/styles/colors';

interface EmailProps {
    nestedLevel: number;
    value: string;
    key: string;
}

class Email extends React.Component<EmailProps, {}> {

    constructor(props: EmailProps) {
        super(props);
    }

    render() {
        return (
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.value}
                secondaryText={'Email'}
                leftIcon={<Icon color={yellow200} />}
            />
        );
    }
}
export default Email;
