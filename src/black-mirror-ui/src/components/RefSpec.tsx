import * as React from 'react';

import { ListItem } from 'material-ui/List';
import DeveloperMode from 'material-ui/svg-icons/hardware/device-hub';
import { lightBlue300 } from 'material-ui/styles/colors';

interface RefSpecProps {
    refspec: string;
    nestedLevel: number;
}

class RefSpec extends React.Component<RefSpecProps, {}> {
    constructor(props: RefSpecProps) {
        super(props);
        this.state = {
        };
    }

    render() {
        return (
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.refspec}
                secondaryText={'Target repository refspec'}
                leftIcon={<DeveloperMode color={lightBlue300} />}
            />
        );
    }
}
export default RefSpec;