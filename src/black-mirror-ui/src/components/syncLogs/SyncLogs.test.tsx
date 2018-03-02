import * as React from 'react';
import renderer from 'react-test-renderer';

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
const customMuiTheme = getMuiTheme(darkBaseTheme);
import SyncLogs from './SyncLogs';

// todo: this test should mock API - component should have dependency on 'store'
// and then mocked store should be injected
// currently this component instantiates and calls API itself which is an antipattern
it('renders and is compatible with the snapshot', () => {
    const p = { params: { id: 'xyz' } };

    const tree = renderer
        .create(
        <MuiThemeProvider muiTheme={customMuiTheme}>
            <SyncLogs
                match={p}
            />
        </MuiThemeProvider>)
        .toJSON();
    expect(tree).toMatchSnapshot();
});
