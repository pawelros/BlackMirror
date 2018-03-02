import * as React from 'react';
import { RadioButton, RadioButtonGroup } from 'material-ui/RadioButton';
import ActionDateRange from 'material-ui/svg-icons/action/date-range';

const styles = {
    block: {
        maxWidth: 250,
    },
    radioButton: {
        marginBottom: 16,
    },
};

import DatePicker from 'material-ui/DatePicker';

const DatePickerExampleSimple = () => (
    <div>
        <DatePicker hintText="Date time" disabled={false} />
    </div>
);

const RadioButtonExampleSimple = () => (
    <div>
        <RadioButtonGroup name="shipSpeed" defaultSelected="not_light">
            <RadioButton
                value="not_light"
                label="Now"
                style={styles.radioButton}
                checkedIcon={<ActionDateRange style={{ color: '#F44336' }} />}
                uncheckedIcon={<ActionDateRange />}
            />
            <RadioButton
                value="light"
                label="Specific time"
                style={styles.radioButton}
                checkedIcon={<ActionDateRange style={{ color: '#F44336' }} />}
                uncheckedIcon={<ActionDateRange />}
            />
        </RadioButtonGroup>
        <DatePickerExampleSimple />
    </div>
);

class SyncSchedule extends React.Component<{}, any> {
    constructor(props: {}) {
        super(props);

        this.state = {
            dataSource: []
        };
    }

    render() {
        return (
            <RadioButtonExampleSimple />
        );
    }
}
export default SyncSchedule;
