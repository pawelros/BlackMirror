import React from 'react';

import { ListItem } from 'material-ui/List';
import Dns from 'material-ui/svg-icons/action/dns';
import Divider from 'material-ui/Divider';
import { blue50 } from 'material-ui/styles/colors';

import Id from '../Id';
import Revision from './Revision';
import Ref from '../interfaces/Reflection';

interface ReflectionProps {
    index: number;
    reflection: Ref;
}

class Reflection extends React.Component<ReflectionProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;
    constructor(props: ReflectionProps) {
        super(props);

        this.state = {
            open: false
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
                <ListItem
                    primaryText={this.props.index}
                    nestedLevel={1}
                    leftIcon={<Dns color={blue50} />}
                    initiallyOpen={true}
                    onClick={this.handleToggle}
                    primaryTogglesNestedList={true}
                    nestedItems={[
                        <Id
                            nestedLevel={1}
                            value={this.props.reflection.Id}
                            key={this.props.reflection.Id + '_reflection_id'}
                        />,
                        <Revision
                            nestedLevel={1}
                            text={'Source'}
                            revision={this.props.reflection.SourceRevision}
                            key={'source'}
                        />,
                        <Revision
                            nestedLevel={1}
                            text={'Target'}
                            revision={this.props.reflection.TargetRevision}
                            key={'target'}
                        />
                    ]}
                />
                <Divider inset={true} />
            </div>
        );
    }
}
export default Reflection;
