import * as React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import ActionAndroid from 'material-ui/svg-icons/av/library-add';
// import RestApi from '../../actions/restApi';
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

interface CreateAccountButtonProps {
    history?: any;
}

class CreateAccountButton extends React.Component<CreateAccountButtonProps, any> {
    handleClick: () => void;
    constructor(props: CreateAccountButtonProps) {
        super(props);

        this.handleClick = () => {
            // var mirrorId = this.props.selectedMirror.valueKey;
            // RestApi.postSync(mirrorId).then((response) => {

            //     console.log('OK. Redirecting to sync page.');

            //     this.props.history.push('/mirror/' + mirrorId + '/sync');
            // }
            //     , function (error) {
            //         console.error("Failed2!", error);
            //     });

        };
    }

    render() {
        return (
            <div>
                <RaisedButton
                    label="Create Account"
                    labelPosition="before"
                    primary={true}
                    icon={<ActionAndroid />}
                    style={styles.button}
                    onClick={() => this.props.history.push('/settings/account/new')}
                    fullWidth={true}
                />
            </div>
        );
    }
}
export default withRouter(CreateAccountButton);
