import * as React from 'react';
import renderer from 'react-test-renderer';

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
const customMuiTheme = getMuiTheme(darkBaseTheme);
import User from './User';

it('renders and is compatible with the snapshot', () => {
    const tree = renderer
        .create(
        <MuiThemeProvider muiTheme={customMuiTheme}>
            <User
                user={'Pro'}
                nestedLevel={0}
                secondaryText={'Admin'}
            />
        </MuiThemeProvider>)
        .toJSON();
    expect(tree).toMatchSnapshot();
});
