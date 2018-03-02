import * as React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import ActionAndroid from 'material-ui/svg-icons/av/library-add';
import RestApi from '../../actions/restApi';
import { withRouter } from 'react-router-dom';

const styles = {
    button: {
        margin: 0,
    },
    exampleImageInput: {
        cursor: 'pointer',
        position: 'absolute',
        top: 0,
        bottom: 0,
        right: 0,
        left: 0,
        width: '100%',
        opacity: 0,
    },
};

interface AddSyncButtonProps {
    selectedMirror: any;
    history?: any;
}

class AddSyncButton extends React.Component<AddSyncButtonProps, {}> {
    handleClick: () => void;
    constructor(props: AddSyncButtonProps) {
        super(props);

        this.handleClick = () => {
            var mirrorId = this.props.selectedMirror.valueKey;
            RestApi.postSync(mirrorId).then((response) => {
      
                this.props.history.push('/mirror/' + mirrorId + '/sync');
            }
                // tslint:disable-next-line:no-empty
                ,                           function (error: any) {
                });
    
        };
    }

    render() {
        return (
            <div>
                <RaisedButton
                    label="Create Sync"
                    labelPosition="before"
                    primary={true}
                    icon={<ActionAndroid />}
                    style={styles.button}
                    onClick={this.handleClick}
                />
            </div>
        );
    }
}
export default withRouter(AddSyncButton);
