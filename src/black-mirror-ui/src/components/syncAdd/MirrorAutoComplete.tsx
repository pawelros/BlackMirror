import * as React from 'react';
import AutoComplete from 'material-ui/AutoComplete';
import RestApi from '../../actions/restApi';

interface MirrorAutoCompleteProps {
    selectMirror(match: any): void;
}

const dataSourceConfig = {
    text: 'textKey',
    value: 'valueKey',
};

class MirrorAutoComplete extends React.Component<MirrorAutoCompleteProps, any> {
    handleUpdateInput: (value: any) => void;

    constructor(props: MirrorAutoCompleteProps) {
        super(props);
        this.state = {
            dataSource: [],
            error: null
        };

        this.handleUpdateInput = (value) => {
            var match = null;
            this.state.dataSource.forEach(function (m: any) {
                if (m.textKey === value) {
                    match = m;

                    return;
                }
            });

            if (!match) {
                this.setState({ errorText: 'You must chose an existing mirror.' });
            } else {
                this.setState({ errorText: null });
            }
            let f = props.selectMirror;

            f(match);
        };
    }

    componentDidMount() {
        var self = this;
        RestApi.getMirrors().then((response) => {

            let mirrors: Array<{ textKey: string, valueKey: string }>;
            mirrors = [];

            response.forEach(function (element: any) {
                mirrors.push({ textKey: element.Name, valueKey: element.Id });
            });

            self.setState({ dataSource: mirrors });
        }
            // tslint:disable-next-line:no-empty
            ,                     function (error: any) {
            });
    }

    render() {
        return (
            <div>
                <AutoComplete
                    floatingLabelText="Select mirror"
                    filter={AutoComplete.noFilter}
                    openOnFocus={true}
                    dataSource={this.state.dataSource}
                    dataSourceConfig={dataSourceConfig}
                    errorText={this.state.errorText}
                    onUpdateInput={this.handleUpdateInput}
                /><br />
            </div>
        );
    }
}
export default MirrorAutoComplete;
