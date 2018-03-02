import * as React from 'react';
// icons
import CloudDone from 'material-ui/svg-icons/file/cloud-done';
import AlarmOn from 'material-ui/svg-icons/action/alarm-on';
import Failed from 'material-ui/svg-icons/notification/sms-failed';
import { blue500, red500, green600, green800 } from 'material-ui/styles/colors';

import RefreshIndicator from 'material-ui/RefreshIndicator';
import CircularProgress from 'material-ui/CircularProgress';
import SynchronizationStatus from '../interfaces/SynchronizationStatus';

const style: any = {
  container: {
    position: 'relative',
  },
  refresh: {
    position: 'absolute',
  },
  progress: {
    position: 'absolute',
    left: 0
  },
  icon: {
    position: 'absolute',
    left: -55,
    top: 5
  },
};

const Refresh = () => (
  <div style={style.refresh}>
    <RefreshIndicator
      size={40}
      left={-70}
      top={0}
      status="loading"
      style={style.item}
    />
  </div>
);

const Progress = (props: any) => (
  <div style={props.style ? props.style.progress : style.progress}>
    <CircularProgress size={40} thickness={3} style={props.style ? props.style.item : style.item} color={green600} />
  </div>
);
const CloudDoneIcon = () => (
  <div style={style.container}>
    <CloudDone color={green800} style={style.icon} />
  </div>
);

const Alarm = () => (
  <div style={style.container}>
    <AlarmOn color={blue500} style={style.icon} />
  </div>
);

const FailedIcon = () => (
  <div style={style.container}>
    <Failed color={red500} style={style.icon} />
  </div>
);

interface SyncStatusProps {
  status: SynchronizationStatus;
}

class SyncStatus extends React.Component<SyncStatusProps, {}> {

  render() {
    switch ('' + this.props.status) {
      case SynchronizationStatus[SynchronizationStatus.Unknown]:
        return (
          <Refresh />
        );
      case SynchronizationStatus[SynchronizationStatus.Scheduled]:
        return (
          <Alarm />
        );
      case SynchronizationStatus[SynchronizationStatus.InProgress]:
        return (
          <Progress />
        );
      case SynchronizationStatus[SynchronizationStatus.Done]:
        return (
          <CloudDoneIcon />
        );
      case SynchronizationStatus[SynchronizationStatus.Failed]:
        return (
          <FailedIcon />
        );
      default:
        return (<Refresh />);
    }
  }
}
export default SyncStatus;
