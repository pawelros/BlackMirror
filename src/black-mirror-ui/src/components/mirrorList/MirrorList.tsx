import * as React from 'react';
import RestApi from '../../actions/restApi';
import { Card, CardTitle, CardText } from 'material-ui/Card';

import MirrorFlat from './MirrorFlat';
import IM from '../interfaces/Mirror';
import Synchronization from '../interfaces/Synchronization';
import SynchronizationStatus from '../interfaces/SynchronizationStatus';

interface MirrorListProps {

}

interface EnhancedMirror {
    mirror: IM;
    synchronizations: Synchronization[];
    history?: any;
}

interface MirrorListState {
    mirrors: EnhancedMirror[];
    mirrorsInterval: any;
}

class MirrorList extends React.Component<MirrorListProps, MirrorListState> {
    constructor(props: MirrorListProps) {
        super(props);

        this.state = {
            mirrors: [],
            mirrorsInterval: false,
        };
    }

    componentDidMount() {
        var self = this;
        // run first
        RestApi.getMirrors().then((response) => {

            var m = response.map((item: IM) => {
                return { mirror: item, lasSync: SynchronizationStatus.Unknown };
            });
            self.setState({ mirrors: m });

            m.forEach(function (mirror: EnhancedMirror) {
                RestApi.getLastSync(mirror.mirror.Id).then((res) => {

                    let sx: Synchronization[];
                    sx = res;

                    mirror.synchronizations = sx;
                    self.setState({ mirrors: m });
                });
            });

        }, function (error: any) {
            // console.error("Failed2!", error);
        });
        // set interval
        self.setState({
            mirrorsInterval: setInterval(function () {
                RestApi.getMirrors().then((response) => {

                    var m = response.map((item: IM) => {
                        return { mirror: item, lasSync: SynchronizationStatus.Unknown };
                    });
                    self.setState({ mirrors: m });

                    m.forEach(function (mirror: EnhancedMirror) {
                        RestApi.getLastSync(mirror.mirror.Id).then((res) => {

                            let sx: Synchronization[];
                            sx = res;

                            mirror.synchronizations = sx;
                            self.setState({ mirrors: m });
                        });
                    });
                }, function (error: any) {
                    // console.error("Failed2!", error);
                });
            }.bind(this), 5000)
        });
    }

    componentWillUnmount() {
        // tslint:disable-next-line:no-unused-expression
        this.state.mirrorsInterval && clearInterval(this.state.mirrorsInterval);
        this.setState({ mirrorsInterval: false });
    }

    render() {
        const listItems = this.state.mirrors.map((m: EnhancedMirror) =>
            // tslint:disable-next-line:jsx-wrap-multiline
            <MirrorFlat
                key={m.mirror.Id}
                mirror={m.mirror}
                synchronizations={m.synchronizations}
                nestedLevel={1}
                value={m.mirror.Id}
                initiallyOpen={false}
            />
        );

        return (
            <Card>
                <CardTitle title="Mirrors" subtitle="All mirror definitions that you own." />
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
export default MirrorList;
