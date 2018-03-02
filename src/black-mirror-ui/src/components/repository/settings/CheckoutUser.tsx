import * as React from 'react';
import AutoComplete from 'material-ui/AutoComplete';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import ISvcRepository from '../../interfaces/SvcRepository';

interface CheckoutUserProps {
    repository: ISvcRepository;
    users: any;
}

const dataSourceConfig = {
    text: 'Id',
    value: 'Id'
};

@observer
class CheckoutUser extends React.Component<CheckoutUserProps, any> {

    constructor(props: CheckoutUserProps) {
        super(props);

        this.state = {
            ownerSearchText: props.repository.CheckoutUser.Id
        };
    }

    handleOwnerUpdateInput = (searchText: string) => {
        this.setState({
            ownerSearchText: searchText,
        });
    }

    @action
    handleOwnerNewRequest = (chosenRequest: any) => {
        this.props.repository.CheckoutUser.Id = chosenRequest.Id;
        this.setState({
            ownerSearchText: chosenRequest.Id.toString(),
        });
    }

    render() {
        return (
            <AutoComplete
                floatingLabelText="Checkout user"
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
export default CheckoutUser;