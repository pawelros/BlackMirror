import * as React from 'react';
import RestApi from '../../actions/restApi';
import { Card, CardTitle, CardText } from 'material-ui/Card';
import SyncStatus from './SyncStatus';
import * as moment from 'moment';

const style = {
    container: {
        position: 'relative',
    },
    refresh: {
        position: 'absolute',
    },
    progress: {
        position: 'absolute',
        left: 10
    },
    icon: {
        position: 'absolute',
        left: -55,
        top: 5
    },
};

class SyncLogs extends React.Component<any, any> {
    constructor(props: any) {
        super(props);

        this.state = {
            sync: { Mirror: { Name: 'Retrieving' }, Status: 'status' },
            logs: [],
            logsInterval: null,
            syncInterval: null,
        };
    }

    componentDidMount() {
        var self = this;

        // lame
        // run first
        RestApi.getSyncLogs(this.props.match.params.id).then((response) => {
            self.setState({ logs: response.Logs });
        }
        );

        RestApi.getSync(this.props.match.params.id).then((response) => {

            self.setState({ sync: response });
        });

        // then set interval
        self.setState({
            logsInterval: setInterval(function () {
                RestApi.getSyncLogs(self.props.match.params.id).then((response) => {
                    self.setState({ logs: response.Logs });
                });
            }.bind(this), 10000)
        });

        self.setState({
            syncInterval: setInterval(function () {
                RestApi.getSync(self.props.match.params.id).then((response) => {

                    self.setState({ sync: response });
                });
            }.bind(this), 10000)
        });
    }

    componentWillUnmount() {
        // tslint:disable-next-line:no-unused-expression
        this.state.logsInterval && clearInterval(this.state.logsInterval);
        this.setState({ logsInterval: false });

        // tslint:disable-next-line:no-unused-expression
        this.state.syncInterval && clearInterval(this.state.syncInterval);
        this.setState({ syncInterval: false });
    }

    render() {
        const listItems = this.state.logs.map((l: any, index: number) =>
            <p key={index}> {moment(l.Timestamp).format('DD-MM-YYYY HH:mm')} {l.Text}</p>
        );

        return (
            <Card>
                <CardTitle
                    title={'Logs'}
                    subtitle={this.state.sync.Mirror.Name
                        + ' sync ' + this.state.sync.Id + ' '
                        + this.state.sync.Status}
                />
                <CardText>
                    <div>
                        <div className="">

                            <SyncStatus status={this.state.sync.Status} style={style} />

                            {listItems}
                        </div>
                    </div>
                </CardText>
            </Card>
        );
    }
}
export default SyncLogs;
