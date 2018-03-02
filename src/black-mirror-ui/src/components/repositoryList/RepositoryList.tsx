import * as React from 'react';
import RestApi from '../../actions/restApi';
import { Card, CardTitle, CardText } from 'material-ui/Card';
import Repository from '../interfaces/SvcRepository';
import RepositoryFlat from './RepositoryFlat';

interface RepositoryListProps {

}

interface RepositoryListState {
    repositories: Repository[];
    repositoriesInterval: any;
}

class RepositoryList extends React.Component<RepositoryListProps, RepositoryListState> {
    constructor(props: RepositoryListProps) {
        super(props);

        this.state = {
            repositories: [],
            repositoriesInterval: false,
        };
    }

    componentDidMount() {
        var self = this;
        // run first
        RestApi.getRepositories().then((response) => {

            self.setState({ repositories: response });

        }, function (error: any) {
            // console.error("Failed2!", error);
        });
        // set interval
        self.setState({
            repositoriesInterval: setInterval(function () {
                RestApi.getRepositories().then((response) => {

                    self.setState({ repositories: response });

                }, function (error: any) {
                    // console.error("Failed2!", error);
                });
            }.bind(this), 5000)
        });
    }

    componentWillUnmount() {
        // tslint:disable-next-line:no-unused-expression
        this.state.repositoriesInterval && clearInterval(this.state.repositoriesInterval);
        this.setState({ repositoriesInterval: false });
    }

    render() {
        const listItems = this.state.repositories.map((r: Repository) =>
            // tslint:disable-next-line:jsx-wrap-multiline
            <RepositoryFlat
                key={r.Id}
                repository={r}
                nestedLevel={1}
                value={r.Id}
                initiallyOpen={false}
            />
        );

        return (
            <Card>
                <CardTitle title="Repositories" subtitle="All repository definitions that you own." />
                <CardText>
                    <div>
                        <div className="mirrorlist">
                            {listItems}
                        </div>
                    </div>
                </CardText>
            </Card>
        );
    }
}
export default RepositoryList;
