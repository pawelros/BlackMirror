import * as React from 'react';
import AutoComplete from 'material-ui/AutoComplete';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import Mirror from '../../interfaces/Mirror';

interface TargetRepositoryProps {
    mirror: Mirror;
    repositories: any;
}

const dataSourceConfig = {
    text: 'Description',
    value: 'Id'
};

@observer
class TargetRepository extends React.Component<TargetRepositoryProps, any> {

    constructor(props: TargetRepositoryProps) {
        super(props);

        this.state = {
            targetRepositorySearchText: props.mirror.TargetRepository.Name
        };
    }

    handleTargetRepositoryUpdateInput = (searchText: string) => {
        this.setState({
            targetRepositorySearchText: searchText,
        });
    }

    @action
    handleTargetRepositoryNewRequest = (chosenRequest: any) => {
        this.props.mirror.SourceRepository.Id = chosenRequest.Id;
        this.setState({
            targetRepositorySearchText: chosenRequest.Name.toString(),
        });
    }

    render() {
        return (
            <AutoComplete
                floatingLabelText="Target repository"
                searchText={this.state.targetRepositorySearchText}
                onUpdateInput={this.handleTargetRepositoryUpdateInput}
                onNewRequest={this.handleTargetRepositoryNewRequest}
                dataSource={this.props.repositories}
                filter={AutoComplete.fuzzyFilter}
                openOnFocus={true}
                dataSourceConfig={dataSourceConfig}
            />
        );
    }
}
export default TargetRepository;