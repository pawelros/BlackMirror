import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Link from 'material-ui/svg-icons/action/fingerprint';
import { deepOrangeA200
} from 'material-ui/styles/colors';

interface DefaultCommitMessagePrefixProps {
    nestedLevel: number;
    value: string;
    key: string;
}

class DefaultCommitMessagePrefix extends React.Component<DefaultCommitMessagePrefixProps, {}> {

    constructor(props: DefaultCommitMessagePrefixProps) {
        super(props);
    }

    render() {
        return (
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.value}
                secondaryText={'Default commit message prefix'}
                leftIcon={<Link color={deepOrangeA200} />}
            />
        );
    }
}
export default DefaultCommitMessagePrefix;
