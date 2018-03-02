import * as React from 'react';
import { withRouter } from 'react-router-dom';
import { List, ListItem } from 'material-ui/List';
import Divider from 'material-ui/Divider';
import GitIcon from '../../images/svg-icons/git';
import SvnIcon from '../../images/svg-icons/svn';
import ISvcRepository from '../interfaces/SvcRepository';
import ISvcRepositoryType from '../interfaces/SvcRepositoryType';

interface RepositoryFlatProps {
    nestedLevel: number;
    value: string;
    repository: ISvcRepository;
    initiallyOpen: boolean;
    history?: any;
}

class RepositoryFlat extends React.Component<RepositoryFlatProps, any> {

    render() {
        return (
            <div>
                <List>
                    <ListItem
                        key={this.props.repository.Id}
                        primaryText={this.props.repository.Name}
                        secondaryText={this.props.repository.Type}
                        primaryTogglesNestedList={true}
                        leftIcon={this.props.repository.Type.toString() === ISvcRepositoryType[ISvcRepositoryType.git] ?
                            <GitIcon /> : <SvnIcon />}
                        onClick={() => this.props.history.push('/repository/' + this.props.repository.Id)}
                    />
                </List>
                <Divider inset={true} />
            </div>
        );
    }
}

export default withRouter(RepositoryFlat);
