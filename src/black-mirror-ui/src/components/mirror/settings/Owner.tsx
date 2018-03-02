import * as React from 'react';
import AutoComplete from 'material-ui/AutoComplete';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import Mirror from '../../interfaces/Mirror';

interface OwnerProps {
    mirror: Mirror;
    users: any;
}

const dataSourceConfig = {
    text: 'Id',
    value: 'Id'
};

@observer
class Owner extends React.Component<OwnerProps, any> {

    constructor(props: OwnerProps) {
        super(props);

        this.state = {
            ownerSearchText: props.mirror.Owner.Id
        };
    }

    handleOwnerUpdateInput = (searchText: string) => {
        this.setState({
            ownerSearchText: searchText,
        });
    }

    @action
    handleOwnerNewRequest = (chosenRequest: any) => {
        this.props.mirror.Owner.Id = chosenRequest.Id;
        this.setState({
            ownerSearchText: chosenRequest.Id.toString(),
        });
    }

    render() {
        return (
            <AutoComplete
                floatingLabelText="Owner"
                searchText={this.state.ownerSearchText}
                onUpdateInput={this.handleOwnerUpdateInput}
                onNewRequest={this.handleOwnerNewRequest}
                dataSource={this.props.users}
                filter={AutoComplete.fuzzyFilter}
                openOnFocus={true}
                dataSourceConfig={dataSourceConfig}
            />
        );
    }
}
export default Owner;