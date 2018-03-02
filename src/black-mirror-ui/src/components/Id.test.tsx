import * as React from 'react';
import renderer from 'react-test-renderer';

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
const customMuiTheme = getMuiTheme(darkBaseTheme);

import Id from './Id';

it('renders and is compatible with the snapshot', () => {
    const tree = renderer
        .create(
        <MuiThemeProvider muiTheme={customMuiTheme}>
            <Id
                value={'012345'}
                nestedLevel={0}
            />
        </MuiThemeProvider>)
        .toJSON();
    expect(tree).toMatchSnapshot();
});
