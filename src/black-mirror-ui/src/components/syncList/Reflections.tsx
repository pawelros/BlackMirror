import React from 'react';
import RestApi from '../../actions/restApi';
import { List, ListItem } from 'material-ui/List';
import Divider from 'material-ui/Divider';
import Dns from 'material-ui/svg-icons/action/dns';

import { blue50 } from 'material-ui/styles/colors';

import Reflection from './Reflection';
import Ref from '../interfaces/Reflection';

interface ReflectionsProps {
    syncId: string;
}

class Reflections extends React.Component<ReflectionsProps, any> {
    handleNestedListToggle: (item: any) => void;
    handleToggle: () => void;
    constructor(props: ReflectionsProps) {
        super(props);

        this.state = {
            reflections: [],
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

    componentDidMount() {
        var self = this;
        RestApi.getReflections(this.props.syncId).then((response) => {
            self.setState({ reflections: response });
        }
            // tslint:disable-next-line:no-empty
            ,                                          function (error: any) {

            });
    }

    render() {
        const listItems = this.state.reflections.map((r: Ref, i: number) =>
            <Reflection reflection={r} index={i} key={i} />
        );
        return (
            <div>
                <List>
                    <ListItem
                        nestedLevel={1}
                        primaryText={'Reflections'}
                        secondaryText={'Total: ' + listItems.length}
                        leftIcon={<Dns color={blue50} />}
                        initiallyOpen={false}
                        onClick={this.handleToggle}
                        primaryTogglesNestedList={true}
                        nestedItems={[
                            listItems
                        ]}
                    />
                </List>
                <Divider inset={true} />
            </div>
        );
    }
}
export default Reflections;
