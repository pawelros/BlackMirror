import * as React from 'react';

// icons
import CloudDone from 'material-ui/svg-icons/file/cloud-done';
import AlarmOn from 'material-ui/svg-icons/action/alarm-on';
import { blue500, green600, green800 } from 'material-ui/styles/colors';

import RefreshIndicator from 'material-ui/RefreshIndicator';
import CircularProgress from 'material-ui/CircularProgress';
import SynchronizationStatus from '../interfaces/SynchronizationStatus';

interface SyncStatusProps {
  status: SynchronizationStatus;
  style: any;
}

class SyncStatus extends React.Component<SyncStatusProps, any> {
  constructor(props: SyncStatusProps) {
    super(props);

    this.state = {
      status: SynchronizationStatus.Unknown
    };
  }

  render() {

    switch (this.props.status) {
      case SynchronizationStatus.Failed:
        return (
          <Alarm />
        );
      case SynchronizationStatus.InProgress:
        return (
          <Progress />
        );
      case SynchronizationStatus.Done:
        return (
          <Cloud />
        );
      case SynchronizationStatus.Unknown:
        return (
          <Refresh />
        );
      default:
        return (<Refresh />);
    }
  }
}
// tslint:disable
const Refresh = () => (
  <div className={'sync-status-refresh'}>
    <RefreshIndicator
      size={40}
      left={-70}
      top={0}
      status="loading"
    />
  </div>
);

const Progress = (props: any) => (
  <div className={'sync-status-progress'}>
    <CircularProgress size={40} thickness={3} color={green600} />
  </div>
);
const Cloud = () => (
  <div className={'sync-status-container'}>
    <CloudDone color={green800} className={'sync-status-icon'} />
  </div>
);

const Alarm = () => (
  <div className={'sync-status-container'}>
    <AlarmOn color={blue500} className={'sync-status-icon'} />
  </div>
);

export default SyncStatus;
