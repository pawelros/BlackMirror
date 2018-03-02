import * as React from 'react';
import './App.css';
import { Grid, Row, Col } from 'react-flexbox-grid';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';

import { BrowserRouter as Router } from 'react-router-dom';
const customMuiTheme = getMuiTheme(darkBaseTheme);
import RestApi from './actions/restApi';
import Header from './containers/Header';
import Main from './containers/Main';
import Footer from './containers/Footer';
import UserInterface from './actions/interfaces/User';

interface MyState {
  user: UserInterface;
}
// tslint:disable-next-line:no-any
class App extends React.Component<any, MyState> {
  // tslint:disable-next-line:no-any
  constructor(props: any) {
    super(props);
    this.state = {
      user: { Auth: { Id: '', Pid: '', Name: '' }, Exists: false, Identity: { Id: '', Name: '', Email: '' } }
    };
  }

  componentWillMount() {
    var self = this;
    // run first
    RestApi.getUserSelf().then((response) => {
      self.setState({ user: response });
    },
      function (error: Error) {
        // tslint:disable-next-line:no-console
        console.error('Failed to load user details', error);
      });
  }

  render() {

    return (
      <Router>
        <Grid>
          <Row top="xs">
            <Col xs={12} >
              <Header theme={customMuiTheme} user={this.state.user} history={this.props.history} />
            </Col>
          </Row>
          <Row middle="xs">
            <Col xs={12} >
              <Main theme={customMuiTheme} user={this.state.user} />
            </Col>
          </Row>
          <Row bottom="xs">
            <Col xs={12} >
              <Footer theme={customMuiTheme} user={this.state.user} history={this.props.history} />
            </Col>
          </Row>
        </Grid>
      </Router>
    );
  }
}

export default App;
