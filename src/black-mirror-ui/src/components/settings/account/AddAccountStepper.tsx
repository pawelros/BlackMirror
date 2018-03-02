import * as React from 'react';
import {
  Step,
  Stepper,
  StepLabel,
  StepContent,
} from 'material-ui/Stepper';
import RaisedButton from 'material-ui/RaisedButton';
import FlatButton from 'material-ui/FlatButton';
import AccountForm from './AccountForm';
import RepositoryCredentials from './RepositoryCredentials';

import { observer } from 'mobx-react';

interface AddAccountStepperSteps {
  store: any;
}

@observer
class AddAccountStepper extends React.Component<AddAccountStepperSteps, any> {
  constructor(props: AddAccountStepperSteps) {
    super(props);
    this.state = {
      finished: false,
      stepIndex: 0,
      error: ''
    };
  }

  handleNext = () => {
    const { stepIndex } = this.state;
    // will be finish
    if (stepIndex + 1 === 3) {
      // send a call to API
      this.props.store.payloads.newUser.save();
    }
    this.setState({
      stepIndex: stepIndex + 1,
      finished: stepIndex >= 2,
    });
  }

  handlePrev = () => {
    const { stepIndex } = this.state;
    if (stepIndex > 0) {
      this.setState({ stepIndex: stepIndex - 1 });
    }
  }

  renderStepActions(step: number) {
    const { stepIndex } = this.state;

    return (
      <div style={{ margin: '12px 0' }}>
        <RaisedButton
          label={stepIndex === 2 ? 'Finish' : 'Next'}
          disableTouchRipple={true}
          disableFocusRipple={true}
          primary={true}
          onClick={this.handleNext}
          style={{ marginRight: 12 }}
        />
        {step > 0 && (
          <FlatButton
            label="Back"
            disabled={stepIndex === 0}
            disableTouchRipple={true}
            disableFocusRipple={true}
            onClick={this.handlePrev}
          />
        )}
      </div>
    );
  }

  render() {
    const { stepIndex } = this.state;

    return (
      <div style={{ maxWidth: 380, maxHeight: 1000, margin: 'auto' }}>
        <Stepper activeStep={stepIndex} orientation="vertical">
          <Step>
            <StepLabel style={{ color: 'white' }}>Add your account details</StepLabel>
            <StepContent>
              <AccountForm
                exists={this.props.store.currentUser.Exists}
                account={this.props.store.payloads.newUser}
              />
              {this.renderStepActions(0)}
            </StepContent>
          </Step>
          <Step>
            <StepLabel style={{ color: 'white' }}>Add repository credentials</StepLabel>
            <StepContent>
              <p>If you want to create a new mirror that will use your repository credentials
                to sync changes, or allow other mirrors to use your credentials, here you can add them.</p>
              <RepositoryCredentials
                credentials={this.props.store.payloads.newUser.repositoryCredentials}
              />
              {this.renderStepActions(1)}
            </StepContent>
          </Step>
          <Step>
            <StepLabel style={{ color: 'white' }}>That is it</StepLabel>
            <StepContent>
              <div>
                <p>
                  That&#39;s all mate! You can now submit or go back and review once more.
              </p>
                <p style={{ color: 'red' }}>
                  {String(this.state.error)}
                </p>
                {this.renderStepActions(2)}
              </div>
            </StepContent>
          </Step>
        </Stepper>
      </div>
    );
  }
}

export default AddAccountStepper;