import * as React from 'react';
import AutoComplete from 'material-ui/AutoComplete';
import { observer, } from 'mobx-react';
import { action } from 'mobx';
import Mirror from '../../interfaces/Mirror';

interface SourceRepositoryProps {
    mirror: Mirror;
    repositories: any;
}

const dataSourceConfig = {
    text: 'Description',
    value: 'Id'
};

@observer
class SourceRepository extends React.Component<SourceRepositoryProps, any> {

    constructor(props: SourceRepositoryProps) {
        super(props);

        this.state = {
            sourceRepositorySearchText: props.mirror.SourceRepository.Name
        };
    }

    handleSourceRepositoryUpdateInput = (searchText: string) => {
        this.setState({
            sourceRepositorySearchText: searchText,
        });
    }

    @action
    handleSourceRepositoryNewRequest = (chosenRequest: any) => {
        this.props.mirror.SourceRepository.Id = chosenRequest.Id;
        this.setState({
            sourceRepositorySearchText: chosenRequest.Name.toString(),
        });
    }

    render() {
        return (
            <AutoComplete
                floatingLabelText="Source repository"
                searchText={this.state.sourceRepositorySearchText}
                onUpdateInput={this.handleSourceRepositoryUpdateInput}
                onNewRequest={this.handleSourceRepositoryNewRequest}
                dataSource={this.props.repositories}
                filter={AutoComplete.fuzzyFilter}
                openOnFocus={true}
                dataSourceConfig={dataSourceConfig}
            />
        );
    }
}
export default SourceRepository;