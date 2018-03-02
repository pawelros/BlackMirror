import * as React from 'react';
import renderer from 'react-test-renderer';
import { mount, shallow } from 'enzyme';
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
import getMuiTheme from 'material-ui/styles/getMuiTheme';
import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
const muiTheme = getMuiTheme(darkBaseTheme);
// import { createMount } from 'material-ui/test-utils';
import Reflection from './Reflection';
import { ListItem } from 'material-ui/List';
// import Id from '../Id';
// import Revision from './Revision';
// import Ref from '../interfaces/Reflection';
const json = require('../../test/reflection.json');

it('renders and is compatible with the snapshot', () => {
    const props = {
        index: 6,
        reflection: json
    };

    const tree = renderer
        .create(
        <MuiThemeProvider muiTheme={muiTheme}>
            <Reflection index={props.index} reflection={props.reflection} />
        </MuiThemeProvider>)
        .toJSON();
    expect(tree).toMatchSnapshot();
});

it('click unfolds item', () => {
    const props = {
        index: 6,
        reflection: json
    };

    const component = shallow(
        <Reflection index={props.index} reflection={props.reflection} />
    );

    expect(component.state('open')).toBe(false);
    component.find(ListItem).simulate('click');
    expect(component.state('open')).toBe(true);

});

it('it contains all elements', () => {
    const props = {
        index: 6,
        reflection: json
    };

    const component = mount(
        <MuiThemeProvider muiTheme={muiTheme}>
            <Reflection index={props.index} reflection={props.reflection} />
        </MuiThemeProvider>
    );

    expect(component.state('open')).toBe(false);
    component.find(ListItem).simulate('click');
    expect(component.state('open')).toBe(true);

});
