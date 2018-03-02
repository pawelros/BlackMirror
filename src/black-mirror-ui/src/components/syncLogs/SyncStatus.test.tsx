// import * as React from 'react';
// import renderer from 'react-test-renderer';

// import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider';
// import getMuiTheme from 'material-ui/styles/getMuiTheme';
// import darkBaseTheme from 'material-ui/styles/baseThemes/darkBaseTheme';
// const customMuiTheme = getMuiTheme(darkBaseTheme);
// import SyncStatus from './SyncStatus';

// it('renders and is compatible with the snapshot - Scheduled status', () => {
//     const tree = renderer
//         .create(
//         <MuiThemeProvider muiTheme={customMuiTheme}>
//             <SyncStatus
//                 status={'Scheduled'}
//             />
//         </MuiThemeProvider>)
//         .toJSON();
//     expect(tree).toMatchSnapshot();
// });

// it('renders and is compatible with the snapshot - InProgress status', () => {
//     const tree = renderer
//         .create(
//         <MuiThemeProvider muiTheme={customMuiTheme}>
//             <SyncStatus
//                 status={'InProgress'}
//             />
//         </MuiThemeProvider>)
//         .toJSON();
//     expect(tree).toMatchSnapshot();
// });

// it('renders and is compatible with the snapshot - Done status', () => {
//     const tree = renderer
//         .create(
//         <MuiThemeProvider muiTheme={customMuiTheme}>
//             <SyncStatus
//                 status={'Done'}
//             />
//         </MuiThemeProvider>)
//         .toJSON();
//     expect(tree).toMatchSnapshot();
// });

// it('renders and is compatible with the snapshot - XXX status', () => {
//     const tree = renderer
//         .create(
//         <MuiThemeProvider muiTheme={customMuiTheme}>
//             <SyncStatus
//                 status={'XXX'}
//             />
//         </MuiThemeProvider>)
//         .toJSON();
//     expect(tree).toMatchSnapshot();
// });

// it('renders and is compatible with the snapshot - Retrieving status... status', () => {
//     const tree = renderer
//         .create(
//         <MuiThemeProvider muiTheme={customMuiTheme}>
//             <SyncStatus
//                 status={'Retrieving status...'}
//             />
//         </MuiThemeProvider>)
//         .toJSON();
//     expect(tree).toMatchSnapshot();
// });
