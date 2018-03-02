import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Key from 'material-ui/svg-icons/communication/vpn-key';
import { yellow500 } from 'material-ui/styles/colors';

interface IdProps {
    nestedLevel: number;
    value: string;
}

class Id extends React.Component<IdProps, {}> {
    constructor(props: IdProps) {
        super(props);
        this.state = {
            dupa: props.value
        };
    }

    render() {
        return (
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.value}
                secondaryText={'Id'}
                leftIcon={<Key color={yellow500} />}
            />
        );
    }
}
export default Id;