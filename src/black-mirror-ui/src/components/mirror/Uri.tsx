import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Link from 'material-ui/svg-icons/content/link';
import { lightBlue500 } from 'material-ui/styles/colors';

interface UriProps {
    nestedLevel: number;
    value: string;
    key: string;
}

class Uri extends React.Component<UriProps, {}> {

    constructor(props: UriProps) {
        super(props);
    }

    render() {
        return (
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.value}
                secondaryText={'Remote'}
                leftIcon={<Link color={lightBlue500} />}
            />
        );
    }
}
export default Uri;
