import * as React from 'react';
import renderer from 'react-test-renderer';

import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
const customMuiTheme = getMuiTheme(darkBaseTheme);
import DateTime from './DateTime';

it('renders and is compatible with the snapshot - text not provided', () => {
    const tree = renderer
        .create(
        <MuiThemeProvider muiTheme={customMuiTheme}>
            <DateTime
                value={'012345'}
                nestedLevel={0}
                text={''}
            />
        </MuiThemeProvider>)
        .toJSON();
    expect(tree).toMatchSnapshot();
});

it('renders and is compatible with the snapshot - text provided', () => {
    const tree = renderer
        .create(
        <MuiThemeProvider muiTheme={customMuiTheme}>
            <DateTime
                value={'012345'}
                nestedLevel={0}
                text={'This is a test.'}
            />
        </MuiThemeProvider>)
        .toJSON();
    expect(tree).toMatchSnapshot();
});
