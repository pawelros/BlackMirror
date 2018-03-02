import * as React from 'react';

import { ListItem } from 'material-ui/List';
import Code from 'material-ui/svg-icons/action/code';
import Id from '../Id';
import User from '../User';
import Message from './Message';

interface RevisionProps {
    nestedLevel: number;
    text: string;
    revision: any;
}

class Revision extends React.Component<RevisionProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;
    constructor(props: RevisionProps) {
        super(props);

        this.state = {
            open: true
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
            <ListItem
                nestedLevel={this.props.nestedLevel}
                primaryText={this.props.text}
                initiallyOpen={true}
                leftIcon={<Code />}
                onNestedListToggle={this.handleNestedListToggle}
                open={this.state.open}
                onClick={this.handleToggle}
                nestedItems={[
                    <Id nestedLevel={1} value={this.props.revision.Id} key={this.props.revision.Id + '_revision_id'} />,
                    <User
                        user={this.props.revision.Author}
                        secondaryText={'Author'}
                        nestedLevel={1}
                        key={this.props.revision.Author + '_revision_author'}
                    />,
                    <Message
                        value={this.props.revision.Message}
                        key={this.props.revision.Id + '_revision_message'}
                        nestedLevel={1}
                    />,
                ]}
            />
        );
    }
}
export default Revision;
