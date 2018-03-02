import * as React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import ActionAndroid from 'material-ui/svg-icons/av/library-add';
import Snackbar from 'material-ui/Snackbar';
import RestApi from '../../actions/restApi';

const styles = {
  button: {
    margin: 12,
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
  mirrorId: string;
}

class AddSyncButton extends React.Component<AddSyncButtonProps, any> {
  constructor(props: AddSyncButtonProps) {
    super(props);

    this.state = {
      openSnackbar: false,
      snackbarMessage: ''
    };
  }

  handleSnackbarRequestClose = () => {
    this.setState({
      openSnackbar: false,
    });
  }

  handleClick = () => {
    var self = this;
    RestApi.postSync(this.props.mirrorId).then((response) => {
      this.setState({ openSnackbar: true, snackbarMessage: 'Synchronization created.' });
    }
      // tslint:disable-next-line:no-empty
      , function (error: any) {
        self.setState({ openSnackbar: true, snackbarMessage: error.message });
      });
  }

  render() {
    return (
      <div>
        <RaisedButton
          label="Sync Now"
          labelPosition="before"
          primary={true}
          icon={<ActionAndroid />}
          style={styles.button}
          onClick={this.handleClick}
        />
        <Snackbar
          open={this.state.openSnackbar}
          message={this.state.snackbarMessage}
          autoHideDuration={3000}
          onRequestClose={this.handleSnackbarRequestClose}
        />
      </div>
    );
  }
}
export default AddSyncButton;
