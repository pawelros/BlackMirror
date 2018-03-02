import * as React from 'react';
import renderer from 'react-test-renderer';

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
const customMuiTheme = getMuiTheme(darkBaseTheme);
import Logs from './Logs';
var SyncData = require('../../test/sync.json');

it('renders and is compatible with the snapshot', () => {
    const tree = renderer
        .create(
        <MuiThemeProvider muiTheme={customMuiTheme}>
            <Logs sync={SyncData} />
        </MuiThemeProvider>)
        .toJSON();
    expect(tree).toMatchSnapshot();
});
